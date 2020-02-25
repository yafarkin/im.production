using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Contract> GetOneTimeContracts(string login)
        {
            var customer = _game.Customers.Where(obj => obj.Login.Equals(login)).FirstOrDefault();
            var factories = customer?.Factories;
            var factoryId = factories.FirstOrDefault()?.Id;
            var contracts = customer.Contracts.Where(
                obj => obj.TillCount == null && 
                      (obj.SourceFactory.Id.Equals(factoryId) ||
                       obj.DestinationFactory.Id.Equals(factoryId)));
            return contracts;
        }

        public IEnumerable<Contract> GetMultiTimeContracts(string login)
        {
            var customer = _game.Customers.Where(obj => obj.Login.Equals(login)).FirstOrDefault();
            var factories = customer?.Factories;
            var factoryId = factories.FirstOrDefault()?.Id;
            var contracts = customer.Contracts.Where(
                obj => obj.TillCount != null && 
                      (obj.SourceFactory.Id.Equals(factoryId) ||
                       obj.DestinationFactory.Id.Equals(factoryId)));
            return contracts;
        }
    }
}
