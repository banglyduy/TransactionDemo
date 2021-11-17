using Akka.Actor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionDemo.Actors.Messages;
using TransactionDemo.Hubs;

namespace TransactionDemo.Actors.Actors
{
    internal class TransactionBridgeActor : ReceiveActor
    {
        private IServiceScope _scope;
        private readonly TransactionHubHelper _hub;
        private readonly IActorRef _accountCordinator;

        public TransactionBridgeActor(IActorRef accountCordinator, IServiceProvider sp)
        {
            _scope = sp.CreateScope();
            _hub = _scope.ServiceProvider.GetRequiredService<TransactionHubHelper>();
            _accountCordinator = accountCordinator;

            setupHub();
        }

        private void setupHub()
        {
            Receive<CreateUserMessage>(msg => _accountCordinator.Tell(msg));
            Receive<TransactionMessage>(msg => _accountCordinator.Tell(msg));
            Receive<StatusMesssage>(msg => _hub.WriteStatusMessage($"{msg.Type} : {msg.Message}"));
            Receive<List<AccountStateMessage>>(msg => _hub.NewStatistic(msg));
        }
    }
}
