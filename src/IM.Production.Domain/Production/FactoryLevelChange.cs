using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryLevelChange : FactoryChange
    {
        public decimal? SpentSumToNextLevelUp { get; protected set; }
        public decimal? NeedSumToNextLevelUp { get; protected set; }

        public FactoryLevelChange(GameTime time, Factory factory, int newLevel, decimal? spentSumToNextLevelUp = null, decimal? needSumToNextLevelUp = null, string description = null)
            : base(time, factory, newLevel, null, null, description)
        {
            SpentSumToNextLevelUp = spentSumToNextLevelUp;
            NeedSumToNextLevelUp = needSumToNextLevelUp;
        }

        public override void DoAction()
        {
            base.DoAction();

            if (SpentSumToNextLevelUp.HasValue)
            {
                Factory.SpentSumToNextLevelUp = SpentSumToNextLevelUp.Value;
            }

            if (NeedSumToNextLevelUp.HasValue)
            {
                Factory.NeedSumToNextLevelUp = NeedSumToNextLevelUp.Value;
            }

            NewRDProgress = Factory.RDProgress;
        }
    }
}