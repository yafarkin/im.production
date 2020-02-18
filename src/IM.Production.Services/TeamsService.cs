using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Exceptions;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.CalculationEngine;
using System.Collections.Generic;

namespace IM.Production.Services
{
    public class TeamsService : ITeamsService
    {
        private readonly Game _game;
        private readonly Logic _logic;

        public TeamsService(Game game, Logic logic)
        {
            _game = game;
            _logic = logic;
        }

        public IEnumerable<Customer> GetTeams()
        {
            return _game.Customers;
        }

        public void AddTeam(Customer team)
        {
            _logic.AddCustomer(team.Login, team.PasswordHash, team.DisplayName);
        }
    }
}
