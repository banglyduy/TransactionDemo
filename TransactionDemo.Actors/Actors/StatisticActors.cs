using System;
using System.Collections.Generic;
using Akka.Actor;
using TransactionDemo.Actors.Actors;
using TransactionDemo.Actors.Messages;


namespace TransactionDemo.Actors
{
    public class StatisticActor : ReceiveActor
    {
       public List<AccountStateMessage> TopList { get; set; }
       public StatisticActor()
       {
            TopList = new List<AccountStateMessage>();
            Receive<AccountStateMessage>(msg => HandleStatistic(msg));
       }

        private void HandleStatistic(AccountStateMessage msg)
        {
            var idx = TopList.FindIndex(x => x.Id == msg.Id);
            if(idx != -1) { 
                TopList[idx] = msg;
                return;
            }
            if (TopList.Count < 5)
            {
                TopList.Add(msg);
                TopList.Sort();
            }
            else
            {
                var lastidx = TopList.Count - 1;
                if (msg.Balance > TopList[TopList.Count - 1].Balance)
                {
                    TopList.RemoveAt(lastidx);
                    TopList.Add(msg);
                    TopList.Sort();
                }
            }
            TransactionSystem.SignalRBrigde.Tell(TopList);
        }
    }
}
