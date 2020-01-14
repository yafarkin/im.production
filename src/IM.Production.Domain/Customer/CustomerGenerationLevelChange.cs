using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerGenerationLevelChange : CustomerRDSpentChange
    {
        public CustomerGenerationLevelChange(GameTime time, Customer customer, int newGenerationLevel, decimal spentSumToNextGenerationLevel, decimal sumToNextGenerationLevel, string description = null)
            : base(time, customer, spentSumToNextGenerationLevel, sumToNextGenerationLevel, description)
        {
            NewFactoryGenerationLevel = newGenerationLevel;
        }

        public override void DoAction()
        {
            NewRDProgress = Customer.RDProgress;
            base.DoAction();
        }
    }
}