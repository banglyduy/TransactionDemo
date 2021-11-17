using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionDemo.Actors.Messages
{
    internal class StatusMesssage
    {
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
