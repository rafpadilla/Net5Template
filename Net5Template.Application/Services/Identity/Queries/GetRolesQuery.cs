using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Net5Template.Core.Bus;
using Net5Template.Core.Entities;

namespace Net5Template.Application.Services.Identity.Queries
{
    public class GetRolesQuery : IQuery<IEnumerable<string>>
    {
        public GetRolesQuery(string userId) => UserId = userId;
        public string UserId { get; set; }
    }
    public class GetRolesHandler : IQueryHandler<GetRolesQuery, IEnumerable<string>>
    {
        private readonly UserManager<AspNetUser> _userManager;
        public GetRolesHandler(UserManager<AspNetUser> userManager) => _userManager = userManager;
        public async Task<IEnumerable<string>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
                throw new ArgumentException();

            return await _userManager.GetRolesAsync(user);
        }
    }
}
