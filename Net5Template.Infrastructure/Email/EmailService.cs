using Net5Template.Core;
using Net5Template.Core.Enums;
using Net5Template.Core.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly EmailSettings _emailSettings;

        public EmailService(IEmailSender emailSender, IOptions<EmailSettings> emailSettings)
        {
            _emailSender = emailSender;
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return SendEmailAsync(email, email, subject, htmlMessage);
        }

        public async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlMessage, dynamic jsonParams = null, IEnumerable<object> attachmentsInline = null, IEnumerable<object> attachments = null, LanguageEnum language = LanguageEnum.es_ES)
        {
            var html = await BuildMessage(htmlMessage, subject, jsonParams == null ? string.Empty : JsonConvert.SerializeObject(jsonParams), language);

            await _emailSender.SendEmailAsync(toEmail, toName, subject, html, (EmailAttachments)attachmentsInline, (EmailAttachments)attachments);
        }
        /// <summary>
        /// Returns template with body in html for client presentation, draft...
        /// </summary>
        /// <param name="htmlMessage"></param>
        /// <param name="subject"></param>
        /// <param name="jsonParams"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public Task<string> GetEmailTemplateHtml(string htmlMessage, string subject = "subject", dynamic jsonParams = null, LanguageEnum language = LanguageEnum.es_ES)
        {
            return BuildMessage(htmlMessage, subject, jsonParams == null ? string.Empty : JsonConvert.SerializeObject(jsonParams), language);
        }
        private async Task<string> BuildMessage(string htmlMessage, string subject, string jsonParams = null, LanguageEnum language = LanguageEnum.es_ES)
        {
            var mailTemplate = await GetMailTemplate(language);
            if (string.IsNullOrEmpty(mailTemplate) || !mailTemplate.Contains("{{body}}"))
            {
                throw new InvalidDataException("MailTemplate does not contain {{body}} format param");
            }

            var fullBodyHtml = htmlMessage;

            if (!string.IsNullOrEmpty(jsonParams))
            {
                var json = JsonConvert.DeserializeObject(jsonParams);
                if (json != null && json is JObject)
                {
                    foreach (var param in json as JObject)
                    {
                        fullBodyHtml = fullBodyHtml.Replace("{" + param.Key + "}", param.Value.ToString());
                    }
                }
            }

            mailTemplate = mailTemplate
                .Replace("{{body}}", fullBodyHtml)
                .Replace("{{subject}}", subject);

            return mailTemplate;
        }
        private async Task<string> GetMailTemplate(LanguageEnum language = LanguageEnum.es_ES)
        {
            string htmlBody;
            var path = Directory.GetParent(this.GetType().Assembly.Location).FullName + Path.DirectorySeparatorChar.ToString()
                + string.Format(_emailSettings.MailLayoutPath, language.GetDescription());
            using (StreamReader sr = File.OpenText(path))
            {
                htmlBody = await sr.ReadToEndAsync();
            }
            return htmlBody;
        }
    }
}
