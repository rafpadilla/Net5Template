using AutoMapper;
using Net5Template.Core.Bus;
using Net5Template.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Identity.Commands
{
    public class AddUserPasswordCommand : ICommand
    {
        public AddUserPasswordCommand(Guid id, string password)
        {
            Id = id;
            Password = password;
        }
        public Guid Id { get; set; }
        public string Password { get; set; }
    }
    public class AddUserPasswordCommandMapping : Profile
    {
        public AddUserPasswordCommandMapping() => CreateMap<AddUserPasswordCommand, AspNetUser>();
    }
    public class AddUserPasswordCommandHandler : ICommandHandler<AddUserPasswordCommand>
    {
        private readonly UserManager<AspNetUser> _userManager;
        public AddUserPasswordCommandHandler(UserManager<AspNetUser> userManager) => _userManager = userManager;
        public async Task<Unit> Handle(AddUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString("D"));

            if (user == null)
                throw new ArgumentException();

            var result = await _userManager.AddPasswordAsync(user, request.Password);

            if (!result.Succeeded)
                throw new ArgumentException();

            return new Unit();
        }
    }
}