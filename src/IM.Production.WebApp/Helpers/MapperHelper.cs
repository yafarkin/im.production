using AutoMapper;
using Epam.ImitationGames.Production.Domain;
using IM.Production.WebApp.Dtos;

namespace IM.Production.WebApp.Helpers
{
    public class MapperHelper
    {
        private static MapperConfiguration m_Config;
        private static IMapper m_Mapper;
        static MapperHelper()
        {
            m_Config = new MapperConfiguration(cfg => cfg.CreateMap<Contract, ContractDto>());
            m_Mapper = m_Config.CreateMapper();
        }

    }
}
