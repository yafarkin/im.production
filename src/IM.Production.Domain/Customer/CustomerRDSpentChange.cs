using System;
using Epam.ImitationGames.Production.Domain.Base;

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

        public CustomerRDSpentChange(GameTime time, Customer customer, decimal? spentSumToNextGenerationLevel, decimal? sumToNextGenerationLevel, string description = null)
            : base(time, customer, description)
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