using AutoMapper;
using Net5Template.Core.Bus;
using Net5Template.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Users.Queries
{
    public class GetUserByUserNameQuery : IQuery<GetUserByUserNameDTO>
    {
        public GetUserByUserNameQuery(string username)
        {
            UserName = username;
        }
        public string UserName { get; }
    }
    public class GetUserByUserNameDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastAccessDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public bool Disabled { get; set; }
        public double Rating { get; set; }
        public int CountryId { get; set; }
        public bool Deleted { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public int LanguageId { get; set; }//Enum_Language
        public Guid? ImageUrl { get; set; }
    }
    public class GetUserByUserNameMapping : Profile
    {
        public GetUserByUserNameMapping() => CreateMap<AspNetUser, GetUserByUserNameDTO>();
    }
    public class GetUserByUserNameHandler : IQueryHandler<GetUserByUserNameQuery, GetUserByUserNameDTO>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;

        public GetUserByUserNameHandler(IMapper mapper, UserManager<AspNetUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<GetUserByUserNameDTO> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var res = await _userManager.FindByNameAsync(request.UserName);

            return _mapper.Map<GetUserByUserNameDTO>(res);
        }
    }
}