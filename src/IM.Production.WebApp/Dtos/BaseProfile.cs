﻿using AutoMapper;
using System.Linq;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Production;

namespace IM.Production.WebApp.Dtos
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<Contract, ContractDto>()
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

            CreateMap<NewTeamDto, Customer>()
            .ForMember(source => source.DisplayName, opt => opt.MapFrom(dest => dest.Name));

            CreateMap<Customer, TeamDto>()
            .ForMember(source => source.Name,
                       opt => opt.MapFrom(src => src.Login))
            .ForMember(source => source.ProductionType,
                       opt => opt.MapFrom(src => src.ProductionType.DisplayName))
            .ForMember(source => source.Sum,
                       opt => opt.MapFrom(src => src.Sum))
            .ForMember(source => source.Factories,
                       opt => opt.MapFrom(src => string.Join(". ", src.Factories.Select(s => s.FactoryDefinition.DisplayName))))
            .ForMember(source => source.Contracts,
                       opt => opt.MapFrom(src => string.Join(". ", src.Contracts.Select(s => s.Description))));

            CreateMap<Factory, FactoryDto>()
            .ForMember(source => source.ProductionTypeKey,
                       opt => opt.MapFrom(dest => dest.FactoryDefinition.ProductionType.Key))
            .ForMember(source => source.Id,
                       opt => opt.MapFrom(dest => dest.Id));

            CreateMap<(Factory, Factory[]), FactoryAndContractFactories>()
            .ForMember(source => source.Factory,
                       opt => opt.MapFrom(dest => dest.Item1))
            .ForMember(source => source.ContractFactories,
                       opt => opt.MapFrom(dest => dest.Item2));

        }
    }
}
