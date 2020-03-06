using AutoMapper;
using Epam.ImitationGames.Production.Domain.Authentication;
using Epam.ImitationGames.Production.Domain.Authorization;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.WebApp.Controllers;
using IM.Production.WebApp.Dtos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace IM.Production.WebApp.Tests.Controllers
{
    [TestClass]
    public class AuthenticationControllerTests
    {
        private Mock<IMapper> _mapperMock;

        private Mock<IAuthenticationService> _serviceMock;

        private AuthenticationController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mapperMock = new Mock<IMapper>();
            _serviceMock = new Mock<IAuthenticationService>();
            _controller = new AuthenticationController(_serviceMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Constructor_NullService_ExceptionThrown()
        {
            IAuthenticationService service = null;

            Assert.ThrowsException<ArgumentNullException>(() => new AuthenticationController(service, _mapperMock.Object));
        }

        [TestMethod]
        public void Constructor_NullMapper_ExceptionThrown()
        {
            IMapper mapper = null;

            Assert.ThrowsException<ArgumentNullException>(() => new AuthenticationController(_serviceMock.Object, mapper));
        }

        [TestMethod]
        public void Authenticate_ValidAuthentication_ServiceCalledToAuthenticate()
        {
            var authentication = new AuthenticationDto { Login = "team", Password = "password" };

            _controller.Authenticate(authentication);

            _serviceMock.Verify(s => s.Authenticate(authentication.Login, authentication.Password), Times.Once);
        }

        [TestMethod]
        public void Authenticate_ValidAuthentication_MapperCalledToMapUser()
        {
            var user = new User("team", Roles.Team, "token");
            _serviceMock.Setup(s => s.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(user);

            _controller.Authenticate(new AuthenticationDto());

            _mapperMock.Verify(m => m.Map<UserDto>(user), Times.Once);
        }

        [TestMethod]
        public void Authenticate_ValidAuthentication_MapperReturnedMappedUser()
        {
            var mapped = new UserDto();
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(mapped);

            var actual = _controller.Authenticate(new AuthenticationDto());

            Assert.AreEqual(mapped, actual);
        }
    }
}
