using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Net5Template.Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(
            IOptions<EmailSettings> emailSettings,
            IWebHostEnvironment env,
            ILogger<EmailSender> logger)
        {
            _emailSettings = emailSettings.Value;
            _env = env;
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage, EmailAttachments attachmentsInline = null, EmailAttachments attachments = null)
        {
            return SendEmailAsync(email, email, subject, htmlMessage, attachmentsInline, attachments);
        }
        public async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlMessage, EmailAttachments attachmentsInline = null, EmailAttachments attachments = null)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));

                if (!_env.IsProduction())
                {
                    mimeMessage.To.Add(new MailboxAddress("Net5Template[Dev]", _emailSettings.DevMail));
                }
                else
                {
                    mimeMessage.To.Add(new MailboxAddress(toName, toEmail));
                }

                mimeMessage.Subject = subject;

                static string GetString(string str)
                {
                    Regex rx = new Regex("<[^>]*>");
                    str = rx.Replace(str, "");
                    return HttpUtility.HtmlDecode(str);
                }

                var builder = new BodyBuilder
                {
                    TextBody = GetString(htmlMessage)
                };

                if (attachmentsInline != null && attachmentsInline.Attachments.Count > 0)
                {
                    foreach (var att in attachmentsInline.Attachments)
                    {
                        att.ContentId = MimeUtils.GenerateMessageId();
                        builder.LinkedResources.Add(att);
                    }
                    //use->builder.HtmlBody =string.Format(@"<img src=""cid:{0}"">", image.ContentId);//in order {0}{1}
                    htmlMessage = string.Format(htmlMessage, attachmentsInline.Attachments);
                }
                builder.HtmlBody = htmlMessage;

                if (attachments != null && attachments.Attachments.Count > 0)
                {
                    foreach (var att in attachments.Attachments)
                    {
                        builder.Attachments.Add(att);
                    }
                }

                mimeMessage.Body = builder.ToMessageBody();

                using var client = new SmtpClient();
                //[temp]accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                if (!_emailSettings.MailPort.HasValue)
                    await client.ConnectAsync(_emailSettings.MailServer);
                else
                    await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort.Value, _emailSettings.UseSSL);

                if (!string.IsNullOrEmpty(_emailSettings.SMTPUser) && !string.IsNullOrEmpty(_emailSettings.SMTPPassword))
                {
                    await client.AuthenticateAsync(_emailSettings.SMTPUser, _emailSettings.SMTPPassword);
                }

                await client.SendAsync(mimeMessage);

                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
            }
        }
    }
}
