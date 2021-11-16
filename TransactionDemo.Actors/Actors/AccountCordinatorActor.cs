using System;
using System.Collections.Generic;
using Akka.Actor;
using TransactionDemo.Actors.Actors;
using TransactionSystem.Actors.Messages;

namespace TransactionSystem.Actors.Actors
{
    public class AccountCordinatorActor : ReceiveActor
    {
        private readonly Dictionary<int, IActorRef> AccountList;

        public AccountCordinatorActor()
        {
            AccountList = new Dictionary<int, IActorRef>();
            Receive<AddBalanceMessage>(msg => AddBalance(msg));
            Receive<CreateUserMessage>(msg => CreateAccount(msg));
            Receive<TransactionMessage>(msg => TransactionHandler(msg));
        }

        private void CreateAccount(CreateUserMessage msg)
        {
            if (!AccountList.TryGetValue(msg.Id, out var account))
            {
                var props = Props.Create(() => new AccountActor(msg.Id, msg.StartingBalance));
                var newUserActor = Context.ActorOf(props, msg.Id.ToString());
                AccountList.Add(msg.Id, newUserActor);
            }
            else
            {
                throw new Exception("User already exist");
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