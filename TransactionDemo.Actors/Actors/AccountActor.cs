using System;
using Akka.Actor;
using TransactionDemo.Actors.Messages;

namespace TransactionDemo.Actors.Actors
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
                TransactionSystem.AccountCordinator.Tell(new AccountStateMessage(AccountId, Balance));
                Sender.Tell(true);
            });
        }

        private async void HandleTransaction(TransactionMessage msg)
        {
           var waitTitme =  new TimeSpan(0,5,0);    
            var sender = Sender;
            try
            {
                if (Balance < msg.Amount) throw new Exception("Not enough money in account");
                Balance -= msg.Amount;
                var success = await msg.Actor.Ask<bool>(
                    new TransactionConfirmMessage(AccountId, msg.Amount),
                    waitTitme
                );
                if (success)
                {
                    TransactionSystem.AccountCordinator.Tell(new AccountStateMessage(AccountId, Balance));
                    sender.Tell(new StatusMesssage()
                    {
                        Message = $"From account with Id {msg.FromId} to id {msg.ToId} with amount: {msg.Amount}",
                        Type = "Transfer"
                    });
                }
                else
                {
                    Balance += msg.Amount;
                    sender.Tell(new StatusMesssage()
                    {
                        Message = $"Failed to tranfer from account with Id {msg.FromId} to id {msg.ToId}.",
                        Type = "Transfer"
                    });
                }
            }catch(Exception ex)
            {
                Balance += msg.Amount;
                sender.Tell(new StatusMesssage()
                {
                    Message = $"Failed to tranfer from account with Id {msg.FromId} to id {msg.ToId}:" + ex,
                    Type = "Transfer"
                });
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