using AutoMapper;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Production;
using IM.Production.Services;
using IM.Production.WebApp.Dtos;
using System.Linq;

namespace IM.Production.WebApp.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contract, ContractDto>()
                .ForMember(source => source.SourceCustomerLogin, opt => opt.MapFrom(dest => dest.SourceFactory.Customer.Login))
                .ForMember(source => source.SourceFactoryName, opt => opt.MapFrom(dest => dest.SourceFactory.FactoryDefinition.Name))
                .ForMember(source => source.SourceGenerationLevel, opt => opt.MapFrom(dest => dest.SourceFactory.FactoryDefinition.GenerationLevel))
                .ForMember(source => source.SourceWorkers, opt => opt.MapFrom(dest => dest.SourceFactory.Workers))
                .ForMember(source => source.DestinationCustomerLogin, opt => opt.MapFrom(dest => dest.DestinationFactory.Customer.Login))
                .ForMember(source => source.DestinationFactoryName, opt => opt.MapFrom(dest => dest.DestinationFactory.FactoryDefinition.Name))
                .ForMember(source => source.DestinationGenerationLevel, opt => opt.MapFrom(dest => dest.DestinationFactory.FactoryDefinition.GenerationLevel))
                .ForMember(source => source.DestinationWorkers, opt => opt.MapFrom(dest => dest.DestinationFactory.Workers));

            CreateMap<NewTeamDto, Customer>()
                .ForMember(source => source.DisplayName, opt => opt.MapFrom(dest => dest.Name));

            CreateMap<Customer, TeamDto>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Login))
                .ForMember(dest => dest.ProductionType, opts => opts.MapFrom(src => src.ProductionType.DisplayName))
                .ForMember(dest => dest.Sum, opts => opts.MapFrom(src => src.Sum))
                .ForMember(dest => dest.Factories, opt => opt.MapFrom(src => string.Join(". ", src.Factories.Select(s => s.FactoryDefinition.DisplayName))));

            CreateMap<User, UserDto>();

            CreateMap<MaterialWithPrice, StockMaterialDto>()
                .ForMember(dest => dest.Key, opts => opts.MapFrom(src => src.Material.Key))
                .ForMember(dest => dest.ProductionType, opts => opts.MapFrom(src => src.Material.ProductionType.Key))
                .ForMember(dest => dest.Amount, opts => opts.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Amount, opts => opts.MapFrom(src => src.Material.AmountPerDay));
        }
    }
}
