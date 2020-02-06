using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryPerformanceChange : FactoryChange
    {
        public decimal Performance { get; protected set; }

        public FactoryPerformanceChange(Factory factory, decimal performance, string description = null)
            : base(factory, description)
        {
            Performance = performance;
        }

        public override void DoAction()
        {
            base.DoAction();
            Factory.Performance = Performance;
        }
    }
}