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
    public class RegisterUserNoPasswordCommand : ICommand
    {
        public RegisterUserNoPasswordCommand(Guid id, string email, string userName)
        {
            Id = id;
            Email = email;
            UserName = userName;
        }

        public Guid Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
    }
    public class RegisterUserNoPasswordCommandMapping : Profile
    {
        public RegisterUserNoPasswordCommandMapping() => CreateMap<RegisterUserNoPasswordCommand, AspNetUser>();
    }
    public class RegisterUserNoPasswordCommandHandler : ICommandHandler<RegisterUserNoPasswordCommand>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;

        public RegisterUserNoPasswordCommandHandler(IMapper mapper, UserManager<AspNetUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(RegisterUserNoPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<AspNetUser>(request);

            var queryUser = await _userManager.FindByEmailAsync(request.Email);

            if (queryUser != null)
                throw new ArgumentException("Email is in use by another user");

            var result = await _userManager.CreateAsync(user);

            if(!result.Succeeded)
                throw new ArgumentException();

            return new Unit();
        }
    }
}
