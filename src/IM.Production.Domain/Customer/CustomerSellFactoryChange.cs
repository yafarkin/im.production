using System;
using Epam.ImitationGames.Production.Domain.Base;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerSellFactoryChange : CustomerChange, IFinanceChanging
    {
        public decimal SumChange { get; protected set; }

        public CustomerSellFactoryChange(GameTime time, Factory soldFactory, decimal cost, Customer otherCustomer = null, string description = null)
            : base(time, soldFactory.Customer, description)
        {
            SoldFactory = soldFactory;
            OtherCustomer = otherCustomer;
            SumChange = cost;
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer.DelFactory(SoldFactory);
            Customer.AddSum(SumChange);
        }
    }
}