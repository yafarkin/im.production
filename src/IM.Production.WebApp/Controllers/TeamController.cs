using AutoMapper;
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
        public IEnumerable<FactoryDto> GetTeamFactories(string login)
        {
            var factories = _service.GetFactoriesByLogin(login);
            var result = new List<FactoryDto>();
            foreach (var factory in factories)
            {
                var dto = _mapper?.Map<FactoryDto>(factory);
                result.Add(dto);
            }
            return result;
        }

        [HttpGet]
        [Route("get-team-contract-factories")]
        public IEnumerable<FactoryDto[]> GetTeamContractFactories(string login)
        {
            var contractFactoriesList = _service.GetContractFactoriesByLogin(login);
            var result = new List<FactoryDto[]>();
            foreach (var contractFactories in contractFactoriesList)
            {
                var list = new List<FactoryDto>();
                foreach (var factory in contractFactories)
                {
                    var dto = _mapper?.Map<FactoryDto>(factory);
                    list.Add(dto);
                }
                result.Add(list.ToArray());
            }
            return result;
        }
    }
}
