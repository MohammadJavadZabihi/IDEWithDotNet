using IDECore.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IDECore.Service
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IConfiguration _configuration;
        public AuthenticateService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string AuthenticatJwtToken(string userId, string userName, string email)
        {
            var security = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("userName", userName));
            claimsForToken.Add(new Claim("userId", userId));
            claimsForToken.Add(new Claim("emial", email));

            var jwtSecurityToke = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.Now,
                DateTime.Now.AddDays(31),
                signingCredentials
                );

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToke);

            return tokenToReturn;
        }
    }
}
