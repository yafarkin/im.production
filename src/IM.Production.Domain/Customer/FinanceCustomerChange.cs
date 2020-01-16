using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class FinanceCustomerChange : FinanceChange
    {
        public FinanceCustomerChange(Customer customer, decimal sumChange, string description = null)
            :  base(customer, sumChange, description)
        {
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer.Sum += SumChange;
        }
    }
}