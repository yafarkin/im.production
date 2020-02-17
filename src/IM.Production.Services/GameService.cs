using System;
using System.Collections.Generic;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain.Services;
using Epam.ImitationGames.Production.Domain.Static;

namespace IM.Production.Services
{
    public class GameService : IGameService
    {
        private readonly Game _game;
        private readonly CalculationEngine.CalculationEngine _calculationEngine;

        public GameService(Game game, CalculationEngine.CalculationEngine calculationEngine)
        {
            _game = game;
            _calculationEngine = calculationEngine;
        }

        public int CalculateDay()
        {
            //start game
            _calculationEngine.Calculate();
            return CurrentGameProps.GameDay;
        }

        public void RestartGame()
        {
            _game.Reset();
            CurrentGameProps.GameDay = 0;
        }
    }
}
