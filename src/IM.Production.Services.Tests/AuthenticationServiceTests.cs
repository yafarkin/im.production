using CalculationEngine;
using Epam.ImitationGames.Production.Domain.Authentication;
using Epam.ImitationGames.Production.Domain.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using IM.Production.CalculationEngine;
using System.Security.Authentication;

namespace IM.Production.Services.Tests
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        private Game _gameMock;
        private Logic _logicMock;
        private Mock<ITokenGenerator> _tokenGeneratorMock;
        private IOptions<CredentialsOptions> _optionsMock;
        private AuthenticationService _service;

        [TestInitialize]
        public void TestInitialize()
        {
            _gameMock = new Game();
            _logicMock = new Logic(_gameMock);
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _optionsMock = Options.Create(new CredentialsOptions { Login = "admin", Password = "admin" });
            _service = new AuthenticationService(_gameMock, _tokenGeneratorMock.Object, _optionsMock);
        }

        [TestMethod]
        public void Authenticate_NullGame_ExceptionThrown()
        {
            Game game = null;

            Assert.ThrowsException<ArgumentNullException>(() => new AuthenticationService(game, _tokenGeneratorMock.Object, _optionsMock));
        }

        [TestMethod]
        public void Authenticate_NullTokenGenerator_ExceptionThrown()
        {
            ITokenGenerator tokenGenerator = null;

            Assert.ThrowsException<ArgumentNullException>(() => new AuthenticationService(_gameMock, tokenGenerator, _optionsMock));
        }

        [TestMethod]
        public void Authenticate_NullOptions_ExceptionThrown()
        {
            IOptions<CredentialsOptions> options = null;

            Assert.ThrowsException<ArgumentNullException>(() => new AuthenticationService(_gameMock, _tokenGeneratorMock.Object, options));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Authenticate_InvalidLogin_ExceptionThrown(string login) => Assert.ThrowsException<ArgumentException>(() => _service.Authenticate(login, "password"));

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Authenticate_InvalidPassword_ExceptionThrown(string password) => Assert.ThrowsException<ArgumentException>(() => _service.Authenticate("admin", password));

        [TestMethod]
        public void Authenticate_AdminCredentials_TokenGeneratorCalledWithClaims()
        {
            var login = _optionsMock.Value.Login;
            var password = _optionsMock.Value.Password;
            _tokenGeneratorMock.Setup(g => g.Generate(It.IsAny<IEnumerable<Claim>>())).Returns("token");

            _service.Authenticate(login, password);

            _tokenGeneratorMock.Verify(g => g.Generate(It.Is<IEnumerable<Claim>>(claims =>
                claims.Any(c => c.Type == ClaimTypes.Name && c.Value == login) &&
                claims.Any(c => c.Type == ClaimTypes.Role && c.Value == Roles.Admin))), Times.Once);
        }

        [TestMethod]
        public void Authenticate_AdminCredentials_UserReturned()
        {
            _tokenGeneratorMock.Setup(g => g.Generate(It.IsAny<IEnumerable<Claim>>())).Returns("token");

            var user = _service.Authenticate(_optionsMock.Value.Login, _optionsMock.Value.Password);

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void Authenticate_AdminCredentials_UserWithLoginReturned()
        {
            var login = _optionsMock.Value.Login;
            _tokenGeneratorMock.Setup(g => g.Generate(It.IsAny<IEnumerable<Claim>>())).Returns("token");

            var user = _service.Authenticate(login, _optionsMock.Value.Password);

            Assert.AreEqual(login, user.Login);
        }

        [TestMethod]
        public void Authenticate_AdminCredentials_UserWithAdminRoleReturned()
        {
            _tokenGeneratorMock.Setup(g => g.Generate(It.IsAny<IEnumerable<Claim>>())).Returns("token");

            var user = _service.Authenticate(_optionsMock.Value.Login, _optionsMock.Value.Password);

            Assert.AreEqual(Roles.Admin, user.Role);
        }

        [TestMethod]
        public void Authenticate_AdminCredentials_UserWithTokenReturned()
        {
            const string token = "token";
            _tokenGeneratorMock.Setup(g => g.Generate(It.IsAny<IEnumerable<Claim>>())).Returns(token);

            var user = _service.Authenticate(_optionsMock.Value.Login, _optionsMock.Value.Password);

            Assert.AreEqual(token, user.Token);
        }

        [TestMethod]
        public void Authenticate_TeamCredentials_TokenGeneratorCalledWithClaims()
        {
            const string login = "team";
            const string password = "password";
            _logicMock.AddCustomer(login, password, string.Empty, default);
            _tokenGeneratorMock.Setup(g => g.Generate(It.IsAny<IEnumerable<Claim>>())).Returns("token");

            _service.Authenticate(login, password);

            _tokenGeneratorMock.Verify(g => g.Generate(It.Is<IEnumerable<Claim>>(claims =>
                claims.Any(c => c.Type == ClaimTypes.Name && c.Value == login) &&
                claims.Any(c => c.Type == ClaimTypes.Role && c.Value == Roles.Team))), Times.Once);
        }

        [TestMethod]
        public void Authenticate_TeamCredentials_UserReturned()
        {
            const string login = "team";
            const string password = "password";
            _logicMock.AddCustomer(login, password, default, default);
            _tokenGeneratorMock.Setup(g => g.Generate(It.IsAny<IEnumerable<Claim>>())).Returns("token");

            var user = _service.Authenticate(login, password);

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void Authenticate_TeamCredentials_UserWithLoginReturned()
        {
            const string login = "team";
            const string password = "password";
            _logicMock.AddCustomer(login, password, default, default);
            _tokenGeneratorMock.Setup(g => g.Generate(It.IsAny<IEnumerable<Claim>>())).Returns("token");

            var user = _service.Authenticate(login, password);

            Assert.AreEqual(login, user.Login);
        }

        [TestMethod]
        public void Authenticate_TeamCredentials_UserWithTeamRoleReturned()
        {
            const string login = "team";
            const string password = "password";
            _logicMock.AddCustomer(login, password, default, default);
            _tokenGeneratorMock.Setup(g => g.Generate(It.IsAny<IEnumerable<Claim>>())).Returns("token");

            var user = _service.Authenticate(login, password);

            Assert.AreEqual(Roles.Team, user.Role);
        }

        [TestMethod]
        public void Authenticate_TeamCredentials_UserWithTokenReturned()
        {
            const string login = "team";
            const string password = "password";
            const string token = "token";
            _logicMock.AddCustomer(login, password, default, default);
            _tokenGeneratorMock.Setup(g => g.Generate(It.IsAny<IEnumerable<Claim>>())).Returns(token);

            var user = _service.Authenticate(login, password);

            Assert.AreEqual(token, user.Token);
        }

        [TestMethod]
        public void Authenticate_InvalidCredentials_ExceptionThrown()
        {
            Assert.ThrowsException<InvalidCredentialException>(() => _service.Authenticate("team", "password"));
        }
    }
}
