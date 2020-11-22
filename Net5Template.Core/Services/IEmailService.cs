using Net5Template.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendEmailAsync(string toEmail, string toName, string subject, string htmlMessage, dynamic jsonParams = null,
            IEnumerable<object> attachmentsInline = null, IEnumerable<object> attachments = null, LanguageEnum language = LanguageEnum.es_ES);
        Task<string> GetEmailTemplateHtml(string htmlMessage, string subject = "subject", dynamic jsonParams = null, LanguageEnum language = LanguageEnum.es_ES);
    }
}
