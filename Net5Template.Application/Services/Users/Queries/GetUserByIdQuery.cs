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
    public class GetUserByIdQuery : IQuery<GetUserByIdDTO>
    {
        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
        public Guid UserId { get; }
    }
    public class GetUserByIdDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
    public class GetUserByIdMapping : Profile
    {
        public GetUserByIdMapping() => CreateMap<AspNetUser, GetUserByIdDTO>();
    }
    public class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, GetUserByIdDTO>
    {
        private readonly IAspNetUserRepository _aspNetUserRepository;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IAspNetUserRepository aspNetUserRepository, IMapper mapper)
        {
            _aspNetUserRepository = aspNetUserRepository;
            _mapper = mapper;
        }
        public async Task<GetUserByIdDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _aspNetUserRepository.GetById(request.UserId);

            if (res == null)
                throw new ArgumentException();

            return _mapper.Map<GetUserByIdDTO>(res);
        }
    }
}
