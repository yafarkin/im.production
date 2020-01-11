using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryRDProgressChange : FactoryChange
    {
        public FactoryRDProgressChange(GameTime time, Factory factory, decimal newRDProgress, string description = null)
            : base(time, factory, null, null, null, description)
        {
            NewRDProgress = newRDProgress;
        }
    }
}