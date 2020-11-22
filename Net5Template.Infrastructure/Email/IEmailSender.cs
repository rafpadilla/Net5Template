using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage, EmailAttachments attachmentsInline = null, EmailAttachments attachments = null);
        Task SendEmailAsync(string toEmail, string toName, string subject, string htmlMessage, EmailAttachments attachmentsInline = null, EmailAttachments attachments = null);
    }
}
