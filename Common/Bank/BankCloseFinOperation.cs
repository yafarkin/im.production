using Epam.ImitationGames.Production.Common.Base;

namespace Epam.ImitationGames.Production.Common.Bank
{
    /// <summary>
    /// Операция закрытия вклада / кредита.
    /// </summary>
    /// <remarks>После этой записи должна следовать запись <see cref="BankFinAction"/> с указанием итоговой выплаты.</remarks>
    public class BankCloseFinOperation : BankFinOperation
    {
        /// <summary>
        /// Ссылка на исходный вклад / кредит.
        /// </summary>
        public BankFinOperation SourceOperation { get; set; }

        public override string DisplayName => $"Закрытие {(SourceOperation is BankDebit ? "вклада" : "кредита")}, сумма {Sum}";

        public BankCloseFinOperation(GameTime time, Customer customer, string description = null)
            : base(time, customer, description)
        {
        }
    }
}