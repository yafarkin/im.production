using System;

namespace Epam.ImitationGames.Production.Domain.Base
{
    /// <summary>
    /// Базовая сущность для всего в игре.
    /// </summary>
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Идентификатор сущности.
        /// </summary>
        public Guid Id { get; set; }
    }
}
