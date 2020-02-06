using System;

namespace Epam.ImitationGames.Production.Domain.Bank
{
    /// <summary>
    /// Операция оформления кредита.
    /// </summary>
    [Serializable]
    public class BankCredit : BankFinOperation
    {
        public override string DisplayName =>
            $"Оформление кредита для {Customer.DisplayName}, сумма {Sum}, ставка {Percent}, дней: {Days}";

        public BankCredit(Customer customer, decimal sum, string description = null)
            : base(customer, sum, description)
        {
        }
    }
}