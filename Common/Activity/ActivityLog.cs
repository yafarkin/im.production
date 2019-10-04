using Epam.ImitationGames.Production.Common.Base;

namespace Epam.ImitationGames.Production.Common.Activity
{
    /// <summary>
    /// Лог активности действий.
    /// </summary>
    public class ActivityLog : BaseEntity
    {
        /// <summary>
        /// Описание изменений.
        /// </summary>
        public BaseChanging Change { get; set; }

        /// <summary>
        /// Хэш-код действия (вычисляется как хэш от предыдущей записи и хэш этой).
        /// </summary>
        public string HashCode { get; set; }
    }
}