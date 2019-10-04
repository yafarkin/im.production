using Epam.ImitationGames.Production.Common.Base;

namespace Epam.ImitationGames.Production.Common.Production
{
    /// <summary>
    /// Налог на фабрику или продажу материала.
    /// </summary>
    public class Tax : BaseChanging
    {
        /// <summary>
        /// Фабрика, с которой списывается налог.
        /// </summary>
        public Factory Factory { get; set; }

        /// <summary>
        /// Сумма налога.
        /// </summary>
        public decimal Sum { get; set; }

        public Tax(GameTime time, Factory factory, string description = null)
            : base(time, factory.Customer, description)
        {
            Factory = factory;
        }
    }
}