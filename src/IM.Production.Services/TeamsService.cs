using System.Linq;
using CalculationEngine;
using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.CalculationEngine;

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
            _logic.AddNewCustomer(team.Login, team.PasswordHash, team.DisplayName);
        }

        public TeamProgress GetTeamProgress(string login)
        {
            var customer = _game.Customers.FirstOrDefault(obj => obj.Login.Equals(login));
            return new TeamProgress()
            {
                MoneyBalance = customer.Sum,
                RDProgress = customer.RDProgress,
                FactoryGenerationLevel = customer.FactoryGenerationLevel
            };
        }
    }
}
