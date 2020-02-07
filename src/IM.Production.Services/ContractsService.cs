using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Services;
using System.Collections.Generic;

namespace IM.Production.Services
{
    public class ContractsService : IContractsService
    {
        private readonly Game _game;

        public ContractsService(Game game)
        {
            _game = game;
        }
        public IEnumerable<Contract> GetContracts()
        {
            var customerContracts = new List<Contract>();
            foreach (var customer in _game.Customers)
            {
                customerContracts.AddRange(customer.Contracts);
            }
            return customerContracts;
        }
    }
}
