using AutoMapper;
using System.Linq;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.WebApp.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _service;
        private readonly IMapper _mapper;

        public TeamController(ITeamService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("get-team-factories")]
        public FactoryAndContractFactories[] GetTeamFactories(string login)
        {
            var factoriesAndContractFactoriesList = _service.GetContractFactoriesByLogin(login).ToList();
            return _mapper?.Map<FactoryAndContractFactories[]>(factoriesAndContractFactoriesList);
        }

        [HttpGet]
        [Route("get-team-game-progress")]
        public GameProgressDto GetTeamGameProgress(string login)
        {
            var teamGameProgress = _service.GetTeamGameProgress(login);
            return _mapper?.Map<GameProgressDto>(teamGameProgress);
        }

    }
}