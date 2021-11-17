using Akka.Actor;
using Akka.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TransactionDemo.Actors.Actors;
using TransactionDemo.Actors.Helper;
using TransactionDemo.Models.Messages;
using TransactionSystem.Actors.Actors;

namespace TransactionDemo.Actors
{
    public class ActorSystemService : IHostedService, IMessageProcessor
    {
        private ActorSystem _actorSystem;
        private IActorRef _signalRActor;
        private readonly IServiceProvider _serviceProvider;

        private readonly IHostApplicationLifetime _applicationLifetime;

        public ActorSystemService(IServiceProvider serviceProvider, IHostApplicationLifetime appLifetime)
        {
            _serviceProvider = serviceProvider;
            _applicationLifetime = appLifetime;
        }

        public IBridgeMessageHandler GetMessageHandler()
        {
            return new BridgeMessageHandler(_signalRActor);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var boostrap = BootstrapSetup.Create();
            var di = DependencyResolverSetup.Create(_serviceProvider);
            var actorSystemSetup = boostrap.And(di);
            _actorSystem = ActorSystem.Create("TransactionSystem", actorSystemSetup);

            var accountCordinator = _actorSystem.ActorOf<AccountCordinatorActor>("AccountCordinator");
            var bridgeProps = DependencyResolver.For(_actorSystem).Props<TransactionBridgeActor>(accountCordinator);

            _signalRActor = _actorSystem.ActorOf(bridgeProps, "TransactionBridge");

            TransactionSystem.SignalRBrigde = _signalRActor;
            TransactionSystem.AccountCordinator = accountCordinator;

            _actorSystem.WhenTerminated.ContinueWith(tr => {
                _applicationLifetime.StopApplication();
            });

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await CoordinatedShutdown
                .Get(TransactionSystem.ActorSystem)
                .Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}
