using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Запись о произведении материала на фабрике.
    /// </summary>
    public class FactoryProduction : BaseEntity
    {
        /// <summary>
        /// Когда.
        /// </summary>
        public GameTime Time { get; set; }

        /// <summary>
        /// Ссылка на фабрику.
        /// </summary>
        public Factory Factory { get; set; }

        /// <summary>
        /// Произведенный материал.
        /// </summary>
        public Material Material { get; set; }

        /// <summary>
        /// Количество произведенного материала.
        /// </summary>
        public decimal Amount { get; set; }
    }
}