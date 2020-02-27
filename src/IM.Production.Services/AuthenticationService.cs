using Epam.ImitationGames.Production.Domain.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace IM.Production.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IOptions<Credentials> _options;

        public AuthenticationService(IOptions<Credentials> options)
        {
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

            var credentials = _options.Value;

            if (login.Equals(credentials.Login, StringComparison.InvariantCultureIgnoreCase) &&
                password.Equals(credentials.Password, StringComparison.InvariantCultureIgnoreCase))
            {
                var handler = new JwtSecurityTokenHandler();
                var descriptor = new SecurityTokenDescriptor();

                var token = handler.CreateToken(descriptor);

                return handler.WriteToken(token);
            }

            //UNDONE Authenticate Team

            throw new UnauthorizedAccessException();
        }
    }
}
