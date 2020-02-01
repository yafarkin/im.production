using System;
using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.WebApp.Dtos;
using Microsoft.AspNetCore.Mvc;
using Epam.ImitationGames.Production.Domain;
using AutoMapper;
using Epam.ImitationGames.Production.Domain.Production;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/teams")]
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
            var Teams = _mapper.Map<IEnumerable<Customer>, IEnumerable<TeamDto>>(teams);
            return Teams;
        }
    }
}
