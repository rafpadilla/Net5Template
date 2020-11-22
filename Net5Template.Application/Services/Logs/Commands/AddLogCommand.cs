using Net5Template.Core.Bus;
using Net5Template.Core.Entities;
using Net5Template.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Logs.Commands
{
    public class AddLogCommand : ICommand<int> //this is only a sample, logs are added using ILogger, returns the log Id (int)
    {
        public AddLogCommand(string message)
        {
            Message = message;
        }
        public string Message { get; }
    }
    public class AddLogCommandHandler : ICommandHandler<AddLogCommand, int>
    {
        private readonly ILogRepository _logRepository;

        public AddLogCommandHandler(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        public async Task<int> Handle(AddLogCommand request, CancellationToken cancellationToken)
        {
            //you can use Automapper to map command to entity
            //Ids could be generated on webapi/frontend
            var newLog = new Log()
            {
                Message = request.Message,
                Level = "Information",
                TimeStamp = DateTime.Now,
                Properties = "<properties></properties>"
            };

            await _logRepository.Add(newLog);

            return newLog.Id;
        }
    }
}