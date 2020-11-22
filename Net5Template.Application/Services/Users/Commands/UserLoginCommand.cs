using Net5Template.Application.Services.Users.Queries;
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
    public class UserLoginCommand : ICommand<SignInResult>
    {
        public UserLoginCommand(string userOrEmail, string password, string fingerprint)
        {
            UserOrEmail = userOrEmail;
            Password = password;
            Fingerprint = fingerprint;
        }

        public string UserOrEmail { get; set; }
        public string Password { get; set; }
        public string Fingerprint { get; set; }
    }
    public class UserLoginCommandHandler : ICommandHandler<UserLoginCommand, SignInResult>
    {
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly IQueryBus _queryBus;

        public UserLoginCommandHandler(SignInManager<AspNetUser> signInManager, IQueryBus queryBus)
        {
            _signInManager = signInManager;
            _queryBus = queryBus;
        }
        public async Task<SignInResult> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            if (request.UserOrEmail.Contains("@"))
            {
                var user = await _queryBus.Send(new GetUserByEmailQuery(request.UserOrEmail));
                if (user != null)
                    return await _signInManager.PasswordSignInAsync(user.UserName, request.Password, isPersistent: false, lockoutOnFailure: false);
            }
            else
            {
                var user = await _queryBus.Send(new GetUserByUserNameQuery(request.UserOrEmail));
                if (user != null)
                    return await _signInManager.PasswordSignInAsync(user.UserName, request.Password, isPersistent: false, lockoutOnFailure: false);
            }
            return SignInResult.Failed;
        }
    }
}
