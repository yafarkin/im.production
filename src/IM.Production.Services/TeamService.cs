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

        public IEnumerable<Factory[]> GetContractFactoriesByLogin(string login)
        {
            //_game?.Customers?.Where(obj => obj.Login.Equals(login)).FirstOrDefault()?.Factories
            var result = new List<Factory[]>();
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
                    if (!list.Contains(item.DestinationFactory))
                    {
                        list.Add(item.DestinationFactory);
                    }
                }
                //thisFactoryContracts.ForEach(item => list.Add(item.DestinationFactory));
                result.Add(list.ToArray());
            }

            return result;
        }
    }
}
