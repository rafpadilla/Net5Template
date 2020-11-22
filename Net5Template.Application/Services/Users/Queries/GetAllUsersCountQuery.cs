using AutoMapper;
using Net5Template.Core.Bus;
using Net5Template.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Users.Queries
{
    public class GetAllUsersCountQuery : IQuery<int>
    {
    }
    public class GetAllUsersCountHandler : IQueryHandler<GetAllUsersCountQuery, int>
    {
        private readonly IAspNetUserRepository _aspNetUserRepository;

        public GetAllUsersCountHandler(IAspNetUserRepository aspNetUserRepository)
        {
            _aspNetUserRepository = aspNetUserRepository;
        }
        public async Task<int> Handle(GetAllUsersCountQuery request, CancellationToken cancellationToken)
        {
            return await _aspNetUserRepository.GetAllCount();
        }
    }
}
