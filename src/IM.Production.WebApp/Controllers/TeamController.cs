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
            var index = 0;
            var factoriesAndContractFactoriesList = _service.GetContractFactoriesByLogin(login).ToList();
            var result = new FactoryAndContractFactories[factoriesAndContractFactoriesList.Count];
            
            foreach (var factoryAndContractFactoriesList in factoriesAndContractFactoriesList)
            {
                var entity = _mapper?.Map<FactoryAndContractFactories>(factoryAndContractFactoriesList);
                result[index++] = entity;
            }

            return result;
        }

        [HttpGet]
        [Route("get-team-game-progress")]
        public GameProgressDto GetTeamGameProgress(string login)
        {
            var teamGameProgress = _service.GetTeamGameProgress(login);
            var gameProgressDto = new GameProgressDto() { 
                MoneyBalance = teamGameProgress.Item1, 
                RDProgress = teamGameProgress.Item2
            };
            return gameProgressDto;
        }

    }
}