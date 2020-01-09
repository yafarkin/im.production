using System;
using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Bank;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;

namespace IM.Production.CalculationEngine
{
    /// <summary>
    /// Класс логики для взаимодействия с движком.
    /// </summary>
    public class Logic
    {
        protected CalculationEngine _engine;

        protected static object _lockObj = new object();

        public Logic(CalculationEngine engine)
        {
            _engine = engine;
        }

        /// <summary>
        /// Добавляет кредит или вклад для команды.
        /// </summary>
        /// <param name="customer">Команда.</param>
        /// <param name="finOperation">Операция открытия кредита или вклада.</param>
        public void TakeDebitOrCredit(Customer customer, BankFinOperation finOperation)
        {
            var isCredit = finOperation is BankCredit;

            if (null == customer)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (null == finOperation)
            {
                throw new ArgumentNullException(nameof(finOperation));
            }

            if (0 == finOperation.Days)
            {
                throw new ArgumentException("Количество дней должно быть больше 0", nameof(finOperation.Days));
            }

            if (finOperation.Sum < 0)
            {
                throw new ArgumentException("Сумма не может быть отрицательной", nameof(finOperation.Sum));
            }

            if (!isCredit && finOperation.Sum > customer.Sum)
            {
                throw new ArgumentException("На счёту команды не достаточно денег для открытия вклада", nameof(finOperation.Sum));
            }

            lock (_lockObj)
            {
                var finAction = new BankFinAction(finOperation.Time, customer, isCredit ? "Открытие кредита" : "Открытие вклада")
                    {
                        FinOperation = finOperation,
                        Sum = isCredit ? finOperation.Sum : -finOperation.Sum
                    };

                customer.BankFinanceOperations.Add(finOperation);
                customer.BankFinanceActions.Add(finAction);
                _engine.Game.AddActivity(finOperation);
                _engine.Game.AddActivity(finAction);

                customer.Sum += isCredit ? finOperation.Sum : -finOperation.Sum;
                var customerChange = new CustomerChange(finOperation.Time, customer, finAction.Description)
                {
                    SumChange = finAction.Sum,
                    FinAction = finAction
                };
                _engine.Game.AddActivity(customerChange);
            }
        }

        /// <summary>
        /// Добавляет указанный контракт в список действующих контрактов.
        /// </summary>
        /// <param name="contract">Объект контракта.</param>
        public void AddContract(Contract contract)
        {
            if (null == contract)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            if (null == contract.SourceFactory && null == contract.DestinationFactory)
            {
                throw new ArgumentNullException("Не указана исходная фабрика и фабрика назначения", nameof(contract));
            }

            if (null == contract.Customer)
            {
                throw new ArgumentNullException("Не указана команда", nameof(contract.Customer));
            }

            if ((null == contract.SourceFactory || null == contract.DestinationFactory) && (contract.TillCount.HasValue || contract.TillCount.HasValue))
            {
                throw new ArgumentException("При поставке товаров от игры или игре в контракте нельзя указать ограничений.");
            }

            lock (_lockObj)
            {
                var customerChange =
                    new CustomerChange(_engine.Game.Time, contract.Customer, "Добавление нового контракта")
                    {
                        NewContract = contract
                    };
                _engine.Game.AddActivity(customerChange);

                contract.Customer.Contracts.Add(contract);
                _engine.Game.AddActivity(contract);
            }
        }

        /// <summary>
        /// Закрытие контракта.
        /// </summary>
        /// <param name="contract">Контракт.</param>
        public void CloseContract(Contract contract)
        {
            if (null == contract)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            lock (_lockObj)
            {
                var customerChange =
                    new CustomerChange(_engine.Game.Time, contract.Customer, "Закрытие контракта")
                    {
                        ClosedContract = contract
                    };
                _engine.Game.AddActivity(customerChange);

                contract.Customer.Contracts.Remove(contract);
            }
        }

        /// <summary>
        /// Покупка командой новой фабрики.
        /// </summary>
        /// <param name="customer">Команда.</param>
        /// <param name="factoryDefinition">Описание фабрики, которую команда покупает.</param>
        public void BuyFactory(Customer customer, FactoryDefinition factoryDefinition, int workers = 0, List<Material> productionMaterials = null)
        {
            if (null == customer)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (null == factoryDefinition)
            {
                throw new ArgumentNullException(nameof(factoryDefinition));
            }

            var buySumm = ReferenceData.CalculateFactoryCostForBuy(factoryDefinition);
            if (customer.Sum < buySumm)
            {
                throw new InvalidOperationException($"На счету команды ({customer.Sum:C} не хватает средств для покупки фабрики ({buySumm})");
            }

            lock (_lockObj)
            {
                var factory = new Factory
                {
                    Customer = customer,
                    FactoryDefinition = factoryDefinition,
                    Workers = workers,
                    ProductionMaterials = productionMaterials ?? new List<Material>()
                };

                var customerChange =  new CustomerChange(_engine.Game.Time, customer, "Покупка фабрики")
                {
                    SumChange = -buySumm,
                    BoughtFactory = factory
                };
                _engine.Game.AddActivity(customerChange);

                customer.Sum -= buySumm;
                customer.Factories.Add(factory);
            }
        }

        /// <summary>
        /// Продажа командой фабрики.
        /// </summary>
        /// <param name="factory">Фабрика.</param>
        public void SellFactory(Factory factory)
        {
            if (null == factory)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            lock (_lockObj)
            {
                var sellSumm = ReferenceData.CalculateFactoryCostForSell(factory);

                var customerChange = new CustomerChange(_engine.Game.Time, factory.Customer, "Продажа фабрики")
                {
                    SumChange = +sellSumm,
                    SoldFactory = factory
                };
                _engine.Game.AddActivity(customerChange);

                factory.Customer.Sum += sellSumm;
                factory.Customer.Factories.Remove(factory);
            }
        }
    }
}
