using Epam.ImitationGames.Production.Common.Base;

namespace Epam.ImitationGames.Production.Common.Bank
{
    /// <summary>
    /// Операция оформления вклада.
    /// </summary>
    public class BankDebit : BankFinOperation
    {
        public override string DisplayName =>
            $"Оформление вклада для {Customer.DisplayName}, сумма {Sum}, ставка {Percent}";

        public BankDebit(GameTime time, Customer customer, string description = null)
            : base(time, customer, description)
        {
        }
    }
}