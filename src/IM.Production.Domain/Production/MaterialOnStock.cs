using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Материал на складе.
    /// </summary>
    [Serializable]
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