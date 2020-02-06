using System;
using Epam.ImitationGames.Production.Domain.Base;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerBuyFactoryChange : CustomerChange, IFinanceChanging
    {
        /// <summary>
        /// Купленная фабрика.
        /// </summary>
        public Factory BoughtFactory { get; protected set; }

        /// <summary>
        /// Команда, у которой фабрика была куплена (если фабрика покупается/продаётся игре - то не задаётся).
        /// </summary>
        public Customer OtherCustomer { get; protected set; }

        /// <summary>
        /// Сумма, за которую фабрика была куплена.
        /// </summary>
        public decimal SumChange { get; protected set; }

        public CustomerBuyFactoryChange(Customer customer, Factory boughtFactory, decimal cost, Customer otherCustomer = null, string description = null)
            : base(customer, description)
        {
            BoughtFactory = boughtFactory;
            OtherCustomer = otherCustomer;
            SumChange = -cost;
        }

        public override void DoAction()
        {
            base.DoAction();
            BoughtFactory.Customer = Customer;
            Customer._factories.Add(BoughtFactory);
            Customer.Sum += SumChange;
        }
    }
}