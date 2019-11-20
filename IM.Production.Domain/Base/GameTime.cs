using Epam.ImitationGames.Production.Common.Static;
using System;

namespace Epam.ImitationGames.Production.Common.Base
{
    /// <summary>
    /// Игоровое время.
    /// </summary>
    public class GameTime : BaseEntity
    {
        public GameTime()
        {
            Day = CurrentGameProps.GameDay;
            When = DateTime.UtcNow;
        }

        /// <summary>
        /// Игровой день (№ п/п).
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Когда это было в реальном времени.
        /// </summary>
        public DateTime When { get; set; }
    }
}