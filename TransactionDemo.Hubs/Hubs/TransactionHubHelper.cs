using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionDemo.Actors.Messages;

namespace TransactionDemo.Hubs
{
    public class TransactionHubHelper
    {
        private readonly IHubContext<TransactionHub> _hub;

        public TransactionHubHelper(IHubContext<TransactionHub> hub)
        {
            _hub = hub;
        }

        public void WriteStatusMessage(string message)
        {
            _hub.Clients.All.SendAsync("ReceiveStatusMessage", message);
        }

        public void NewStatistic(List<AccountStateMessage> list)
        {
            _hub.Clients.All.SendAsync("ReceiveNewStatistic", list);
        }
    }

}
