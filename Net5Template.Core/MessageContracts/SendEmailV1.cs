using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.MessageContracts
{
    public class SendEmailV1
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
    }
}
