using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.WebApp.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AuthenticationController(IAuthenticationService service)
        {
            _service = service ?? throw new System.ArgumentNullException(nameof(service));
        }

        [HttpPost]
        public string Authenticate(AuthenticationDto authentication)
        {
            return _service.Authenticate(authentication.Login, authentication.Password);
        }
    }
}