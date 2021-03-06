﻿using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface IFactoriesService
    {
        IEnumerable<Factory> GetFactoriesByLogin(string login);
    }
}
