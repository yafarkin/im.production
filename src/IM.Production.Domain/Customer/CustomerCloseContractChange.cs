using System;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerCloseContractChange : CustomerChange
    {
        /// <summary>
        /// Закрытый контракт.
        /// </summary>
        public Contract ClosedContract { get; protected set; }

        public CustomerCloseContractChange(Contract closedContract, string description = null)
            : base(closedContract.Customer, description)
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