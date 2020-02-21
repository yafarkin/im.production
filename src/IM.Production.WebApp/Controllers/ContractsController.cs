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
        public ContractDto GetContract(string id)
        {
            var guid = Guid.Parse(id);
            var customerContracts = _service.GetContracts();
            foreach (var contract in customerContracts)
            {
                if (contract.Id.Equals(guid))
                {
                    return _mapper?.Map<ContractDto>(contract);
                }
            }
            return null;
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

        [HttpGet]
        [Route("get-factory-contracts")]
        public FactoryContractDto[] GetFactoryContracts(string login)
        {
            var contracts = _service.GetFactoryContracts(login).ToList();
            var contractsDtos = new FactoryContractDto[contracts.Count];
            for (var i = 0; i < contracts.Count; i++)
            {
                var contractDto = _mapper.Map<FactoryContractDto>(contracts[i]);
                contractsDtos[i] = contractDto;
            }
            return contractsDtos;
        }

    }
}