using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class CustomerSumOnRDChange : CustomerChange
    {
        public CustomerSumOnRDChange(GameTime time, Customer customer, decimal newSumOnRd, string description = null) :
            base(time, customer, description)
        {
            NewSumOnRD = newSumOnRd;
        }
    }
}