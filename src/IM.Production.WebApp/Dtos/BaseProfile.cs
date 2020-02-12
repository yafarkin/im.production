using AutoMapper;
using System.Linq;
using Epam.ImitationGames.Production.Domain;
using System.Security.Cryptography;
using System.Text;

namespace IM.Production.WebApp.Dtos
{
    public class BaseProfile : Profile
    {
        public static string GetMD5Hash(string str)
        {
            return GetMD5Hash(Encoding.ASCII.GetBytes(str));
        }

        public static string GetMD5Hash(byte[] array)
        {
            using (var md5 = MD5.Create())
            {
                var hashCode = GetHashString(md5.ComputeHash(array));
                return hashCode;
            }
        }

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

            string GetMD5Hash(string input)
            {
                using (var md5 = MD5.Create())
                {
                    var array = Encoding.ASCII.GetBytes(input);
                    var sb = new StringBuilder();
                    var binArray = md5.ComputeHash(array);
                    for (var i = 0; i < binArray.Length; i++)
                    {
                        sb.Append(binArray[i].ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
                
            CreateMap<NewTeamDto, Customer>()
                .ForMember(source => source.Login, opt => opt.MapFrom(dest => dest.Login))
                .ForMember(source => source.Name, opt => opt.MapFrom(dest => dest.Name))
                .ForMember(source => source.PasswordHash, opt => opt.MapFrom(dest => dest));

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
