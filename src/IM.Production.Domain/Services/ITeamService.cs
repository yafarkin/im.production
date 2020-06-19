using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface ITeamService
    {
        void BuyFactory(string login, int indexOfDefinition);
        IList<FactoryDefinition> GetFactoriesDefinitions(string login);
    }
}
