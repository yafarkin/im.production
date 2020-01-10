using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Описание изменения, произошедшего с фабрикой
    /// </summary>
    [Serializable]
    public class FactoryChange : BaseChanging
    {
        /// <summary>
        /// Ссылка на фабрику.
        /// </summary>
        public Factory Factory { get; set; }

        /// <summary>
        /// Изменение уровня.
        /// </summary>
        public int LevelChange { get; set; }

        /// <summary>
        /// Изменение количества рабочих.
        /// </summary>
        public int WorkersChange { get; set; }

        /// <summary>
        /// Изменение % исследования по RD.
        /// </summary>
        public decimal RDProgressChange { get; set; }

        /// <summary>
        /// Изменение суммы, выделяемой на RD.
        /// </summary>
        public decimal SumOnRDChange { get; set; }

        public FactoryChange(GameTime time, Factory factory, string description = null)
            : base(time, factory.Customer, description)
        {
            Factory = factory;
        }
    }
}