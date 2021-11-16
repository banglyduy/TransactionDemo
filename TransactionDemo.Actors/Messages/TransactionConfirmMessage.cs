using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionDemo.Actors.Messages
{
    internal class TransactionConfirmMessage
    {
        public int FromId { get; private set; }
        public long Amount { get; private set; }
        public TransactionConfirmMessage(int from, long amount)
        {
            FromId = from;
            Amount = amount;
        }
    }
}
