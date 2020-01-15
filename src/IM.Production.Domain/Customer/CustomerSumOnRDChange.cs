using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerSumOnRDChange : CustomerChange
    {
        /// <summary>
        /// Изменение суммы, выделяемой на RD.
        /// </summary>
        public decimal NewSumOnRD { get; protected set; }

        public CustomerSumOnRDChange(GameTime time, Customer customer, decimal newSumOnRd, string description = null) :
            base(time, customer, description)
        {
            NewSumOnRD = newSumOnRd;
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer.SumOnRD = NewSumOnRD;
        }
    }
}