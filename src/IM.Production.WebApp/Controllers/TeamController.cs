using AutoMapper;
using System.Linq;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.WebApp.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            var result = new List<FactoryAndContractFactories>();
            foreach (var factoryAndContractFactoriesList in factoriesAndContractFactoriesList)
            {
                var entity = _mapper?.Map<FactoryAndContractFactories>(factoryAndContractFactoriesList);
                result.Add(entity);
            }
            return result.ToArray();
        }

    }
}