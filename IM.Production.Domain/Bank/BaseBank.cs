using Epam.ImitationGames.Production.Common.Base;

namespace Epam.ImitationGames.Production.Common.Bank
{
    /// <summary>
    /// Базовый класс для сущностей, связанных с банковскими операциями.
    /// </summary>
    public abstract class BaseBank : BaseChanging, IVisibleEntity
    {
        public abstract string DisplayName { get; }

        protected BaseBank(GameTime time, Customer customer, string description = null)
            : base(time, customer,  description)
        {
        }
    }
}