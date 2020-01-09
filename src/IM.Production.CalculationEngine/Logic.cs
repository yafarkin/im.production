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
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        public Customer AddCustomer(string login, string password, string name, ProductionType productionType)
        {
            lock (_lockObj)
            {
                var customer = new Customer
                {
                    Login = login,
                    PasswordHash = _engine.Game.GetMD5Hash(password),
                    DisplayName = name,
                    ProductionType = productionType,
                    FactoryGenerationLevel = 1,
                    Sum = ReferenceData.InitialCustomerBalance,
                    BankFinanceActions = new List<BankFinAction>(),
                    BankFinanceOperations = new List<BankFinOperation>(),
                    Contracts = new List<Contract>(),
                    Factories = new List<Factory>()
                };

                _engine.Game.Customers.Add(customer);

                var customerChange = new CustomerChange(_engine.Game.Time, customer, "Добавление новой команды");
                _engine.Game.AddActivity(customerChange);

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

            lock (_lockObj)
            {
                var customerChange = new CustomerChange(_engine.Game.Time, customer, $"Покупка фабрики у команды {otherCustomer.DisplayName}")
                {
                    OtherCustomer = otherCustomer,
                    BoughtFactory = factory,
                    SumChange = -cost
                };
                _engine.Game.AddActivity(customerChange);

                customer.Sum -= cost;
                customer.Factories.Add(factory);

                customerChange = new CustomerChange(_engine.Game.Time, otherCustomer, $"Продажа фабрики команде {customer.DisplayName}")
                {
                    OtherCustomer = customer,
                    SoldFactory = factory,
                    SumChange = cost
                };
                _engine.Game.AddActivity(customerChange);

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
        public void BuyFactoryFromGame(Customer customer, FactoryDefinition factoryDefinition, int workers = 0, List<Material> productionMaterials = null)
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
                throw new InvalidOperationException($"На счету команды ({customer.Sum:C} не хватает средств для покупки фабрики ({buySumm:C})");
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
