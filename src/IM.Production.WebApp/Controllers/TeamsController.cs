using AutoMapper;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.WebApp.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamsService _service;
        private readonly IMapper _mapper;

        public TeamsController(ITeamsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<TeamDto> GetTeams()
        {
            var teams = _service.GetTeams();
            var teamsDtos = _mapper.Map<IEnumerable<TeamDto>>(teams);
            return teamsDtos;
        }

        [HttpPost]
        [Route("add-team")]
        public void AddTeam(NewTeamDto team)
        {
            var customer = _mapper.Map<NewTeamDto, Customer>(team);
            _service.AddTeam(customer);
        }
    }
}
