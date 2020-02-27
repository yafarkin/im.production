using System;
using System.Collections.Generic;
using System.Text;

namespace Epam.ImitationGames.Production.Domain.Exceptions
{
    public class GameFinishedException : Exception
    {
        public GameFinishedException(string message)
            : base(message)
        {
        }
    }
}
