using AutoMapper;
using Epam.ImitationGames.Production.Domain;

namespace IM.Production.WebApp.Dtos
{
    public class FactoryProfile : Profile
    {
        public FactoryProfile()
        {
            CreateMap<Contract, ContractDto>()
            .ForMember(source => source.Id,
                        opt => opt.MapFrom(dest => dest.Id))
            .ForMember(source => source.SourceCustomerLogin,
                        opt => opt.MapFrom(dest => dest.SourceFactory.Customer.Login))
            .ForMember(source => source.SourceFactoryName,
                        opt => opt.MapFrom(dest => dest.SourceFactory.FactoryDefinition.Name))
            .ForMember(source => source.SourceGenerationLevel,
                        opt => opt.MapFrom(dest => dest.SourceFactory.FactoryDefinition.GenerationLevel))
            .ForMember(source => source.SourceWorkers,
                        opt => opt.MapFrom(dest => dest.SourceFactory.Workers))

            .ForMember(source => source.DestinationCustomerLogin,
                        opt => opt.MapFrom(dest => dest.DestinationFactory.Customer.Login))
            .ForMember(source => source.DestinationFactoryName,
                        opt => opt.MapFrom(dest => dest.DestinationFactory.FactoryDefinition.Name))
            .ForMember(source => source.DestinationGenerationLevel,
                        opt => opt.MapFrom(dest => dest.DestinationFactory.FactoryDefinition.GenerationLevel))
            .ForMember(source => source.DestinationWorkers,
                        opt => opt.MapFrom(dest => dest.DestinationFactory.Workers));

        }
    }
}
