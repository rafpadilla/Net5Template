using AutoMapper;
using Net5Template.Core.Bus;
using Net5Template.Core.Entities;
using Net5Template.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Logs.Queries
{
    public class GetLogsQuery : IQuery<IEnumerable<GetLogsDTO>>
    {
    }
    public class GetLogsDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
    }
    public class GetLogsMapping : Profile
    {
        public GetLogsMapping() => CreateMap<Log, GetLogsDTO>();
    }
    public class GetLogsHandler : IQueryHandler<GetLogsQuery, IEnumerable<GetLogsDTO>>
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;

        public GetLogsHandler(ILogRepository logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetLogsDTO>> Handle(GetLogsQuery request, CancellationToken cancellationToken)
        {
            var results = await _logRepository.GetLogs();

            return _mapper.Map<IEnumerable<GetLogsDTO>>(results);
        }
    }
}
