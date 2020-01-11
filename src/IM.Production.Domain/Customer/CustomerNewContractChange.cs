using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerNewContractChange : CustomerChange
    {
        public CustomerNewContractChange(GameTime time, Contract newContract, string description = null)
            : base(time,  newContract.Customer, description)
        {
            NewContract = newContract;
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer.Contracts.Add(NewContract);
        }
    }
}