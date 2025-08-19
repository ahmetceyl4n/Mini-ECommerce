using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Application.DTOs.User;
using eCommerceAPI.Application.Features.AppUser.Commands.CreateUser;
using eCommerceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence.Services
{
    public class UserService : IUserService
    {

        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager; // Identity IoC'ye eklediğimiz için bu olayda repository pattern kullanmıyoruz. IdentityUserManager sınıfını kullanarak işlemlerimizi yapacağız.

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponseDTO> CreateAsync(CreateUserRequestDTO request)
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

            return new CreateUserResponseDTO
            {
                Succeeded = result.Succeeded,
                Message = message
            };
        }
    }
}
