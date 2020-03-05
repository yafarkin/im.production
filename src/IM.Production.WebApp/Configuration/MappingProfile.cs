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

            //TODO Mapping to Domain Entities should be done via Entities' contructors
            CreateMap<NewTeamDto, Customer>()
                .ForMember(source => source.DisplayName, opt => opt.MapFrom(dest => dest.Name))
                .ForMember(source => source.ProductionType, opt => opt.Ignore())
                .ForMember(source => source.SumOnRD, opt => opt.Ignore())
                .ForMember(source => source.SumToNextGenerationLevel, opt => opt.Ignore())
                .ForMember(source => source.SpentSumToNextGenerationLevel, opt => opt.Ignore())
                .ForMember(source => source.Sum, opt => opt.Ignore())
                .ForMember(source => source.FactoryGenerationLevel, opt => opt.Ignore())
                .ForMember(source => source.Id, opt => opt.Ignore());

            CreateMap<Customer, TeamDto>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Login))
                .ForMember(dest => dest.ProductionType, opts => opts.MapFrom(src => src.ProductionType.DisplayName))
                .ForMember(dest => dest.Sum, opts => opts.MapFrom(src => src.Sum))
                .ForMember(dest => dest.Factories, opt => opt.MapFrom(src => string.Join(". ", src.Factories.Select(s => s.FactoryDefinition.DisplayName))));

            CreateMap<User, UserDto>();

            CreateMap<Factory, FactoryDto>()
                .ForMember(source => source.ProductionTypeKey, opt => opt.MapFrom(dest => dest.FactoryDefinition.ProductionType.Key))
                .ForMember(source => source.Name, opt => opt.MapFrom(dest => dest.DisplayName))
                .ForMember(source => source.Id, opt => opt.MapFrom(dest => dest.Id));

            CreateMap<TeamProgress, TeamProgressDto>();
        }
    }
}
