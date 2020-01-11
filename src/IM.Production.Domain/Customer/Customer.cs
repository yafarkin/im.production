﻿using System;
using Epam.ImitationGames.Production.Domain.Bank;
using Epam.ImitationGames.Production.Domain.Base;
using Epam.ImitationGames.Production.Domain.Production;
using System.Collections.Generic;

namespace Epam.ImitationGames.Production.Domain
{
    /// <summary>
    /// Команда (производитель).
    /// </summary>
    [Serializable]
    public class Customer : VisibleEntity
    {
        public Customer()
        {
            Factories = new List<Factory>();
            Contracts = new List<Contract>();
            BankFinanceOperations = new List<BankFinOperation>();
            BankFinanceActions = new List<BankFinAction>();
            FactoryGenerationLevel = 1;
        }

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Хэш пароля.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Область производства.
        /// </summary>
        public ProductionType ProductionType { get; set; }

        /// <summary>
        /// Список фабрик.
        /// </summary>
        public List<Factory> Factories { get; set; }

        /// <summary>
        /// Заключенные контракты.
        /// </summary>
        public List<Contract> Contracts { get; set; }

        /// <summary>
        /// Все банковские операции.
        /// </summary>
        public List<BankFinOperation> BankFinanceOperations { get; set; }

        /// <summary>
        /// Все банковские действия с операционным счетом команды.
        /// </summary>
        public List<BankFinAction> BankFinanceActions { get; set; }

        /// <summary>
        /// Доступный уровень фабрик для данной команды.
        /// </summary>
        public int FactoryGenerationLevel { get; set; }

        /// <summary>
        /// Сумма, выделяемая на RD, для исследования фабрик следующего уровня.
        /// </summary>
        public decimal SumOnRD { get; set; }

        /// <summary>
        /// Требуемая сумма для открытия фабрик следующего уровня.
        /// </summary>
        public decimal SumToNextGenerationLevel { get; set; }

        /// <summary>
        /// Уже потраченная сумма на исследования фабрик следующего уровня.
        /// </summary>
        public decimal SpentSumToNextGenerationLevel { get; set; }

        public bool ReadyForNextGenerationLevel => SpentSumToNextGenerationLevel > SumToNextGenerationLevel;

        /// <summary>
        /// Общий прогресс исследования, для открытия фабрик следующего уровня.
        /// </summary>
        public decimal RDProgress => SpentSumToNextGenerationLevel / SumToNextGenerationLevel;

        /// <summary>
        /// Итоговая сумма на счету команды
        /// </summary>
        public decimal Sum { get; protected set; }

        public void AddSum(decimal sum) => Sum += sum;

        public void SetSum(decimal sum) => Sum = sum;
    }
}