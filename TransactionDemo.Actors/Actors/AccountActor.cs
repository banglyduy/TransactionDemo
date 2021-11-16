using System;
using Akka.Actor;
using TransactionDemo.Actors.Actors;
using TransactionDemo.Actors.Messages;
using TransactionSystem.Actors.Messages;

namespace TransactionSystem.Actors.Actors
{
    public class AccountActor : ReceiveActor
    {
        readonly int AccountId;
        public long Balance { get; private set; }
        public AccountActor(int id, long balance)
        {
            AccountId = id;
            Balance = balance;

            Receive<AddBalanceMessage>(msg => AddBalance(msg));
            Receive<TransactionMessage>(msg => HandleTransaction(msg));
            Receive<TransactionMessage>(msg => HandleTransaction(msg));
            Receive<TransactionConfirmMessage>(msg =>
            {
                Balance += msg.Amount;
                Sender.Tell(true);
            });
        }

        private async void HandleTransaction(TransactionMessage msg)
        {
           var waitTitme =  new TimeSpan(0,5,0);
            Balance -= msg.Amount;
            try
            {
                var toRef = await Context.ActorSelection($"../{msg.ToId}").ResolveOne(waitTitme);
                var success = await toRef.Ask<bool>(
                    new TransactionConfirmMessage(AccountId, msg.Amount),
                    waitTitme
                );
                if (!success)
                {
                    Balance += msg.Amount;
                }
            }catch(Exception ex)
            {
                Balance += msg.Amount;
            }
        }

        private void AddBalance(AddBalanceMessage msg)
        {
            if(msg.Id == AccountId)
            {
                Balance += msg.Amount;
            }
        }
    }
}