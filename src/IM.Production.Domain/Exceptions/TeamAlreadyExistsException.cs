using System;

namespace Epam.ImitationGames.Production.Domain.Exceptions
{
    public class TeamAlreadyExistsException : Exception
    {
        public TeamAlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}
