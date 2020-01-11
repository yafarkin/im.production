using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryWorkerCountChange : FactoryChange
    {
        public FactoryWorkerCountChange(GameTime time, Factory factory, int newWorkersCount, string description = null)
            : base(time, factory, null, newWorkersCount, null, description)
        {
        }
    }
}