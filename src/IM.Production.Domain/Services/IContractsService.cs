using System;
using System.Collections.Generic;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface IContractsService
    {
        IEnumerable<Contract> GetContracts();
        IEnumerable<Contract> GetOneTimeContracts(string login);
        IEnumerable<Contract> GetMultiTimeContracts(string login);
    }
}