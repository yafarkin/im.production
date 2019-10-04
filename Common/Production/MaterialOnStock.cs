using Epam.ImitationGames.Production.Common.Base;

namespace Epam.ImitationGames.Production.Common.Production
{
    /// <summary>
    /// Материал на складе.
    /// </summary>
    public class MaterialOnStock : BaseEntity
    {
        /// <summary>
        /// Количество материала.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Ссылка на материал.
        /// </summary>
        public Material Material { get; set; }

        public override string ToString() => $"{Material} x {Amount}";
    }
}