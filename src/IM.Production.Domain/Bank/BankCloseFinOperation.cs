using System;

namespace Epam.ImitationGames.Production.Domain.Bank
{
    /// <summary>
    /// Операция закрытия вклада / кредита.
    /// </summary>
    /// <remarks>После этой записи должна следовать запись <see cref="BankFinAction"/> с указанием итоговой выплаты.</remarks>
    [Serializable]
    public class BankCloseFinOperation : BankFinOperation
    {
        /// <summary>
        /// Ссылка на исходный вклад / кредит.
        /// </summary>
        public BankFinOperation SourceOperation { get; set; }

        public override string DisplayName => $"Закрытие {(SourceOperation is BankDebit ? "вклада" : "кредита")}, сумма {Sum}";

        public BankCloseFinOperation(Customer customer, string description = null)
            : base(customer, 0, description)
        {
        }
    }
}