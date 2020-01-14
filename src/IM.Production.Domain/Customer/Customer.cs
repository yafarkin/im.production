using System;
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
            _factories = new List<Factory>();
            _contracts = new List<Contract>();
            _bankFinanceOperations = new List<BankFinOperation>();
            FactoryGenerationLevel = 1;
        }

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login { get; protected set; }

        /// <summary>
        /// Хэш пароля.
        /// </summary>
        public string PasswordHash { get; protected set; }

        /// <summary>
        /// Область производства.
        /// </summary>
        public ProductionType ProductionType { get; protected set; }

        /// <summary>
        /// Список фабрик.
        /// </summary>
        protected List<Factory> _factories { get; set; }

        /// <summary>
        /// Публичный список фабрик.
        /// </summary>
        public IEnumerable<Factory> Factories => _factories;

        /// <summary>
        /// Контракты.
        /// </summary>
        protected List<Contract> _contracts { get; set; }

        /// <summary>
        /// Публичный список контрактов.
        /// </summary>
        public IEnumerable<Contract> Contracts => _contracts;

        /// <summary>
        /// Все банковские операции.
        /// </summary>
        protected List<BankFinOperation> _bankFinanceOperations { get; set; }

        /// <summary>
        /// Публичный список всех банковских операций.
        /// </summary>
        public IEnumerable<BankFinOperation> BankFinanceOperations => _bankFinanceOperations;

        /// <summary>
        /// Доступный уровень фабрик для данной команды.
        /// </summary>
        public int FactoryGenerationLevel { get; protected set; }

        /// <summary>
        /// Сумма, выделяемая на RD, для исследования фабрик следующего уровня.
        /// </summary>
        public decimal SumOnRD { get; protected set; }

        /// <summary>
        /// Требуемая сумма для открытия фабрик следующего уровня.
        /// </summary>
        public decimal SumToNextGenerationLevel { get; protected set; }

        /// <summary>
        /// Уже потраченная сумма на исследования фабрик следующего уровня.
        /// </summary>
        public decimal SpentSumToNextGenerationLevel { get; protected set; }

        public bool ReadyForNextGenerationLevel => SpentSumToNextGenerationLevel > SumToNextGenerationLevel;

        /// <summary>
        /// Общий прогресс исследования, для открытия фабрик следующего уровня.
        /// </summary>
        public decimal RDProgress => SpentSumToNextGenerationLevel / SumToNextGenerationLevel;

        /// <summary>
        /// Итоговая сумма на счету команды
        /// </summary>
        public decimal Sum { get; protected set; }

        internal void AddSum(decimal sum) => Sum += sum;

        internal void SetSum(decimal sum) => Sum = sum;

        internal void SetFactoryGenerationLevel(int level) => FactoryGenerationLevel = level;

        internal void SetSumOnRD(decimal sum) => SumOnRD = sum;

        internal void AddSpentSumOnRD(decimal sum) => SpentSumToNextGenerationLevel += sum;

        internal void SetSumInfoForRD(decimal spentSum, decimal sumToNextLevel)
        {
            SpentSumToNextGenerationLevel = spentSum;
            SumToNextGenerationLevel = sumToNextLevel;
        }

        internal void AddBankFinOperation(BankFinOperation operation) => _bankFinanceOperations.Add(operation);

        internal void AddFactory(Factory factory) => _factories.Add(factory);

        internal void DelFactory(Factory factory) => _factories.Remove(factory);

        internal void AddContract(Contract contract) => _contracts.Add(contract);

        internal void DelContract(Contract contract) => _contracts.Remove(contract);

        public static Customer CreateCustomer(string login, string passwordHash, string displayName, ProductionType productionType)
        {
            var customer = new Customer
            {
                Login = login,
                PasswordHash = passwordHash,
                DisplayName = displayName,
                ProductionType = productionType,
            };

            return customer;
        }
    }
}
