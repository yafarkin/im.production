﻿using AutoMapper;
using System.Linq;
using Epam.ImitationGames.Production.Domain;
using System.Security.Cryptography;
using System.Text;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;

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

            CreateMap<FactoryDefinition, FactoryDefinitionDto>()
                .ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.GenerationLevel))
                .ForMember(dest => dest.WorkersCount, opts => opts.MapFrom(src => src.BaseWorkers))
                .ForMember(dest => dest.Cost, opts => opts.MapFrom(src => ReferenceData.DefaultFactoryCost[src.GenerationLevel]))
                .ForMember(dest => dest.ProductionType, opts => opts.MapFrom(src => ReferenceData.ProductionTypeKeyToEnum[src.ProductionType.Key]));
        }
    }
}
