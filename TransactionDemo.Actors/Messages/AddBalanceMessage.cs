using System;

namespace TransactionSystem.Actors.Messages
{
    public class AddBalanceMessage
    {
        public int Id { get; private set; }
        public long Amount { get; private set; }
        public DateTimeOffset time { get; private set; }
        public AddBalanceMessage(int id, long amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}