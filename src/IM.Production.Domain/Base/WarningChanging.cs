using System;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain.Base
{
    [Serializable]
    public class WarningChanging : InfoChanging
    {
        public Factory Factory { get; protected set; }

        public WarningChanging(Customer customer, Factory factory, string description)
            : base(customer, description)
        {
            Factory = factory;
        }
    }
}