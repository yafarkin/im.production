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
        internal List<Factory> _factories { get; set; }

        /// <summary>
        /// Публичный список фабрик.
        /// </summary>
        public IEnumerable<Factory> Factories => _factories;

        /// <summary>
        /// Контракты.
        /// </summary>
        internal List<Contract> _contracts { get; set; }

        /// <summary>
        /// Публичный список контрактов.
        /// </summary>
        public IEnumerable<Contract> Contracts => _contracts;

        /// <summary>
        /// Все банковские операции.
        /// </summary>
        internal List<BankFinOperation> _bankFinanceOperations { get; set; }

        /// <summary>
        /// Публичный список всех банковских операций.
        /// </summary>
        public IEnumerable<BankFinOperation> BankFinanceOperations => _bankFinanceOperations;

        /// <summary>
        /// Доступный уровень фабрик для данной команды.
        /// </summary>
        public int FactoryGenerationLevel { get; internal set; }

        /// <summary>
        /// Сумма, выделяемая на RD, для исследования фабрик следующего уровня.
        /// </summary>
        public decimal SumOnRD { get; internal set; }

        /// <summary>
        /// Требуемая сумма для открытия фабрик следующего уровня.
        /// </summary>
        public decimal SumToNextGenerationLevel { get; internal set; }

        /// <summary>
        /// Уже потраченная сумма на исследования фабрик следующего уровня.
        /// </summary>
        public decimal SpentSumToNextGenerationLevel { get; internal set; }

        public bool ReadyForNextGenerationLevel => SpentSumToNextGenerationLevel >= SumToNextGenerationLevel;

        /// <summary>
        /// Общий прогресс исследования, для открытия фабрик следующего уровня.
        /// </summary>
        public decimal RDProgress => 0 == SumToNextGenerationLevel ? 0 : SpentSumToNextGenerationLevel / SumToNextGenerationLevel;

        /// <summary>
        /// Итоговая сумма на счету команды
        /// </summary>
        public decimal Sum { get; internal set; }

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
