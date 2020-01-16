using System;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerNewContractChange : CustomerChange
    {
        /// <summary>
        /// Заключенный контракт.
        /// </summary>
        public Contract NewContract { get; protected set; }

        public CustomerNewContractChange(Contract newContract, string description = null)
            : base(newContract.Customer, description)
        {
            NewContract = newContract;
        }

        public override void DoAction()
        {
            base.DoAction();
            Customer._contracts.Add(NewContract);
        }
    }
}