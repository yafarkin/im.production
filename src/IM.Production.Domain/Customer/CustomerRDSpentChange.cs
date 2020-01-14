using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerRDSpentChange : CustomerChange
    {
        public decimal? SpentSumToNextGenerationLevel { get; protected set; }
        public decimal? SumToNextGenerationLevel { get; protected set; }

        public CustomerRDSpentChange(GameTime time, Customer customer, decimal? spentSumToNextGenerationLevel, decimal? sumToNextGenerationLevel, string description = null)
            : base(time, customer, description)
        {
            SpentSumToNextGenerationLevel = spentSumToNextGenerationLevel;
            SumToNextGenerationLevel = sumToNextGenerationLevel;
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer.SetSumInfoForRD(SpentSumToNextGenerationLevel ?? Customer.SpentSumToNextGenerationLevel, SumToNextGenerationLevel ?? Customer.SumToNextGenerationLevel);
        }
    }
}