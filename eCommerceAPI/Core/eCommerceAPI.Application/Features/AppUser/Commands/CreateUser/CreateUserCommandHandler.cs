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
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager; // Identifyı IoCye eklediğimiz için bu olayda repository pattern kullanmıyoruz. IdentityUserManager sınıfını kullanarak işlemlerimizi yapacağız.

        public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.CreateAsync(new Domain.Entities.Identity.AppUser
            {
                NameSurname = request.NameSurname,
                UserName = request.UserName,
                Email = request.Email
            }, request.Password);

            var message = result.Succeeded
                ? "User created successfully."
                : string.Join(", ", result.Errors.Select(e => $"{e.Code} - {e.Description}"));

            return new CreateUserCommandResponse
            {
                Succeeded = result.Succeeded,
                Message = message
            };





        }
    }
}
