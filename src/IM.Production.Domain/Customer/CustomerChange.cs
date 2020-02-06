using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    /// <summary>
    /// Изменения в команде.
    /// </summary>
    [Serializable]
    public abstract class CustomerChange : BaseChanging
    {
        protected CustomerChange(Customer customer, string description = null)
            : base(customer, description)
        {
        }
    }
}