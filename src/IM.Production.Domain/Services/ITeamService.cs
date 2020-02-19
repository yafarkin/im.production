using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface ITeamService
    {
        IEnumerable<Factory> GetFactoriesByLogin(string login);
        IEnumerable<Factory[]> GetContractFactoriesByLogin(string login);
    }
}
