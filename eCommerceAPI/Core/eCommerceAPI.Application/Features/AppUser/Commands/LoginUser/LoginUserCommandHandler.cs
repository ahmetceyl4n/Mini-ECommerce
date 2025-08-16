using eCommerceAPI.Application.Abstractions.Token;
using eCommerceAPI.Application.DTOs;
using eCommerceAPI.Application.Exceptions;
using eCommerceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.AppUser.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {

        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;
        public LoginUserCommandHandler(SignInManager<Domain.Entities.Identity.AppUser> signInManager, ITokenHandler tokenHandler = null, UserManager<Domain.Entities.Identity.AppUser> userManager = null)
        {
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _userManager = userManager;
        }


        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(request.UsernameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            }

            if (user == null)
            {
                throw new NotFoundUserException();
            }

            SignInResult result =  await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded) // If the password is correct
            {
               // ..... Yetkileri belirlememiz gerekiyor
                Token token = _tokenHandler.CreateAccessToken(5);
                
                return new LoginUserSuccessCommandResponse()
                {
                   Token = token,
                };
            }
            throw new AuthenticationErrorException();

           
        }
    }
}
