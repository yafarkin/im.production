using AutoMapper;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.WebApp.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _service;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public UserDto Authenticate(AuthenticationDto authentication)
        {
            var user = _service.Authenticate(authentication.Login, authentication.Password);
            return _mapper.Map<UserDto>(user);
        }
    }
}