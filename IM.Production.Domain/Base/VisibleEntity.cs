namespace Epam.ImitationGames.Production.Common.Base
{
    /// <summary>
    /// Сущность, которая может отображаться в пользовательском интерфейсе.
    /// </summary>
    public class VisibleEntity : BaseEntity, IVisibleEntity
    {
        /// <summary>
        /// Отображаемое в интерфейсе имя.
        /// </summary>
        public string DisplayName { get; set; }

        public override string ToString() => DisplayName;
    }
}