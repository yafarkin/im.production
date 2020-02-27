using CalculationEngine;
using Epam.ImitationGames.Production.Domain.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace IM.Production.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Game _game;

        private readonly IOptions<Authentication> _options;

        public AuthenticationService(Game game, IOptions<Authentication> options)
        {
            _game = game;
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public string Authenticate(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentException("Login cannot be empty or contain white spaces.", nameof(login));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty or contain white spaces.", nameof(password));
            }

            //TODO Extract TokenGenerator to a separate project and extract AuthenticateAsAdmin and AuthenticateAsTeam
            var credentials = _options.Value.Credentials;
            var key = Encoding.ASCII.GetBytes(_options.Value.Secret);
            var handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor();
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var expires = DateTime.UtcNow.AddDays(1);

            if (login.Equals(credentials.Login) && password.Equals(credentials.Password))
            {
                descriptor.Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, credentials.Login),
                    new Claim(ClaimTypes.Role, Roles.Admin)
                });
                descriptor.Expires = expires;
                descriptor.SigningCredentials = signingCredentials;

                return handler.WriteToken(handler.CreateToken(descriptor));
            }

            var team = _game.Customers.SingleOrDefault(c => c.Login.Equals(login) && c.PasswordHash.Equals(Game.GetMD5Hash(password)));

            if (team == null)
            {
                throw new InvalidCredentialException("Credentials are incorrect.");
            }

            descriptor.Subject = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, team.Login),
                new Claim(ClaimTypes.Role, Roles.Team)
            });
            descriptor.Expires = expires;
            descriptor.SigningCredentials = signingCredentials;

            return handler.WriteToken(handler.CreateToken(descriptor));
        }
    }
}
