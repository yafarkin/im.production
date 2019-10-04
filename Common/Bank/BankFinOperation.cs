using Epam.ImitationGames.Production.Common.Base;

namespace Epam.ImitationGames.Production.Common.Bank
{
    /// <summary>
    /// Какая либо банковская операция по запросу команды.
    /// </summary>
    public abstract class BankFinOperation : BaseBank
    {
        protected BankFinOperation(GameTime time, Customer customer, string description = null)
            : base(time, customer, description)
        {
            Status = OperationStatus.Active;
        }

        /// <summary>
        /// Сумма изменения на счету команды (положительная - выплата команде, отрицательная - снятие со счёта команды).
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Процент, под который выдан кредит (на один игровой день) или вклад.
        /// </summary>
        public decimal Percent { get; set; }

        /// <summary>
        /// Количество игровых дней, на которые выдан кредит.
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Статус операции.
        /// </summary>
        public OperationStatus Status { get; set; }
    }
}