﻿namespace Epam.ImitationGames.Production.Domain.Static
{
    /// <summary>
    /// Контейнер для хранения актуальной информации о текущей игре.
    /// </summary>
    public static class CurrentGameProps
    {
        /// <summary>
        /// Текущий игровой день.
        /// </summary>
        public static int GameDay { get; private set; }

        static CurrentGameProps()
        {
            GameDay = 1;
        }

        public static void IncGameDay()
        {
            GameDay++;
        }
    }
}
