using System;

namespace SantaHelpers.Exceptions
{
    /// <summary>
    /// Низкоуровневые исключения.
    /// </summary>
    class SantaHelpersException : Exception
    {
        public SantaHelpersException(Exception e) : base("Помощники санты не смогли...", e)
        {

        }

        public SantaHelpersException(string message, Exception e) : base(message, e)
        {

        }
    }
}
