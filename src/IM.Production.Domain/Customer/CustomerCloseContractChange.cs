using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerCloseContractChange : CustomerChange
    {
        /// <summary>
        /// Закрытый контракт.
        /// </summary>
        public Contract ClosedContract { get; protected set; }

        public CustomerCloseContractChange(GameTime time, Contract closedContract, string description = null)
            : base(time, closedContract.Customer, description)
        {
            ClosedContract = closedContract;
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer._contracts.Remove(ClosedContract);
        }
    }
}