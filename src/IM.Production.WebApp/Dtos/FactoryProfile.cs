using AutoMapper;
using Epam.ImitationGames.Production.Domain;

namespace IM.Production.WebApp.Dtos
{
    public class FactoryProfile : Profile
    {
        public FactoryProfile()
        {
            CreateMap<Contract, ContractDto>()
            .ForMember(source => source.SourceFactoryCustomerLogin, 
                        opt => opt.MapFrom(dest => dest.SourceFactory.Customer.Login))
            .ForMember(source => source.DestinationFactoryCustomerLogin, 
                opt => opt.MapFrom(dest => dest.DestinationFactory.Customer.Login));
        }
    }
}
