using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.CalculationEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IM.Production.Services
{
    public class TeamService : ITeamService
    {
        private readonly Game _game;
        private readonly Logic _logic;

        public TeamService(Game game, Logic logic)
        {
            _game = game;
            _logic = logic;
        }

        public void BuyFactory(string login, int indexOfDefinition)
        {
            var customer = _game.Customers.First(customer => customer.Login.Equals(login));
            var definitions = ReferenceData.GetAvailFactoryDefenitions(customer, false);
            var definition = definitions[indexOfDefinition];
            _logic.BuyFactoryFromGame(customer, definition);
        }

        public IList<FactoryDefinition> GetFactoriesDefinitions(string login)
        {
            var customer = _game.Customers.First(customer => customer.Login.Equals(login));
            var definitions = ReferenceData.GetAvailFactoryDefenitions(customer, false);
            return definitions;
        }

    }
}
