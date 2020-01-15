using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactorySumOnRDChange : FactoryChange
    {
        /// <summary>
        /// Изменение суммы, выделяемой на RD.
        /// </summary>
        public decimal NewSumOnRD { get; protected set; }

        public FactorySumOnRDChange(GameTime time, Factory factory, decimal newSumOnRD, string description = null)
            : base(time, factory, description)
        {
            NewSumOnRD = newSumOnRD;
        }

        public override void DoAction()
        {
            base.DoAction();
            Factory.SumOnRD = NewSumOnRD;
        }
    }
}