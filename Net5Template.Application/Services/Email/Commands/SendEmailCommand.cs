using MediatR;
using Net5Template.Core.Bus;
using Net5Template.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Email.Commands
{
    public class SendEmailCommand : ICommand<Unit>
    {
        public SendEmailCommand(string toEmail, string toName, string subject, string htmlBody)
        {
            ToEmail = toEmail;
            ToName = toName;
            Subject = subject;
            HtmlBody = htmlBody;
        }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
    }
    public class UserLoginCommandHandler : ICommandHandler<SendEmailCommand>
    {
        private readonly IEmailService _emailService;

        public UserLoginCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            await _emailService.SendEmailAsync(request.ToEmail, request.ToName, request.Subject, request.HtmlBody);

            return new Unit();
        }
    }
}
