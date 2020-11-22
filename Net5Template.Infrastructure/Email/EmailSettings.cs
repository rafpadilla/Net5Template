using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Email
{
    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int? MailPort { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string SMTPUser { get; set; }
        public string SMTPPassword { get; set; }
        public bool UseSSL { get; set; }
        public string DevMail { get; set; }
        public string MailLayoutPath { get; set; }
    }
}
