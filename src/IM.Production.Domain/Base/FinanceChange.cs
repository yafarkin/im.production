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

        protected FinanceChange(GameTime time, Customer customer, decimal sumChange, string description = null)
            : base(time, customer, description)
        {
            SumChange = sumChange;
        }
    }
}