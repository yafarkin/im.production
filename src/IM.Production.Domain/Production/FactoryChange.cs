using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Описание изменения, произошедшего с фабрикой
    /// </summary>
    [Serializable]
    public abstract class FactoryChange : BaseChanging
    {
        /// <summary>
        /// Ссылка на фабрику.
        /// </summary>
        public Factory Factory { get; protected set; }

        protected FactoryChange(Factory factory, string description = null)
            : base(factory.Customer, description)
        {
            Factory = factory;
        }
    }
}