using System.Linq;
using AutoMapper;
using IM.Production.WebApp.Dtos;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Production;

namespace IM.Production.WebApp.Dtos
{
    public class OrganizationProfile: Profile
    {
        public OrganizationProfile()
        {
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
