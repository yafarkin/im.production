using System;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryRDSpentChange : FactoryChange
    {
        public decimal? SpentSumToNextLevel { get; protected set; }
        public decimal? SumToNextLevel { get; protected set; }

        /// <summary>
        /// Изменение % исследования по RD.
        /// </summary>
        public decimal? NewRDProgress { get; set; }

        public FactoryRDSpentChange(Factory factory, decimal? spentSumToNextLevel,  decimal? sumToNextLevel, string description = null)
            : base(factory, description)
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

            NewRDProgress = Factory.RDProgress;

            if (string.IsNullOrWhiteSpace(Description))
            {
                Description = $"Изменение уровня исследования до {NewRDProgress:P}";
                if (SpentSumToNextLevel.HasValue)
                {
                    Description += $"; затрачено {SpentSumToNextLevel:C}";
                }

                if (SumToNextLevel.HasValue)
                {
                    Description += $"; нужно на след. уровень {SumToNextLevel:C}";
                }
            }
        }
    }
}