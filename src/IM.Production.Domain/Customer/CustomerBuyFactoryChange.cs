using System;
using Epam.ImitationGames.Production.Domain.Base;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerBuyFactoryChange : CustomerChange, IFinanceChanging
    {
        public decimal SumChange { get; protected set; }

        public CustomerBuyFactoryChange(GameTime time, Customer customer, Factory boughtFactory, decimal cost, Customer otherCustomer = null, string description = null)
            : base(time, customer, description)
        {
            BoughtFactory = boughtFactory;
            OtherCustomer = otherCustomer;
            SumChange = -cost;
        }

        public override void DoAction()
        {
            base.DoAction();
            BoughtFactory.Customer = Customer;
            Customer.AddFactory(BoughtFactory);
            Customer.AddSum(-SumChange);
        }
    }
}