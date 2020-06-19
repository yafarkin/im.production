using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using IM.Production.WebApp.Dtos;
using Epam.ImitationGames.Production.Domain.Services;
using AutoMapper;
using Epam.ImitationGames.Production.Domain.Production;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        public ITeamService _teamService;
        private readonly IMapper _mapper;

        public TeamController(ITeamService teamService, IMapper mapper)
        {
            _teamService = teamService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("buy-factory")]
        public void BuyFactory(PurchaseFactoryDataDto purchaseFactoryData)
        {
            _teamService.BuyFactory(purchaseFactoryData.Login, purchaseFactoryData.DefinitionIndex);
        }

        [HttpGet]
        [Route("get-factories-definitions")]
        public IList<FactoryDefinitionDto> GetFactoriesDefinitions(string login)
        {
            var definitions = _teamService.GetFactoriesDefinitions(login);
            var definitionsDtos = _mapper.Map<IList<FactoryDefinition>, IList<FactoryDefinitionDto>>(definitions);
            return definitionsDtos;
        }
    }
}
