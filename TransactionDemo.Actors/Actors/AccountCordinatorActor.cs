using System;
using System.Collections.Generic;
using Akka.Actor;
using TransactionDemo.Actors;
using TransactionDemo.Actors.Actors;
using TransactionDemo.Actors.Messages;

namespace TransactionSystem.Actors.Actors
{
    public class AccountCordinatorActor : ReceiveActor
    {
        private readonly Dictionary<int, IActorRef> AccountList;
        private readonly IActorRef StatisticActor;
        public AccountCordinatorActor()
        {
            StatisticActor = Context.ActorOf<StatisticActor>("StatisticActor");
            AccountList = new Dictionary<int, IActorRef>();
            Receive<AddBalanceMessage>(msg => AddBalance(msg));
            Receive<CreateUserMessage>(msg => CreateAccount(msg));
            Receive<TransactionMessage>(msg => TransactionHandler(msg));
            Receive<AccountStateMessage>(msg => StatisticActor.Tell(msg));
        }

        private void CreateAccount(CreateUserMessage msg)
        {
            if (!AccountList.TryGetValue(msg.Id, out var account))
            {
                var props = Props.Create(() => new AccountActor(msg.Id, msg.StartingBalance));
                var newUserActor = Context.ActorOf(props, msg.Id.ToString());
                AccountList.Add(msg.Id, newUserActor);
                StatisticActor.Tell(new AccountStateMessage(msg.Id, msg.StartingBalance));
                Sender.Tell(new StatusMesssage()
                {
                    Message = $"User with Id {msg.Id} created with starting balance: {msg.StartingBalance}",
                    Type= "Create Account"
                });
            }
            else
            {
                Sender.Tell(new StatusMesssage()
                {
                    Message = $"User with ID {msg.Id} already exist",
                    Type = "Create Account"
                });
            }
        }

        private void AddBalance(AddBalanceMessage msg)
        {
            if (!AccountList.TryGetValue(msg.Id, out var account))
            {
                account.Forward(msg);
            }
            else
            {
                this.CreateAccount(new CreateUserMessage(msg.Id, msg.Amount));
            }
        }

        private void TransactionHandler(TransactionMessage msg)
        {
            if(AccountList.TryGetValue(msg.FromId, out var fromaccount) 
                && AccountList.TryGetValue(msg.ToId, out var toaccount))
            {
                fromaccount.Forward(msg.setReferActor(toaccount));
            }
        }
    }
}