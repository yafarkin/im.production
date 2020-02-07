using AutoMapper;
using System.Linq;
using Epam.ImitationGames.Production.Domain;

namespace IM.Production.WebApp.Dtos
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<Contract, ContractDto>()
            .ForMember(source => source.SourceFactoryCustomerLogin,
                        opt => opt.MapFrom(dest => dest.SourceFactory.Customer.Login))
            .ForMember(source => source.DestinationFactoryCustomerLogin,
                opt => opt.MapFrom(dest => dest.DestinationFactory.Customer.Login));
            CreateMap<Customer, TeamDto>()
             .ForMember(dest => dest.Name,
                opts => opts.MapFrom(src => src.Login))
             .ForMember(dest => dest.ProductionType,
                opts => opts.MapFrom(src => src.ProductionType.DisplayName))
             .ForMember(dest => dest.Sum,
                opts => opts.MapFrom(src => src.Sum))
             .ForMember(dest => dest.Factories,
                opt => opt.MapFrom(src => string.Join(". ", src.Factories
                                                                .Select(s => s.FactoryDefinition.DisplayName))))
           .ForMember(dest => dest.Contracts,
                opt => opt.MapFrom(src => string.Join(". ", src.Contracts
                                                                .Select(s => s.Description))));
        }
    }
}
