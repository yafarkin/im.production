using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Bank
{
    /// <summary>
    /// Базовый класс для сущностей, связанных с банковскими операциями.
    /// </summary>
    [Serializable]
    public abstract class BaseBank : BaseChanging, IVisibleEntity
    {
        public abstract string DisplayName { get; }

        protected BaseBank(Customer customer, string description = null)
            : base(customer, description)
        {
        }
    }
}