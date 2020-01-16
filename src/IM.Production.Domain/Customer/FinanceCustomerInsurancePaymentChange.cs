using System;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class FinanceCustomerInsurancePaymentChange : FinanceCustomerChange
    {
        public FinanceCustomerInsurancePaymentChange(Customer customer, decimal sumChange, string description = null)
            : base(customer, sumChange, description)
        {
        }
    }
}