﻿using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryRDSpentChange : FactoryChange
    {
        public decimal? SpentSumToNextLevel { get; protected set; }
        public decimal? SumToNextLevel { get; protected set; }

        public FactoryRDSpentChange(GameTime time, Factory factory, decimal? spentSumToNextLevel,  decimal? sumToNextLevel, string description = null)
            : base(time, factory, null, null, null,  description)
        {
            SpentSumToNextLevel = spentSumToNextLevel;
            SumToNextLevel = sumToNextLevel;
        }

        public override void DoAction()
        {
            base.DoAction();
            if (SpentSumToNextLevel.HasValue)
            {
                Factory.SpentSumToNextLevelUp = SpentSumToNextLevel.Value;
            }

            if (SumToNextLevel.HasValue)
            {
                Factory.NeedSumToNextLevelUp = SumToNextLevel.Value;
            }
        }
    }
}