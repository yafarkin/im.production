using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class FinanceCustomerChange : FinanceChange
    {
        public FinanceCustomerChange(GameTime time, Customer customer, decimal sumChange, string description = null)
            :  base(time, customer, sumChange, description)
        {
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer.Sum += SumChange;
        }
    }
}