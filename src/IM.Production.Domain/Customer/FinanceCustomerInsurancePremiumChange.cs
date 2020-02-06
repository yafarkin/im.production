using System;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class FinanceCustomerInsurancePremiumChange : FinanceCustomerChange
    {
        public FinanceCustomerInsurancePremiumChange(Customer customer, decimal sumChange, string description = null)
            : base(customer, sumChange, description)
        {
        }
    }
}