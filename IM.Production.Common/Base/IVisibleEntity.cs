namespace Epam.ImitationGames.Production.Common.Base
{
    /// <summary>
    /// Интерфейс, указывающий что сущность может отображаться в интерфейсе.
    /// </summary>
    public interface IVisibleEntity
    {
        /// <summary>
        /// Отображаемое в интерфейсе имя.
        /// </summary>
        string DisplayName { get; }
    }
}