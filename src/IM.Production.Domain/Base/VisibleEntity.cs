using System;

namespace Epam.ImitationGames.Production.Domain.Base
{
    /// <summary>
    /// Сущность, которая может отображаться в пользовательском интерфейсе.
    /// </summary>
    [Serializable]
    public class VisibleEntity : BaseEntity, IVisibleEntity
    {
        /// <summary>
        /// Отображаемое в интерфейсе имя.
        /// </summary>
        public string DisplayName { get; set; }

        public override string ToString() => DisplayName;
    }
}