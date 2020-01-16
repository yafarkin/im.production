using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryLevelChange : FactoryRDSpentChange
    {
        /// <summary>
        /// Изменение уровня.
        /// </summary>
        public int NewLevel { get; protected set; }

        public FactoryLevelChange(Factory factory, int newLevel, decimal? spentSumToNextLevel = null, decimal? sumToNextLevel = null, string description = null)
            : base(factory, spentSumToNextLevel, sumToNextLevel, description)
        {
            NewLevel = newLevel;
        }

        public override void DoAction()
        {
            base.DoAction();
            Factory.Level = NewLevel;
        }
    }
}