using System;
using System.Linq;
using System.Collections.Generic;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.Services;

namespace IM.Production.Services
{
    public class StockServie : IStockService
    {
        private readonly Game _game;

        public StockServie(Game game)
        {
            _game = game;
        }

        public IEnumerable<MaterialOnStock> GetMaterials(string login, Guid factoryId)
        {
            var factories = _game.Customers.First(obj => obj.Login.Equals(login)).Factories;
            var result = factories.FirstOrDefault(factory => factory.Id.Equals(factoryId)).Stock;
            return result;
        }
    }
}
