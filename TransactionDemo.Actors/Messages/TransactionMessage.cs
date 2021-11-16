using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionDemo.Actors.Actors
{
    public class TransactionMessage
    {
        public int FromId { get; private set; }
        public int ToId { get; private set; }
        public long Amount { get; private set; }
        public IActorRef Actor { get; private set; }
        public TransactionMessage(int from,int to, long amount)
        {
            FromId = from;
            ToId = to;
            Amount = amount;
        }

        public TransactionMessage setReferActor(IActorRef reference)
        {
            Actor = reference;
            return this;
        }
    }
}
