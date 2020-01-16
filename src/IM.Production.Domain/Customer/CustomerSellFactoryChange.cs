using System;
using Epam.ImitationGames.Production.Domain.Base;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerSellFactoryChange : CustomerChange, IFinanceChanging
    {
        /// <summary>
        /// Проданная фабрика.
        /// </summary>
        public Factory SoldFactory { get; set; }

        /// <summary>
        /// Команда, которой фабрика была продана (если фабрика покупается/продаётся игре - то не задаётся).
        /// </summary>
        public Customer OtherCustomer { get; protected set; }

        /// <summary>
        /// Сумма, за которую фабрика была продана.
        /// </summary>
        public decimal SumChange { get; protected set; }

        public CustomerSellFactoryChange(Factory soldFactory, decimal cost, Customer otherCustomer = null, string description = null)
            : base(soldFactory.Customer, description)
        {
            SoldFactory = soldFactory;
            OtherCustomer = otherCustomer;
            SumChange = cost;
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer._factories.Remove(SoldFactory);
            Customer.Sum += SumChange;
        }
    }
}