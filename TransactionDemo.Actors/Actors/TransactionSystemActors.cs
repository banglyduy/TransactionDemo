using Akka.Actor;

namespace TransactionDemo.Actors
{
    public class TransactionSystemActors
    {
        public static ActorSystem ActorSystem;

        public static IActorRef StatisticActor = ActorRefs.Nobody;
    }
}