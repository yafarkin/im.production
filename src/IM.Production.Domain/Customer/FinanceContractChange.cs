﻿using System;

namespace Epam.ImitationGames.Production.Domain
{
    [Serializable]
    public class FinanceContractChange : FinanceCustomerChange
    {
        public Contract Contract { get; set; }

        public int Amount { get; set; }

        public decimal NetSum { get; set; }
        public decimal TaxSum { get; set; }

        public FinanceContractChange(Contract contract, decimal netSum, decimal taxSum, int amount, string description = null) :
            base(contract.Customer, netSum+taxSum, description)
        {
            Contract = contract;
            Amount = amount;
            NetSum = netSum;
            TaxSum = taxSum;
        }

        public override void DoAction()
        {
            base.DoAction();

            Contract.TotalSumm += SumChange;
            Contract.TotalOnTaxes += TaxSum;
            Contract.TotalCountCompleted += Amount;
        }
    }
}