using Epam.ImitationGames.Production.Common.Base;

namespace Epam.ImitationGames.Production.Common.Production
{
    /// <summary>
    /// Налог на продажу конкретного материала.
    /// </summary>
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