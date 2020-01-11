﻿using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class FinanceCustomerInsurancePremiumChange : FinanceCustomerChange
    {
        public FinanceCustomerInsurancePremiumChange(GameTime time, Customer customer, decimal sumChange,
            string description = null) : base(time, customer, sumChange, description)
        {
        }
    }
}