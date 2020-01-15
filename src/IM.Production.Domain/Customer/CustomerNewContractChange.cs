using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerNewContractChange : CustomerChange
    {
        /// <summary>
        /// Заключенный контракт.
        /// </summary>
        public Contract NewContract { get; protected set; }

        public CustomerNewContractChange(GameTime time, Contract newContract, string description = null)
            : base(time,  newContract.Customer, description)
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