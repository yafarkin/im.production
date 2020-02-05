using System.Collections.Generic;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface IContractsService
    {
        IEnumerable<Contract> GetContracts();
    }
}