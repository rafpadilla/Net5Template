using AutoMapper;
using Net5Template.Core.Bus;
using Net5Template.Core.Entities;
using Net5Template.Core.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Users.Queries
{
    public class GetAllUsersQuery : IQuery<IEnumerable<GetAllUsersDTO>>
    {
        public GetAllUsersQuery(int pageIndex = 1, int pageSize = 20)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllUsersDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
    public class GetAllUsersMapping : Profile
    {
        public GetAllUsersMapping() => CreateMap<AspNetUser, GetAllUsersDTO>();
    }
    public class GetAllUsersHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<GetAllUsersDTO>>
    {
        private readonly IAspNetUserRepository _aspNetUserRepository;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(IAspNetUserRepository aspNetUserRepository, IMapper mapper)
        {
            _aspNetUserRepository = aspNetUserRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetAllUsersDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var results = await _aspNetUserRepository.GetAll(request.PageIndex, request.PageSize);

            return _mapper.Map<IEnumerable<GetAllUsersDTO>>(results);
        }
    }
}
