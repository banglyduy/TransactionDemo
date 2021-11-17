using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionDemo.Actors.Messages
{
    public class AccountStateMessage : IComparable<AccountStateMessage>
    {
        public int Id { get; private set; }
        public long Balance { get; private set; }
        public AccountStateMessage(int id, long balance)
        {
            Id = id;
            Balance = balance;
        }

        public int CompareTo(AccountStateMessage other)
        {
            return other.Balance.CompareTo(Balance);
        }
    }
}
