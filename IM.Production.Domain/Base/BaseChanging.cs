using System;

namespace Epam.ImitationGames.Production.Domain.Base
{
    /// <summary>
    /// Сущность, описывающая изменения с чем либо во времени.
    /// </summary>
    public class BaseChanging : BaseEntity
    {
        /// <summary>
        /// Когда произошли изменения.
        /// </summary>
        public GameTime Time { get; set; }


        /// <summary>
        /// С какой командой связаны изменения.
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Текстовое описание для UI о сути изменений.
        /// </summary>
        public string Description { get; set; }

        public BaseChanging(GameTime time, Customer customer, string description = null)
        {
            Time = time;
            Customer = customer;
            Description = description;
        }
    }
}