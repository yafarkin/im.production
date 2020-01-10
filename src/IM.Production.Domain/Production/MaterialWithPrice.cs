using System;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Материал со стоимостью за единицу.
    /// </summary>
    [Serializable]
    public class MaterialWithPrice : MaterialOnStock
    {
        /// <summary>
        /// Стоимость материала за единицу при продаже.
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// Общая стоимость материалов.
        /// </summary>
        public decimal TotalPrice => Amount * SellPrice;

        public override string ToString() => $"{base.ToString()}, 1 = {SellPrice:C}, T = {TotalPrice:C}";
    }
}