using System;
using System.Collections.Generic;
using System.Linq;
using CalculationEngine;
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
        protected Game _game;

        protected static object _lockObj = new object();

        public Logic(Game game)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public Customer AddCustomer(string login, string password, string name, ProductionType productionType)
        {
            lock (_lockObj)
            {
                var customer = new Customer
                {
                    Login = login,
                    PasswordHash = _game.GetMD5Hash(password),
                    DisplayName = name,
                    ProductionType = productionType,
                    FactoryGenerationLevel = 1,
                    Sum = ReferenceData.InitialCustomerBalance,
                    BankFinanceActions = new List<BankFinAction>(),
                    BankFinanceOperations = new List<BankFinOperation>(),
                    Contracts = new List<Contract>(),
                    Factories = new List<Factory>()
                };

                _game.Customers.Add(customer);

                var customerChange = new CustomerChange(_game.Time, customer, "Добавление новой команды");
                _game.AddActivity(customerChange);

                return customer;
            }
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

            finOperation.Days = isCredit ? ReferenceData.CreditDaysDefault : ReferenceData.DebitDaysDefault;
            finOperation.Percent = isCredit ? ReferenceData.CreditPercentDefault : ReferenceData.DebitPercentDefault;

            if (finOperation.Sum <= 0)
            {
                throw new ArgumentException("Сумма не может быть меньше 0", nameof(finOperation.Sum));
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
                _game.AddActivity(finOperation);
                _game.AddActivity(finAction);

                customer.Sum += isCredit ? finOperation.Sum : -finOperation.Sum;
                var customerChange = new CustomerChange(finOperation.Time, customer, finAction.Description)
                {
                    SumChange = finAction.Sum,
                    FinAction = finAction
                };
                _game.AddActivity(customerChange);
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
                    new CustomerChange(_game.Time, contract.Customer, "Добавление нового контракта")
                    {
                        NewContract = contract
                    };
                _game.AddActivity(customerChange);

                contract.Customer.Contracts.Add(contract);
                _game.AddActivity(contract);
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
                    new CustomerChange(_game.Time, contract.Customer, "Закрытие контракта")
                    {
                        ClosedContract = contract
                    };
                _game.AddActivity(customerChange);

                contract.Customer.Contracts.Remove(contract);
            }
        }

        public void UpdateCustomerSettings(Customer customer, decimal sumOnRD)
        {
            if (null == customer)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (sumOnRD < 0)
            {
                throw new ArgumentException("Сумма на иследования не может быть отрицательной", nameof(sumOnRD));
            }

            lock (_lockObj)
            {
                var customerChange = new CustomerChange(_game.Time, customer, "Изменение параметров команды")
                {
                    SumOnRDChange = sumOnRD - customer.SumOnRD
                };
                customer.SumOnRD = sumOnRD;
                _game.AddActivity(customerChange);
            }
        }

        public void UpdateFactorySettings(Factory factory, int? workers = null, decimal? sumOnRD = null, IList<Material> productionMaterials = null)
        {
            if (null == factory)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if(null == workers && null == sumOnRD && null == productionMaterials)
            {
                return;
            }

            if (workers < 0)
            {
                throw new ArgumentException("Количество рабочих не может быть отрицательным", nameof(workers));
            }

            if (sumOnRD < 0)
            {
                throw new ArgumentException("Сумма на исследования не может быть отрицательной", nameof(sumOnRD));
            }

            if (productionMaterials != null && productionMaterials.Any())
            {
                var factoryCanProduce =  factory.FactoryDefinition.CanProductionMaterials
                    .Where(x => x.Key <= factory.Level)
                    .SelectMany(x => x.Value)
                    .ToList();

                foreach (var productionMaterial in productionMaterials)
                {
                    if (factoryCanProduce.All(x => x.Id != productionMaterial.Id))
                    {
                        throw new ArgumentException($"Указанный материал {productionMaterial.DisplayName} не может быть произведен на этой фабрике");
                    }
                }
            }

            lock(_lockObj)
            {
                var factoryChange = new FactoryChange(_game.Time, factory, "Изменение параметров фабрики");

                if (workers != null)
                {
                    factoryChange.WorkersChange = workers.Value - factory.Workers;
                    factory.Workers = workers.Value;
                }

                if (sumOnRD != null)
                {
                    factoryChange.SumOnRDChange = sumOnRD.Value - factoryChange.SumOnRDChange;
                    factory.SumOnRD = sumOnRD.Value;
                }

                if (productionMaterials != null && productionMaterials.Any())
                {
                    factoryChange.Description += "; изменение списка производимых материалов";
                    factory.ProductionMaterials = new List<Material>(productionMaterials);
                }

                _game.AddActivity(factoryChange);
            }
        }

        /// <summary>
        /// Покупка фабрики у другой команды.
        /// </summary>
        /// <param name="customer">Команда, которая покупает.</param>
        /// <param name="factory">Фабрику, которая команда покупает.</param>
        /// <param name="cost">Стоимость продажи фабрики.</param>
        public void BuyFactoryFromOtherCustomer(Customer customer, Factory factory, decimal cost)
        {
            if (null == customer)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (null == factory)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (cost <= 0)
            {
                throw new ArgumentException("Стоимость фабрики должна быть больше 0");
            }

            if (customer.Sum < cost)
            {
                throw new InvalidOperationException($"На счету команды ({customer.Sum:C} не хватает средств для покупки фабрики ({cost:C})");
            }

            var otherCustomer = factory.Customer;

            if (otherCustomer.Id == customer.Id)
            {
                throw new InvalidOperationException("Нельзя продать фабрику самому себе");
            }

            if (customer.ProductionType.Id != otherCustomer.ProductionType.Id)
            {
                throw new InvalidOperationException("Обе команды должны относится к одному и тому же типу производства");
            }

            lock (_lockObj)
            {
                var customerChange = new CustomerChange(_game.Time, customer, $"Покупка фабрики у команды {otherCustomer.DisplayName}")
                {
                    OtherCustomer = otherCustomer,
                    BoughtFactory = factory,
                    SumChange = -cost
                };
                _game.AddActivity(customerChange);

                customer.Sum -= cost;
                customer.Factories.Add(factory);

                customerChange = new CustomerChange(_game.Time, otherCustomer, $"Продажа фабрики команде {customer.DisplayName}")
                {
                    OtherCustomer = customer,
                    SoldFactory = factory,
                    SumChange = cost
                };
                _game.AddActivity(customerChange);

                otherCustomer.Sum += cost;
                otherCustomer.Factories.Remove(factory);

                factory.Customer = customer;
            }
        }

        /// <summary>
        /// Покупка командой новой фабрики (у игры).
        /// </summary>
        /// <param name="customer">Команда.</param>
        /// <param name="factoryDefinition">Описание фабрики, которую команда покупает.</param>
        /// <returns>Купленная фабрика.</returns>
        public Factory BuyFactoryFromGame(Customer customer, FactoryDefinition factoryDefinition, int workers = 0, List<Material> productionMaterials = null)
        {
            if (null == customer)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (null == factoryDefinition)
            {
                throw new ArgumentNullException(nameof(factoryDefinition));
            }

            if (factoryDefinition.ProductionType.Id != customer.ProductionType.Id)
            {
                throw new InvalidOperationException($"Команда с типом {customer.ProductionType.DisplayName} не может купить фабрику типа {factoryDefinition.ProductionType.DisplayName}");
            }

            if (factoryDefinition.GenerationLevel > customer.FactoryGenerationLevel)
            {
                throw new InvalidOperationException($"Команда ещё не исследовала фабрики этого уровня ({factoryDefinition.GenerationLevel} > {customer.FactoryGenerationLevel})");
            }

            var buySumm = ReferenceData.CalculateFactoryCostForBuy(factoryDefinition);
            if (customer.Sum < buySumm)
            {
                throw new InvalidOperationException($"На счету команды ({customer.Sum:C} не хватает средств для покупки фабрики ({buySumm:C})");
            }

            lock (_lockObj)
            {
                var factory = new Factory
                {
                    Customer = customer,
                    FactoryDefinition = factoryDefinition,
                    Workers = 0 == workers ? factoryDefinition.BaseWorkers : workers,
                    ProductionMaterials = productionMaterials ?? new List<Material>()
                };

                var customerChange =  new CustomerChange(_game.Time, customer, "Покупка фабрики")
                {
                    SumChange = -buySumm,
                    BoughtFactory = factory
                };
                _game.AddActivity(customerChange);

                customer.Sum -= buySumm;
                customer.Factories.Add(factory);

                return factory;
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

                var customerChange = new CustomerChange(_game.Time, factory.Customer, "Продажа фабрики")
                {
                    SumChange = +sellSumm,
                    SoldFactory = factory
                };
                _game.AddActivity(customerChange);

                factory.Customer.Sum += sellSumm;
                factory.Customer.Factories.Remove(factory);
            }
        }
    }
}
