using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Bank
{
    /// <summary>
    /// Операция оформления кредита.
    /// </summary>
    public class BankCredit : BankFinOperation
    {
        public override string DisplayName =>
            $"Оформление кредита для {Customer.DisplayName}, сумма {Sum}, ставка {Percent}, дней: {Days}";

        public BankCredit(GameTime time, Customer customer, string description = null)
            : base(time, customer, description)
        {
        }
    }
}