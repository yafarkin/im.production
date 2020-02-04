using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Base;
using Epam.ImitationGames.Production.Domain.Production;
using IM.Production.CalculationEngine;
using IM.Production.WebApp.Dtos;
using IM.Production.WebApp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly MapperConfiguration m_ContractConfig =
            new MapperConfiguration(cfg =>
            cfg.CreateMap<Contract, ContractDto>()
            .ForMember(source => source.SourceFactoryCustomerLogin,      opt => opt.MapFrom(dest => dest.SourceFactory.Customer.Login))
            .ForMember(source => source.DestinationFactoryCustomerLogin, opt => opt.MapFrom(dest => dest.DestinationFactory.Customer.Login)));
        private IMapper m_Mapper;

        [HttpGet]
        [Route("GetAllContracts")]
        public IEnumerable<ContractDto> GetAllContracts()
        {
            if (m_Mapper == null)
            {
                m_Mapper = m_ContractConfig.CreateMapper();
            }

            var gameInstance = FakeGameInitializer.CreateGame(15);
            var customerContracts = new List<Contract>();
            foreach (var customer in gameInstance.Customers)
            {
                customerContracts.AddRange(customer.Contracts);
            }

            var result = new List<ContractDto>();
            foreach (var element in customerContracts)
            {
                var dto = m_Mapper?.Map<ContractDto>(element);
               
                if (dto != null)
                {
                    if (dto.DestinationFactoryCustomerLogin == null || dto.SourceFactoryCustomerLogin == null)
                    {
                        continue;
                    }
                    result.Add(dto);
                }
            }

            return result;
        }

    }
}