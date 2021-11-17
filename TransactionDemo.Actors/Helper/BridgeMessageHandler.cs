using Akka.Actor;
using TransactionDemo.Actors.Actors;
using TransactionDemo.Models.Messages;
using TransactionDemo.Actors.Messages;

namespace TransactionDemo.Actors.Helper
{
    public class BridgeMessageHandler : IBridgeMessageHandler
    {
        private IActorRef TransactionBridgeActor { get; set; }
        public BridgeMessageHandler(IActorRef transactionBridgeActor)
        {
            TransactionBridgeActor = transactionBridgeActor;
        }

        public void AddAccount(int id, long startingBalance)
        {
            TransactionBridgeActor.Tell(new CreateUserMessage(id, startingBalance));
        }

        public void Transfer(int fromId, int toId, long amount)
        {
            TransactionBridgeActor.Tell(new TransactionMessage(fromId, toId, amount));
        }
    }
}
