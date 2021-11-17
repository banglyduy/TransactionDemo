using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionDemo.Actors
{
    public static class TransactionSystem
    {
        public static ActorSystem ActorSystem { get; internal set; }

        public static IActorRef AccountCordinator { get; internal set; }

        public static IActorRef SignalRBrigde { get; internal set; }
    }
}
