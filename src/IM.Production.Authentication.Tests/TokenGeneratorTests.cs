using IM.Production.Services.Options;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace IM.Production.Authentication.Tests
{
    [TestClass]
    public class TokenGeneratorTests
    {
        private IOptions<AuthenticationOptions> _optionsMock;
        private JwtSecurityTokenHandler _tokenHandler;
        private TokenGenerator _tokenGenerator;

        [TestInitialize]
        public void TestInitialize()
        {
            _optionsMock = Options.Create<AuthenticationOptions>(new AuthenticationOptions { Secret = "SecretForGeneratingJWT" });
            _tokenHandler = new JwtSecurityTokenHandler();
            _tokenGenerator = new TokenGenerator(_optionsMock);
        }

        [TestMethod]
        public void Constructor_NullOptions_ExceptionThrown()
        {
            IOptions<AuthenticationOptions> options = null;

            Assert.ThrowsException<ArgumentNullException>(() => new TokenGenerator(options));
        }

        [TestMethod]
        public void Generate_NullClaims_ExceptionThrown()
        {
            IEnumerable<Claim> claims = null;

            Assert.ThrowsException<ArgumentNullException>(() => _tokenGenerator.Generate(claims));
        }

        [TestMethod]
        public void Generate_NotNullClaims_NotNullTokenGenerated()
        {
            var claims = Enumerable.Empty<Claim>();

            var token = _tokenGenerator.Generate(claims);

            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void Generate_AnyClaims_ValidTokenGenerated()
        {
            var claims = Enumerable.Empty<Claim>();

            var token = _tokenGenerator.Generate(claims);

            Assert.IsTrue(_tokenHandler.CanReadToken(token));
        }

        [TestMethod]
        public void Generate_AnyClaims_ClaimsContained()
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "admin") };

            var token = _tokenGenerator.Generate(claims);

            var validated = _tokenHandler.ReadToken(token);
            Assert.IsNotNull(validated);
        }
    }
}
