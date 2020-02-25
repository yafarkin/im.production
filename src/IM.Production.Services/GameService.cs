using CalculationEngine;
using Epam.ImitationGames.Production.Domain.Exceptions;
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

        public void CalculateDay()
        {
            if (CurrentGameProps.GameDay >= _game.TotalGameDays)
            {
                throw new GameFinishedException("Игра окончена!");
            }
            _calculationEngine.Calculate();
        }
        
        public int GetCurrentDay()
        {
            return CurrentGameProps.GameDay;
        }

        public void RestartGame()
        {
            _game.Reset();
        }

    }
}
