using System;
using System.Collections.Generic;
using System.Text;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface IGameService
    {
        int GetCurrentDay();
        void CalculateDay();
        void RestartGame();
    }
}
