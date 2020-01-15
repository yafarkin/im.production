using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerGenerationLevelChange : CustomerRDSpentChange
    {
        /// <summary>
        /// Изменение уровня поколения фабрик.
        /// </summary>
        public int NewFactoryGenerationLevel { get; protected set; }

        public CustomerGenerationLevelChange(GameTime time, Customer customer, int newGenerationLevel, decimal? spentSumToNextGenerationLevel = null, decimal? sumToNextGenerationLevel = null, string description = null)
            : base(time, customer, spentSumToNextGenerationLevel, sumToNextGenerationLevel, description)
        {
            NewFactoryGenerationLevel = newGenerationLevel;
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer.FactoryGenerationLevel = NewFactoryGenerationLevel;
        }
    }
}