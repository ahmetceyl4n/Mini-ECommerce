using eCommerceAPI.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int second)
        {
            Application.DTOs.Token token = new();
            
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));  // Token'ın imzasını doğrulamak için kullanılan anahtar. simetrik bir anahtar kullanılır.

            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256); // Token'ın imzasını oluşturmak için kullanılan imzalama kimlik bilgileri. şifreli kimlik bilgileri oluşturur.

            token.AccessTokenExpiration = DateTime.UtcNow.AddMinutes(second); // Token'ın geçerlilik süresi. UTC zaman diliminde geçerli olacak şekilde ayarlanır.

            JwtSecurityToken jwtSecurityToken = new( 
                audience: _configuration["Token:Audience"], // tokeni kimler kullanabilir
                issuer: _configuration["Token:Issuer"], // tokeni kim üretti
                expires: token.AccessTokenExpiration,
                notBefore : DateTime.UtcNow, // üretildiği andan ne kadar zaman sonra devreye girsin onu ayarlar
                signingCredentials: signingCredentials // imza bigileri 

                );

            // token oluşturu sınıftan bir örnek alıyoruz 

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            token.AccessToken = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
            return token;


        }
    }
}
