using System;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class FinanceCustomerFineChange : FinanceCustomerChange
    {
        public FinanceCustomerFineChange(Customer customer, decimal sumChange, string description = null)
            : base(customer, sumChange, description)
        {
        }
    }
}