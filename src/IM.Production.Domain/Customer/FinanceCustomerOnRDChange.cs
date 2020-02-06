using System;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class FinanceCustomerOnRDChange : FinanceCustomerChange
    {
        public FinanceCustomerOnRDChange(Customer customer, decimal sumChange, string description = null)
            : base(customer, sumChange, description)
        {
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer.SpentSumToNextGenerationLevel += -SumChange;
        }
    }
}