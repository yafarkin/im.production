using System;
using System.Collections.Generic;
using System.Linq;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Bank;
using Epam.ImitationGames.Production.Domain.Base;
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

        public Customer AddCustomer(string login, string password, string name, ProductionType productionType, decimal? initialBalance = null)
        {
            lock (_lockObj)
            {
                var customer = Customer.CreateCustomer(login, Game.GetMD5Hash(password), name, productionType);
                _game.Customers.Add(customer);

                _game.AddActivity(new InfoChanging(_game.Time, customer, "Добавление новой команды"));
                _game.AddActivity(new FinanceCustomerChange(_game.Time, customer, initialBalance ?? ReferenceData.InitialCustomerBalance,  "Установка начальной суммы на счёте"));
                
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
            if (null == customer)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (null == finOperation)
            {
                throw new ArgumentNullException(nameof(finOperation));
            }

            var isCredit = finOperation is BankCredit;

            if (finOperation.Sum <= 0)
            {
                throw new ArgumentException("Сумма не может быть меньше 0", nameof(finOperation.Sum));
            }

            if (!isCredit && finOperation.Sum > customer.Sum)
            {
                throw new ArgumentException("На счёту команды не достаточно денег для открытия вклада", nameof(finOperation.Sum));
            }

            finOperation.Days = isCredit ? ReferenceData.CreditDaysDefault : ReferenceData.DebitDaysDefault;
            finOperation.Percent = isCredit ? ReferenceData.CreditPercentDefault : ReferenceData.DebitPercentDefault;

            lock (_lockObj)
            {
                var sum = isCredit ? finOperation.Sum : -finOperation.Sum;

                var finAction =
                    new BankFinAction(finOperation.Time, customer, sum,
                        isCredit ? "Открытие кредита" : "Открытие вклада") {FinOperation = finOperation,};

                _game.AddActivity(finOperation);
                _game.AddActivity(finAction);
            }
        }

        /// <summary>
        /// Добавляет указанный контракт в список действующих контрактов.
        /// </summary>
        /// <param name="contract">Объект контракта.</param>
        public Contract AddContract(Contract contract)
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
                _game.AddActivity(new CustomerNewContractChange(_game.Time, contract, "Добавление нового контракта"));
                return contract;
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
                _game.AddActivity(new CustomerCloseContractChange(_game.Time, contract, "Закрытие контракта"));
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
                _game.AddActivity(new CustomerSumOnRDChange(_game.Time, customer, sumOnRD, $"Установка суммы на исследование фабрик следующего поколения = {sumOnRD:C}"));
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
                if (workers.HasValue)
                {
                    _game.AddActivity(new FactoryWorkerCountChange(_game.Time, factory, workers.Value, $"Установка кол-ва рабочих на фабрике = {workers.Value}"));
                }

                if (sumOnRD.HasValue)
                {
                    _game.AddActivity(new FactorySumOnRDChange(_game.Time, factory, sumOnRD.Value, $"Установка суммы на улучшение фабрики = {sumOnRD.Value:C}"));
                }

                if (productionMaterials != null)
                {
                    _game.AddActivity(new FactoryProductionMaterialChange(_game.Time, factory, productionMaterials, $"Изменение списка производимых материалов, кол-во = {productionMaterials.Count}"));
                }
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
                _game.AddActivity(new CustomerSellFactoryChange(_game.Time, factory, cost, customer, $"Продажа фабрики {factory.DisplayName} команде {customer.DisplayName} по цене {cost:C}"));
                _game.AddActivity(new CustomerBuyFactoryChange(_game.Time, customer, factory, cost, otherCustomer, $"Покупка фабрики {factory.DisplayName} у команды {otherCustomer.DisplayName}, по цене {cost:C}"));
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
                var factory = Factory.CreateFactory(customer, factoryDefinition);

                _game.AddActivity(new CustomerBuyFactoryChange(_game.Time, customer, factory, buySumm, null, "Покупка фабрики у игры"));
                _game.AddActivity(new FactoryWorkerCountChange(_game.Time, factory, 0 == workers ? factoryDefinition.BaseWorkers : workers));

                if (productionMaterials != null)
                {
                    _game.AddActivity(new FactoryProductionMaterialChange(_game.Time, factory, productionMaterials));
                }

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

                _game.AddActivity(new CustomerSellFactoryChange(_game.Time, factory, sellSumm, null, "Продажа фабрики игре"));
            }
        }
    }
}
