using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Налог на продажу конкретного материала.
    /// </summary>
    [Serializable]
    public class TaxOnMaterial : BaseEntity
    {
        /// <summary>
        /// Ссылка на материал.
        /// </summary>
        public Material Material { get; set; }

        /// <summary>
        /// % налога (0...1).
        /// </summary>
        public decimal Percent { get; set; }
    }
}