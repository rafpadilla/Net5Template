using Net5Template.Core.Bus;
using Net5Template.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Users.Commands
{
    public class UserRegisterCommand : ICommand<IdentityResult>
    {
        [System.Text.Json.Serialization.JsonIgnore]//para ocultar la propiedad del esquema de swagger, importante que se complete este campo en el controlador
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fingerprint { get; set; }
    }
    public class UserRegisterCommandHandler : ICommandHandler<UserRegisterCommand, IdentityResult>
    {
        private readonly UserManager<AspNetUser> _userManager;

        public UserRegisterCommandHandler(UserManager<AspNetUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new AspNetUser
            {
                Id = request.Id ?? Guid.NewGuid(),
                UserName = request.Username,
                Email = request.Email
            };

            return await _userManager.CreateAsync(user, request.Password);
        }
    }
}
