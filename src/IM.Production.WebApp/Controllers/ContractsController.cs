using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IM.Production.WebApp.Dtos;
using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Services;

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
    }
}