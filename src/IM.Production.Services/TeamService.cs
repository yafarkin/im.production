using System.Linq;
using CalculationEngine;
using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.Services;

namespace IM.Production.Services
{
    public class TeamService : ITeamService
    {
        private readonly Game _game;

        public TeamService(Game game)
        {
            _game = game;
        }

        public IEnumerable<Factory> GetFactoriesByLogin(string login)
        {
            return _game?.Customers?.Where(obj => obj.Login.Equals(login)).FirstOrDefault()?.Factories;
        }

        (Factory, Factory[])[] ITeamService.GetContractFactoriesByLogin(string login)
        {
            var result = new List<(Factory, Factory[])>();
            var customer = _game?.Customers?.Where(obj => obj.Login.Equals(login)).FirstOrDefault();
            var factories = customer?.Factories;
            foreach (var factory in factories)
            {
                var thisFactoryContracts = customer.Contracts
                    .Where(obj => obj.SourceFactory.Id.Equals(factory.Id))
                    .ToList();
                var list = new List<Factory>();
                foreach (var item in thisFactoryContracts)
                {
                    var theSameObject = list
                        .Where(obj => obj.Id.Equals(item.DestinationFactory.Id))
                        .FirstOrDefault();
                    if (theSameObject == null)
                    {
                        list.Add(item.DestinationFactory);
                    }
                }
                result.Add((factory, list.ToArray()));
            }

            return result.ToArray();
        }

        (decimal, decimal, int) ITeamService.GetTeamGameProgress(string login)
        {
            var customer = _game.Customers.Where(obj => obj.Login.Equals(login)).FirstOrDefault();
            return (customer.Sum, customer.RDProgress, customer.FactoryGenerationLevel);
        }

    }
}
