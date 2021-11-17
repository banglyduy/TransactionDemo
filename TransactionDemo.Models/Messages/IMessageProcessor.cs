using System;
using System.Collections.Generic;

namespace TransactionDemo.Models.Messages
{
    public interface IMessageProcessor
    {
        IBridgeMessageHandler GetMessageHandler();
    }
}
