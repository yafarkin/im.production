using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain.Authentication;
using Epam.ImitationGames.Production.Domain.Authorization;
using Epam.ImitationGames.Production.Domain.Services;
using Microsoft.Extensions.Options;

namespace IM.Production.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Game _game;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IOptions<CredentialsOptions> _options;

        public AuthenticationService(Game game, ITokenGenerator tokenGenerator, IOptions<CredentialsOptions> options)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public User Authenticate(string login, string password)
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

            if (login.Equals(credentials.Login) && password.Equals(credentials.Password))
            {
                return AuthenticateAsAdmin(credentials.Login);
            }

            if (_game.Customers.Any(c => c.Login.Equals(login) && c.PasswordHash.Equals(Game.GetMD5Hash(password))))
            {
                return AuthenticateAsTeam(login);
            }

            throw new InvalidCredentialException("Credentials are incorrect.");
        }

        private User AuthenticateAsTeam(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.Role, Roles.Team)
            };

            return new User(login, Roles.Team, _tokenGenerator.Generate(claims));
        }

        private User AuthenticateAsAdmin(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.Role, Roles.Admin)
            };

            return new User(login, Roles.Admin, _tokenGenerator.Generate(claims));
        }
    }
}
