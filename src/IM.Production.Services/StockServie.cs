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

        public IEnumerable<MaterialWithPrice> GetMaterials(Guid id)
        {
            var materials = new List<MaterialWithPrice>();
            _game.Customers
                .Where(customer => customer.Factories.Any(factory => factory.Id.Equals(id)))
                .FirstOrDefault().Contracts.ToList()
                .ForEach(contract => materials.Add(contract.MaterialWithPrice));
            return materials;
        }
    }
}
