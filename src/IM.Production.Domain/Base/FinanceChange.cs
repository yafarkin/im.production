using System;

namespace Epam.ImitationGames.Production.Domain.Base
{
    /// <summary>
    /// Информация, связанная с изменением финасов.
    /// </summary>
    [Serializable]
    public abstract class FinanceChange : BaseChanging, IFinanceChanging
    {
        /// <summary>
        /// Сумма.
        /// </summary>
        public decimal SumChange { get; protected set; }

        protected FinanceChange(Customer customer, decimal sumChange, string description = null)
            : base(customer, description)
        {
            SumChange = sumChange;
        }
    }
}