using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactorySumOnRDChange : FactoryChange
    {
        public FactorySumOnRDChange(GameTime time, Factory factory, decimal newSumOnRD, string description = null)
            : base(time, factory, null, null, newSumOnRD, description)
        {
        }
    }
}