using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Base;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.CalculationEngine;
using IM.Production.WebApp.Dtos;
using IM.Production.WebApp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly IContractsService _service;
        private readonly IMapper _mapper;

        public ContractsController(IContractsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("single")]
        public ContractDto GetContract()
        {
            var customerContracts = _service.GetContracts().ToArray();
            return _mapper?.Map<ContractDto>(customerContracts[10]);
        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<ContractDto> GetContracts()
        {
            var customerContracts = _service.GetContracts();
            var result = new List<ContractDto>();
            foreach (var element in customerContracts)
            {
                var dto = _mapper?.Map<ContractDto>(element);
                result.Add(dto);
            }

            return result;
        }
    }
}