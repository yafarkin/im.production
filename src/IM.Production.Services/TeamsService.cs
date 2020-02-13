using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
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
            var alreadyExists = false;
            /*
                ProductionType:
                * Metallurgical industry
                * Oil and gas and chemical industry
                * Electronic industry
             */
            var metallurgicalCount = 0;
            var oilAndGasAndChemicalCount = 0;
            var electronicCount = 0;
            foreach (var customer in _game.Customers)
            {
                if (customer.DisplayName.ToLower().Equals(team.DisplayName.ToLower()) ||
                    customer.Login.ToLower().Equals(team.Login.ToLower()))
                {
                    alreadyExists = true;
                    break;
                }

                if (customer.ProductionType.DisplayName.Equals("Metallurgical"))
                {
                    ++metallurgicalCount;
                } 
                else if (customer.ProductionType.DisplayName.Equals("Oil and gas and chemical"))
                {
                    ++oilAndGasAndChemicalCount;
                }
                else if (customer.ProductionType.DisplayName.Equals("Electronic"))
                {
                    ++electronicCount;
                }
            }

            var displayName = string.Empty;
            if (metallurgicalCount < oilAndGasAndChemicalCount && metallurgicalCount < electronicCount)
            {
                displayName = "Metallurgical";
            }
            else if (oilAndGasAndChemicalCount < metallurgicalCount && metallurgicalCount < electronicCount)
            {
                displayName = "Oil and gas and chemical";
            }
            else
            {
                displayName = "Electronic";
            }

            if (!alreadyExists)
            {
                var productionType = new ProductionType();
                productionType.DisplayName = displayName;
                productionType.Key = "Key"; 
                _logic.AddCustomer(team.Login, team.PasswordHash, team.DisplayName, productionType);
            }
        }
    }
}
