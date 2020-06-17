using System;
using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface IStockService
    {
        IEnumerable<MaterialOnStock> GetMaterials(string login, Guid factoryIdString);
    }
}
