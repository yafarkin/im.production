using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryLevelChange : FactoryChange
    {
        public decimal SpentSumToNextLevelUp { get; protected set; }
        public decimal NeedSumToNextLevelUp { get; protected set; }

        public FactoryLevelChange(GameTime time, Factory factory, int newLevel, decimal spentSumToNextLevelUp, decimal needSumToNextLevelUp, string description = null)
            : base(time, factory, newLevel, null, null, description)
        {
            SpentSumToNextLevelUp = spentSumToNextLevelUp;
            NeedSumToNextLevelUp = needSumToNextLevelUp;
        }

        public override void DoAction()
        {
            Factory.SpentSumToNextLevelUp = SpentSumToNextLevelUp;
            Factory.NeedSumToNextLevelUp = NeedSumToNextLevelUp;
            NewRDProgress = Factory.RDProgress;

            base.DoAction();
        }
    }
}