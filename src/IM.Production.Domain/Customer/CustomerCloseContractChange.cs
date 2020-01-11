using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerCloseContractChange : CustomerChange
    {
        public CustomerCloseContractChange(GameTime time, Contract closedContract,
            string description = null) : base(time, closedContract.Customer, description)
        {
            ClosedContract = closedContract;
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer.Contracts.Remove(ClosedContract);
        }
    }
}