using Epam.ImitationGames.Production.Domain.Authentication;
using IM.Production.Services.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IM.Production.Authentication
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IOptions<AuthenticationOptions> _options;

        public TokenGenerator(IOptions<AuthenticationOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public string Generate(IEnumerable<Claim> claims)
        {
            if (claims is null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            var key = Encoding.ASCII.GetBytes(_options.Value.Secret);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            return handler.WriteToken(handler.CreateToken(descriptor));
        }
    }
}
