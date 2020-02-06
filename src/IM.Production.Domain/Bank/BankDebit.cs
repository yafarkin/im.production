using System;

namespace Epam.ImitationGames.Production.Domain.Bank
{
    /// <summary>
    /// Операция оформления вклада.
    /// </summary>
    [Serializable]
    public class BankDebit : BankFinOperation
    {
        public override string DisplayName =>
            $"Оформление вклада для {Customer.DisplayName}, сумма {Sum}, ставка {Percent}";

        public BankDebit(Customer customer, decimal sum, string description = null)
            : base(customer, sum, description)
        {
        }
    }
}