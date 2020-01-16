using System;
using Epam.ImitationGames.Production.Domain.Base;

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
    }
}