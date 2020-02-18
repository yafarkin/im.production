using System;
using System.Collections.Generic;
using System.Text;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface IGameService
    {
        void SetGameMaxDays(int maxDays);
        int CalculateDay();
        void RestartGame();
    }
}
