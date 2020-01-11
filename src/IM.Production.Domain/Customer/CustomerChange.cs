using System;
using Epam.ImitationGames.Production.Domain.Base;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain
{
    /// <summary>
    /// Изменения в команде.
    /// </summary>
    [Serializable]
    public abstract class CustomerChange : BaseChanging
    {
        /// <summary>
        /// Заключенный контракт.
        /// </summary>
        public Contract NewContract { get; set; }

        /// <summary>
        /// Закрытый контракт.
        /// </summary>
        public Contract ClosedContract { get; set; }

        /// <summary>
        /// Купленная фабрика.
        /// </summary>
        public Factory BoughtFactory { get; set; }

        /// <summary>
        /// Проданная фабрика.
        /// </summary>
        public Factory SoldFactory { get; set; }

        /// <summary>
        /// Команда, у которой фабрика куплена или продана (если фабрика покупается/продаётся игре - то не задаётся).
        /// </summary>
        public Customer OtherCustomer { get; set; }

        /// <summary>
        /// Изменение % исследования по RD.
        /// </summary>
        public decimal? NewRDProgress { get; set; }

        /// <summary>
        /// Изменение суммы, выделяемой на RD.
        /// </summary>
        public decimal? NewSumOnRD { get; protected set; }

        /// <summary>
        /// Изменение уровня поколения фабрик.
        /// </summary>
        public int? NewFactoryGenerationLevel { get; protected set; }

        protected CustomerChange(GameTime time, Customer customer, string description = null)
            : base(time, customer, description)
        {
        }

        public override void DoAction()
        {
            base.DoAction();
            if (NewSumOnRD.HasValue)
            {
                Customer.SumOnRD = NewSumOnRD.Value;
            }

            if (NewFactoryGenerationLevel.HasValue)
            {
                Customer.FactoryGenerationLevel = NewFactoryGenerationLevel.Value;
            }
        }
    }
}