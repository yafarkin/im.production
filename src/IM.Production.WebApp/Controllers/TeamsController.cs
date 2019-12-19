using System;
using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.WebApp.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamsService _service;

        public TeamsController(ITeamsService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<TeamDto> GetTeams()
        {
            var teams = _service.GetTeams();

            // Map by AutoMapper to IEnumerable<CustomerDto>

            throw new NotImplementedException();
        }
    }
}
