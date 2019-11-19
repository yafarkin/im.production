using Epam.ImitationGames.Production.Common.Bank;
using Epam.ImitationGames.Production.Common.Base;
using Epam.ImitationGames.Production.Common.Production;
using System.Collections.Generic;

namespace Epam.ImitationGames.Production.Common
{
    /// <summary>
    /// Команда (производитель).
    /// </summary>
    public class Customer : VisibleEntity
    {
        public Customer()
        {
            Factories = new List<Factory>();
            Contracts = new List<Contract>();
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
        public decimal NeedSumToNextGenerationLevel { get; set; }

        /// <summary>
        /// Уже потраченная сумма на исследования фабрик следующего уровня.
        /// </summary>
        public decimal SpentSumToNextGenerationLevel { get; set; }

        /// <summary>
        /// Общий прогресс иследования, для открытия фабрик следующего уровня.
        /// </summary>
        public decimal RDProgress => NeedSumToNextGenerationLevel / SpentSumToNextGenerationLevel;

        /// <summary>
        /// Итоговая сумма на счету команды
        /// </summary>
        public decimal Sum { get; set; }
    }
}
