using System;

namespace Epam.ImitationGames.Production.Domain.Base
{
    /// <summary>
    /// Сущность, описывающая изменения с чем либо во времени.
    /// </summary>
    [Serializable]
    public class BaseChanging : BaseEntity
    {
        /// <summary>
        /// Когда произошли изменения.
        /// </summary>
        public GameTime Time { get; protected set; }

        /// <summary>
        /// С какой командой связаны изменения.
        /// </summary>
        public Customer Customer { get; protected set; }

        /// <summary>
        /// Текстовое описание для UI о сути изменений.
        /// </summary>
        public string Description { get; protected set; }

        public BaseChanging(Customer customer, string description = null)
        {
            Time = GameTime.GetGameTime;
            Customer = customer;
            Description = description;
        }

        /// <summary>
        /// Выполняет необходимые действия (если предусмотрено) в момент добавления в лог.
        /// </summary>
        public virtual void DoAction()
        {
        }

        public override string ToString() => Description;
    }
}