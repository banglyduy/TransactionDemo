using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TransactionDemo.Models.Messages;

namespace TransactionDemo.Hubs
{
    public class TransactionHub : Hub 
    {
        private readonly IBridgeMessageHandler _handler;
        public TransactionHub(IMessageProcessor processor)
        {
            _handler = processor.GetMessageHandler();
        }

        public Task InitilizeAccounts(int id, long amount)
        {
            _handler.AddAccount(id,amount);
            return Task.CompletedTask;
        }

        public Task InitTransaction(int fromid, int toid, long amount)
        {
            _handler.Transfer(fromid,toid, amount);
            return Task.CompletedTask;
        }
    }
}
