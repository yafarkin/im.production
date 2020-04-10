using AutoMapper;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.WebApp.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamsService _teamService;
        private readonly IFactoriesService _factoriesService;
        private readonly IMapper _mapper;

        public TeamController(ITeamsService service, IFactoriesService factoriesService, IMapper mapper)
        {
            _teamService = service;
            _factoriesService = factoriesService;
            _mapper = mapper;
        }

        //TODO Extract to FactoriesController
        [HttpGet]
        [Route("factories")]
        public FactoryDto[] GetTeamFactories(string login)
        {
            var factoriesAndContractFactories = _factoriesService.GetFactoriesByLogin(login);
            return _mapper?.Map<FactoryDto[]>(factoriesAndContractFactories);
        }

        //TODO Extract to TeamsController
        [HttpGet]
        [Route("get-team-progress")]
        public TeamProgressDto GetTeamProgress(string login)
        {
            var teamGameProgress = _teamService.GetTeamProgress(login);
            return _mapper?.Map<TeamProgressDto>(teamGameProgress);
        }

    }
}