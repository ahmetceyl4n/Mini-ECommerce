using eCommerceAPI.Application.Abstractions.Token;
using eCommerceAPI.Application.DTOs;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.AppUser.Commands.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        public GoogleLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings() { 
            
                Audience = new List<string> { "594603581579-tap8bm8niss1t73t0rco4af8ljui0rum.apps.googleusercontent.com" },

            };

            var payload= await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

            var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);

            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;
            if (user == null) { 
                user= await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new Domain.Entities.Identity.AppUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = payload.Email,
                        Email = payload.Email,
                        NameSurname = request.Name,

                    };
                    var identityResult =  await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;

                }
            }

            if (result)
            {
                var loginResult = await _userManager.AddLoginAsync(user, info); // Add the login information to the user. AspNetUserLogine kaydediyoruz böylelikle dışarıdan kayıt olan kullanıcılar için de giriş yapabiliyoruz ve dışarıdan kayıtlıları görebiliyoruz
              
            }
            else
            {
                throw new Exception("Invalid external authentication");
            }

            Token token = _tokenHandler.CreateAccessToken(5);

            return new()
            {
                Token = token
            };

        }
    }
}
