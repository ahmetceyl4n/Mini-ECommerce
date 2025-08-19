using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Application.DTOs.User;
using eCommerceAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace eCommerceAPI.Application.Features.AppUser.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        
        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {

            CreateUserResponseDTO response = await _userService.CreateAsync(
                new()
                {
                    NameSurname = request.NameSurname,
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = request.Password,
                    PasswordConfirm = request.PasswordConfirm
                }
            );

            return new CreateUserCommandResponse
            {
                Succeeded = response.Succeeded,
                Message = response.Message
            };


        }
    }
}
