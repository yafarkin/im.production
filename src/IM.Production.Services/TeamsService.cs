using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Services;
using System.Collections.Generic;

namespace IM.Production.Services
{
    public class TeamsService : ITeamsService
    {
        private readonly Game _game;

        public TeamsService(Game game)
        {
            _game = game;
        }

        public IEnumerable<Customer> GetTeams()
        {
            return _game.Customers;
        }
    }
}
