using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Bank
{
    /// <summary>
    /// Какое либо банковское действие со счётом команды.
    /// </summary>
    public class BankFinAction : BaseBank
    {
        /// <summary>
        /// Ссылка на исходный вклад / кредит.
        /// </summary>
        public BankFinOperation FinOperation { get; set; }

        /// <summary>
        /// Сумма тела, которая будет начислена / снята со счёта команды.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Сумма по процентам, которая будет начислена / снята со счёта команды
        /// </summary>
        public decimal PercentSum { get; set; }

        /// <summary>
        /// Сумма штрафа за неисполнение обязательств перед банком (если не хватает средств на выплату по кредиту).
        /// </summary>
        public decimal Fine { get; set; }

        /// <summary>
        /// Итоговая сумма, которая будет начислена / снята со счёта команды
        /// </summary>
        public decimal TotalSum => Sum + PercentSum - Fine;

        public override string DisplayName => $"Выплаты по {(FinOperation is BankDebit ? "вкладу" : "кредиту")}, сумма {TotalSum}";

        public BankFinAction(GameTime time, Customer customer, string description = null)
            : base(time, customer, description)
        {
        }
    }
}