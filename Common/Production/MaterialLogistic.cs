using Epam.ImitationGames.Production.Common.Base;

namespace Epam.ImitationGames.Production.Common.Production
{
    /// <summary>
    /// Операция перемещения материала с одной фабрики на другую.
    /// </summary>
    public class MaterialLogistic : BaseChanging
    {
        /// <summary>
        /// Исходная фабрика, если не задано - поставляется игрой.
        /// </summary>
        public Factory SourceFactory { get; set; }

        /// <summary>
        /// Фабрика назначения, если не задано - продается игре.
        /// </summary>
        public Factory DestinationFactory { get; set; }

        /// <summary>
        /// Конкретный материал с указанием цены (за единицу) и количества.
        /// </summary>
        public MaterialWithPrice MaterialWithPrice { get; set; }

        /// <summary>
        /// Ссылка на списание налога за поставку материала.
        /// </summary>
        public Tax Tax { get; set; }

        public MaterialLogistic(GameTime time, MaterialWithPrice materialWithPrice, string description = null)
            : base(time, null, description)
        {
            MaterialWithPrice = materialWithPrice;
        }
    }
}