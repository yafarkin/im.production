using System;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerRDSpentChange : CustomerChange
    {
        public decimal? SpentSumToNextGenerationLevel { get; protected set; }
        public decimal? SumToNextGenerationLevel { get; protected set; }

        /// <summary>
        /// Изменение % исследования по RD.
        /// </summary>
        public decimal? NewRDProgress { get; set; }

        public CustomerRDSpentChange(Customer customer, decimal? spentSumToNextGenerationLevel, decimal? sumToNextGenerationLevel, string description = null)
            : base(customer, description)
        {
            SpentSumToNextGenerationLevel = spentSumToNextGenerationLevel;
            SumToNextGenerationLevel = sumToNextGenerationLevel;
        }

        public override void DoAction()
        {
            base.DoAction();
            if (SpentSumToNextGenerationLevel.HasValue)
            {
                Customer.SpentSumToNextGenerationLevel = SpentSumToNextGenerationLevel.Value;
            }

            if (SumToNextGenerationLevel.HasValue)
            {
                Customer.SumToNextGenerationLevel = SumToNextGenerationLevel.Value;
            }

            NewRDProgress = Customer.RDProgress;
        }
    }
}