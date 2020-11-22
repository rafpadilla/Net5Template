using AutoMapper;
using Net5Template.Core;
using Net5Template.Core.Bus;
using Net5Template.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Users.Queries
{
    public class GetUserRolesQuery : IQuery<IEnumerable<GetUserRolesDTO>>
    {
        public GetUserRolesQuery(Guid userId)
        {
            UserId = userId;
        }
        public Guid UserId { get; }
    }
    public class GetUserRolesDTO
    {
        public string Name { get; set; }
    }

    public class GetUserRolesHandler : IQueryHandler<GetUserRolesQuery, IEnumerable<GetUserRolesDTO>>
    {
        private readonly UserManager<AspNetUser> _userManager;

        public GetUserRolesHandler(UserManager<AspNetUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IEnumerable<GetUserRolesDTO>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToStringUser());

            var res = await _userManager.GetRolesAsync(user);

            if (res == null)
                throw new ArgumentException();

            return res.Select(a => new GetUserRolesDTO() { Name = a });
        }
    }
}

