﻿using System;

namespace Epam.ImitationGames.Production.Domain.Bank
{
    /// <summary>
    /// Какая либо банковская операция по запросу команды.
    /// </summary>
    [Serializable]
    public abstract class BankFinOperation : BaseBank
    {
        protected BankFinOperation(Customer customer, decimal sum, string description = null)
            : base(customer, description)
        {
            Status = OperationStatus.Active;
            Sum = sum;
        }

        /// <summary>
        /// Сумма изменения на счету команды.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Процент, под который выдан кредит (на один игровой день) или вклад.
        /// </summary>
        public decimal Percent { get; set; }

        /// <summary>
        /// Количество игровых дней, на которые выдан кредит.
        /// </summary>
        public uint Days { get; set; }

        /// <summary>
        /// Статус операции.
        /// </summary>
        public OperationStatus Status { get; set; }

        public override void DoAction()
        {
            base.DoAction();
            Customer._bankFinanceOperations.Add(this);
        }
    }
}