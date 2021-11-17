using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionDemo.Models.Messages
{
    public interface IBridgeMessageHandler
    {
        void AddAccount(int id, long startingBalance);
        void Transfer(int fromId, int toId, long amount);
    }
}
