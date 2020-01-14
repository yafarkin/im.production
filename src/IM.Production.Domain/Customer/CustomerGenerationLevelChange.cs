using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerGenerationLevelChange : CustomerChange
    {
        public decimal SpentSumToNextGenerationLevel { get; protected set; }
        public decimal SumToNextGenerationLevel { get; protected set; }

        public CustomerGenerationLevelChange(GameTime time, Customer customer, int newGenerationLevel, decimal spentSumToNextGenerationLevel, decimal sumToNextGenerationLevel, string description = null)
            : base(time, customer, description)
        {
            NewFactoryGenerationLevel = newGenerationLevel;
            SpentSumToNextGenerationLevel = spentSumToNextGenerationLevel;
            SumToNextGenerationLevel = sumToNextGenerationLevel;
        }

        public override void DoAction()
        {
            Customer.SetSumInfoForRD(SpentSumToNextGenerationLevel, SumToNextGenerationLevel);
            NewRDProgress = Customer.RDProgress;

            base.DoAction();
        }
    }
}