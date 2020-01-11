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
        public TaxFactoryChange(GameTime time, Factory factory, decimal onTax, string description = null)
            : base(time, factory, 0, onTax, 0, description)
        {
        }
    }
}