using System;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Налог на фабрику или продажу материала.
    /// </summary>
    [Serializable]
    public class TaxFactoryChange : FinanceFactoryChange
    {
        public TaxFactoryChange(Factory factory, decimal onTax, string description = null)
            : base(factory, 0, onTax, 0, description)
        {
        }

        public override void DoAction()
        {
            base.DoAction();

            if (string.IsNullOrWhiteSpace(Description))
            {
                Description = $"Оплата налогов на фабрику {Factory} в сумме {OnTax:C}";
            }
        }
    }
}