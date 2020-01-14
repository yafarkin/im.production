using System;
using System.Collections.Generic;
using System.Linq;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Bank;
using Epam.ImitationGames.Production.Domain.Base;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;
using Epam.ImitationGames.Production.Domain.Static;

namespace IM.Production.CalculationEngine
{
    public class CalculationEngine
    {
        protected static object _lockObj = new object();

        protected Game _game { get; set; }

        public CalculationEngine(Game game)
        {
            _game = game;
        }

        public void Calculate()
        {
            _game.AddActivity(new InfoChanging(_game.Time, null, "Начало игрового цикла"));

            lock (_lockObj)
            {
                // 1. Осуществляем поставки по контрактам от игры на фабрику
                // 2. Выплачиваем суммы за модернизацию предприятия
                // 3. Выплачиваем суммы за исследования на следующее поколение мероприятий
                // 4. Выплачиваем налоги на фабрики
                // 5. Осуществляем расчёт цен, по которым игра закупает материалы
                // 6. В цикле, по уровням производства (начиная с самого низшего):
                // 6.1. Выполняем производство согласно настройкам фабрики и помещаем на склад.
                // 6.2. Осуществляем передачу материалов согласно контракту (другим игрокам или игре), включая выплаты штрафов, страховым премий, получения страховок
                // 7. Осуществляем поставки по контрактам от фабрике к игре
                // 8. Осуществляем банковские операции
                // 8.1. Получаем проценты на счёт от банка по вкладам
                // 8.2. Выплачиваем проценты и тело кредита банку
                // 9. В последний игровой день автоматически закрываем все кредиты и вклады в банках.
                // 10. Прибавляем счётчик игровых дней.

                UpdateCurrentGameProperties();

                // п.1. Осуществляем поставки по контрактам от игры на фабрику
                foreach (var customer in _game.Customers)
                {
                    var contracts = customer.Contracts.Where(c => c.SourceFactory == null);
                    foreach (var contract in contracts)
                    {
                        ProcessGameToCustomerContract(contract);
                    }
                }

                // п. 2. Выплачиваем суммы за исследования на следующее поколение мероприятий
                foreach (var customer in _game.Customers)
                {
                    ProcessCustomerRD(customer);
                }

                // п. 3. Выплачиваем суммы за модернизацию предприятия и осуществляем его модернизацию
                foreach (var customer in _game.Customers)
                {
                    foreach (var factory in customer.Factories)
                    {
                        ProcessFactoryRD(factory, customer);
                    }
                }

                // п. 4. Выплачиваем налоги на фабрики
                foreach (var customer in _game.Customers)
                {
                    foreach (var factory in customer.Factories)
                    {
                        PaidTaxes(factory, customer);
                    }
                }

                // п.5. Выполняем расчёт цен, по которым игра покупает материалы
                ReferenceData.UpdateGameDemand(_game.Customers.SelectMany(c => c.Factories).ToList());

                // п.6. Осуществление производства
                // TODO It might be we don't need to process factories sequentually to generations, but it can be done in a random order
                var maxGeneration = _game.Customers.SelectMany(c => c.Factories)
                    .Select(f => f.FactoryDefinition.GenerationLevel).DefaultIfEmpty().Max();

                for (var generation = 1; generation <= maxGeneration; generation++)
                {
                    foreach (var customer in _game.Customers)
                    {
                        var factories =
                            customer.Factories.Where(f => f.FactoryDefinition.GenerationLevel == generation);

                        foreach (var factory in factories)
                        {
                            ProduceFactory(factory);

                            var contracts = customer.Contracts.Where(c => c.SourceFactory != null && c.SourceFactory.Id == factory.Id);

                            foreach (var contract in contracts)
                            {
                                ProcessContract(contract);
                            }
                        }
                    }
                }

                // п.7. Осуществляем поставки по контрактам от игры на фабрику
                foreach (var customer in _game.Customers)
                {
                    var contracts = customer.Contracts.Where(c => c.DestinationFactory == null);
                    foreach (var contract in contracts)
                    {
                        ProcessContract(contract);
                    }
                }

                // п. 8. Осуществляем банковские операции
                // п. 8.1. Получаем проценты на счёт от банка по вкладам
                // п. 8.2. Выплачиваем проценты и тело кредита банку
                foreach (var customer in _game.Customers)
                {
                    DepositProcessing(customer);
                }

                // п. 9. В последний игровой день автоматически закрываем все кредиты и вклады в банках.
                if (_game.Time.Day == _game.TotalGameDays)
                {
                }

                // п. 10. Прибавляем счётчик игровых дней.
                _game.Time.Day++;
                _game.Time.When = DateTime.UtcNow;
            }

            _game.AddActivity(new InfoChanging(_game.Time, null, "Завершение игрового цикла"));
        }

        /// <summary>
        /// Обновление данных текущей игры для упрощенного создания объектов.
        /// </summary>
        protected void UpdateCurrentGameProperties()
        {
            CurrentGameProps.GameDay = _game.Time.Day;
        }

        /// <summary>
        /// Начисление процентов по вкладу и выплата накоплений в случае, если срок вклада подошел к концу.
        /// </summary>
        /// <param name="customer">Команда</param>
        protected void DepositProcessing(Customer customer)
        {
            var time = new GameTime();
            foreach (var operation in customer.BankFinanceOperations)
            {
                if (operation.Status == OperationStatus.Active && operation is BankDebit debit)
                {
                    // Создаем операцию начисления процента по вкладу.
                    var debitAction = new BankFinAction(time, customer, 0, "Начисление процентов по вкладу")
                    {
                        FinOperation = debit, PercentSum = (debit.Sum / 100) * debit.Percent, Fine = 0
                    };

                    _game.AddActivity(debitAction);

                    debit.Sum += debitAction.TotalSum;

                    // Выплачиваем деньги игроку, если срок вклада подошел к концу и закрываем вклад.
                    if (debit.Time.Day + debit.Days == _game.Time.Day)
                    {
                        var closeDebitOperation = new BankCloseFinOperation(time, customer)
                        {
                            SourceOperation = debit,
                            Sum = debit.Sum,
                            Percent = debit.Percent,
                            Days = debit.Days
                        };

                        _game.AddActivity(closeDebitOperation);

                        debit.Status = OperationStatus.Close;

                        var closeDebitAction = new BankFinAction(time, customer, closeDebitOperation.Sum)
                        {
                            FinOperation = debit, PercentSum = 0, Fine = 0
                        };

                        _game.AddActivity(new FinanceCustomerChange(_game.Time, customer, closeDebitAction.TotalSum,
                            $"Закрытие вклада, начисление {closeDebitAction.TotalSum}"));
                    }
                }
            }
        }

        protected void ProcessCustomerRD(Customer customer)
        {
            if (customer.SumToNextGenerationLevel == 0)
            {
                _game.AddActivity(new CustomerRDSpentChange(_game.Time, customer, null, ReferenceData.CalculateRDSummToNextGenerationLevel(customer)));
            }

            if (customer.SumOnRD <= 0)
            {
                return;
            }

            var sumOnRD = customer.SumOnRD;

            if (customer.Sum < sumOnRD)
            {
                var infoChange = new InfoChanging(_game.Time, customer, $"Исследование следующего поколения фабрик приостановлено, т.к. не хватает денег ({sumOnRD:C} > {customer.Sum:C}).");
                _game.AddActivity(infoChange);
                return;
            }

            var time = new GameTime();

            _game.AddActivity(new FinanceCustomerOnRDChange(time, customer, -sumOnRD,
                $"Процент исследования следующего поколения фабрик: {customer.RDProgress:P}, сумма {sumOnRD:C}"));

            if (customer.ReadyForNextGenerationLevel)
            {
                var newGenerationLevel = customer.FactoryGenerationLevel + 1;

                _game.AddActivity(new CustomerGenerationLevelChange(time, customer, newGenerationLevel,
                    customer.SpentSumToNextGenerationLevel - customer.SumToNextGenerationLevel,
                    ReferenceData.CalculateRDSummToNextGenerationLevel(customer, newGenerationLevel),
                    $"Исследован новый уровень поколения фабрик: {newGenerationLevel}"));
            }
        }

        protected void ProcessFactoryRD(Factory factory, Customer customer)
        {
            if (factory.NeedSumToNextLevelUp == 0)
            {
                _game.AddActivity(new FactoryRDSpentChange(_game.Time, factory, 0, ReferenceData.CalculateRDSummToNextFactoryLevelUp(factory)));
            }

            if (factory.SumOnRD <= 0)
            {
                return;
            }

            var sumOnRD = factory.SumOnRD;

            if (customer.Sum < sumOnRD)
            {
                var infoChange = new InfoChanging(_game.Time, customer, $"Исследование улучшения фабрики {factory.DisplayName} приостановлено, т.к. не хватает денег ({sumOnRD:C} > {customer.Sum:C}).");
                _game.AddActivity(infoChange);
                return;
            }

            var time = new GameTime();
            _game.AddActivity(new FactoryRDSpentChange(_game.Time, factory, factory.SpentSumToNextLevelUp + sumOnRD, null));
            _game.AddActivity(new FinanceFactoryChange(time, factory, -sumOnRD, 0, 0,
                $"Оплата исследований на {factory.DisplayName} ({factory.FactoryDefinition.ProductionType.DisplayName}), сумма {sumOnRD:C}"));

            if (factory.ReadyForNextLevel)
            {
                var newLevel = factory.Level + 1;

                _game.AddActivity(new FactoryLevelChange(time, factory, newLevel
                    , factory.SpentSumToNextLevelUp - factory.NeedSumToNextLevelUp,
                    ReferenceData.CalculateRDSummToNextFactoryLevelUp(factory, newLevel),
                    $"Фабрика {factory.DisplayName} достигла уровня {newLevel}"));
            }
        }

        protected void PaidTaxes(Factory factory, Customer customer)
        {
            var taxSum = ReferenceData.CalculateTaxOnFactory(factory);
            _game.AddActivity(new TaxFactoryChange(_game.Time, factory, -taxSum, $"Оплата налога на фабрику {factory.DisplayName} ({factory.FactoryDefinition.ProductionType.DisplayName}), сумма налога {taxSum:C}"));
        }

        protected void ProduceFactory(Factory factory)
        {
            _game.AddActivity(new FactoryPerformanceChange(_game.Time, factory, ReferenceData.CalculateFactoryPerformance(factory)));

            // 2. выполняем производство материалов и помещаем их на склад

            // 2.1. определяем, что вообще сможем произвести
            var productionMaterials = new List<Material>(factory.ProductionMaterials);
            var usedMaterialsOnStock = new List<MaterialOnStock>();

            decimal performancePerMaterial = 0;
            var time = new GameTime();

            // TODO Checking algorithm to be refactored
            while (productionMaterials.Any())
            {
                for (var i = 0; i < productionMaterials.Count; i++)
                {
                    // смотрим - есть ли все необходимые исходные материалы на складе
                    var material = productionMaterials[i];
                    var currentUsedMaterialsOnStock = new List<MaterialOnStock>();
                    var allMaterialsOnStock = true;

                    performancePerMaterial = decimal.Divide(1, productionMaterials.Count);

                    foreach (var inputMaterial in material.InputMaterials)
                    {
                        var materialOnStock = factory.Stock.FirstOrDefault(m => m.Material.Id == inputMaterial.Material.Id);
                        var requiredAmount = inputMaterial.Amount * factory.Performance * performancePerMaterial;

                        if (materialOnStock == null || materialOnStock.Amount < requiredAmount)
                        {
                            allMaterialsOnStock = false;
                            break;
                        }

                        currentUsedMaterialsOnStock.Add(new MaterialOnStock
                        {
                            Material = inputMaterial.Material,
                            Amount = requiredAmount
                        });
                    }

                    if (!allMaterialsOnStock)
                    {
                        // не хватает материалов. выполняем полный пересчёт, с пересчётом производительности в т.ч.
                        productionMaterials.Remove(material);
                        usedMaterialsOnStock.Clear();

                        var infoChanging = new InfoChanging(time, factory.Customer, $"Не хватает ресурсов для производства материала {material.DisplayName}");
                        _game.AddActivity(infoChanging);

                        i = -1;
                        continue;
                    }

                    // все материалы есть. вносим их в список материалов для использования.
                    foreach (var currentMaterialOnStock in currentUsedMaterialsOnStock)
                    {
                        ReferenceData.AddMaterialToStock(usedMaterialsOnStock, currentMaterialOnStock);
                    }
                }

                break;
            }

            // 2.2. осуществляем производство материала и помещение его на склад
            foreach (var material in productionMaterials)
            {
                var producedAmount = material.AmountPerDay * performancePerMaterial * factory.Performance;
                var producedMaterial = new MaterialOnStock { Amount = producedAmount, Material = material };

                // добавляем произведенный материал на склад
                ReferenceData.AddMaterialToStock(factory.Stock, producedMaterial);

                var infoChanging = new InfoChanging(time, factory.Customer, $"Произведен материал {material.DisplayName} в количестве {producedMaterial.Amount}");
                _game.AddActivity(infoChanging);
            }

            // 2.3. Списываем со склада исходные материалы, потраченные на производство этого
            foreach (var usedMaterial in usedMaterialsOnStock)
            {
                var materialOnStock = factory.Stock.First(m => m.Material.Id == usedMaterial.Material.Id);

                materialOnStock.Amount -= usedMaterial.Amount;

                if (materialOnStock.Amount == 0)
                {
                    factory.Stock.Remove(materialOnStock);
                }
            }

            // 3. списываем ФОТ. Cписываем деньги со счёта игрока
            var totalSalary = factory.Workers * ReferenceData.CalculateWorkerSalary(factory);

            _game.AddActivity(new FinanceFactoryChange(_game.Time, factory, 0, 0, -totalSalary, $"Выплата зарплаты на фабрике {factory.DisplayName}, рабочих {factory.Workers}, общая сумма {totalSalary:C}"));
        }

        private void ProcessGameToCustomerContract(Contract contract)
        {
            var factory = contract.DestinationFactory;
            var material = contract.MaterialWithPrice.Material;
            var materialOnStock = factory.Stock.FirstOrDefault(m => m.Material.Id == material.Id);

            if (materialOnStock == null)
            {
                materialOnStock = new MaterialOnStock { Material = material };
                factory.Stock.Add(materialOnStock);
            }

            var amount = Convert.ToInt32(contract.MaterialWithPrice.Amount);

            var totalPrice = ReferenceData.Supply.Materials.First(m => m.Material.Id == material.Id).SellPrice * amount;
            if (totalPrice > contract.Customer.Sum)
            {
                var infoChange = new InfoChanging(_game.Time, contract.Customer,
                    $"Нет возможности закупить материалы у игры, т.к. не хватает денег ({totalPrice:C} > {contract.Customer.Sum:C}).");
                _game.AddActivity(infoChange);
                return;
            }

            // начисляем материал на склад
            materialOnStock.Amount += amount;

            var time = new GameTime();

            // списываем деньги со счёта игрока и обновляем счётчики контракта
            _game.AddActivity(new FinanceContractChange(time, contract, -totalPrice, 0, amount,
                $"Оплата товара (игре) {contract.MaterialWithPrice.Material.DisplayName}, в количестве {amount}, на сумму {totalPrice:C}"));

            // добавляем активность по поставке материала
            var materialLogistic = new MaterialLogistic(time, contract.MaterialWithPrice)
            {
                SourceFactory = contract.SourceFactory,
                DestinationFactory = contract.DestinationFactory
            };
            _game.AddActivity(materialLogistic);
        }

        /// <summary>
        /// Выполнение заключенных контрактов.
        /// </summary>
        /// <param name="contract">Контракт.</param>
        protected void ProcessContract(Contract contract)
        {
            if (null == contract.SourceFactory || null == contract.MaterialWithPrice)
            {
                return;
            }

            var time = new GameTime();

            if (contract.TillDate.HasValue && contract.TillDate < _game.TotalGameDays)
            {
                // закрываем контракт по истечению срока действия
                _game.AddActivity(new CustomerCloseContractChange(time, contract,
                    "Закрытие контракта по истечению срока действия"));

                return;
            }

            if (contract.TillCount.HasValue && contract.TillCount >= contract.TotalCountCompleted)
            {
                // закрываем контракт по достижению поставленного количества
                _game.AddActivity(new CustomerCloseContractChange(time, contract,
                    "Закрытие контракта по выполнению обязательств по поставленному количеству"));

                return;
            }

            // определяем, покупается материал (т.е. зачисляется на указанную фабрику) или продается игре (то тогда берем исходную фабрику)
            var factory = contract.SourceFactory ?? contract.DestinationFactory;
            var material = contract.MaterialWithPrice.Material;
            var materialOnStock = factory.Stock.FirstOrDefault(m => m.Material.Id == material.Id) ??
                                  new MaterialOnStock {Material = material};

            MaterialLogistic materialLogistic;

            int amount;
            decimal totalPrice;

            // идёт поставка игре или другой команде
            var isGameDemand = null == contract.DestinationFactory;
            var needAmount = 0;

            var sellPrice = isGameDemand
                ? ReferenceData.Demand.Materials.First(m => m.Material.Id == material.Id).SellPrice
                : contract.MaterialWithPrice.SellPrice;

            amount = Convert.ToInt32(contract.MaterialWithPrice.Amount);
            if (contract.TillCount.HasValue && contract.TotalCountCompleted + amount > contract.TillCount)
            {
                amount = contract.TillCount.Value - contract.TotalCountCompleted;
            }

            contract.TotalCountCompleted += amount;

            if (materialOnStock.Amount < amount)
            {
                needAmount = amount - Convert.ToInt32(materialOnStock.Amount);
                amount = Convert.ToInt32(materialOnStock.Amount);
            }

            // общая сумма
            totalPrice = sellPrice * amount;

            // сумма налога
            var tax = ReferenceData.CalculateTaxOnMaterial(material);
            var taxSum = totalPrice * tax;
            var taxChange = new TaxFactoryChange(time, contract.SourceFactory, taxSum);

            // чистая сумма
            var netSum = totalPrice - taxSum;

            // списываем материал со склада
            materialOnStock.Amount -= amount;

            // начисляем деньги на счёт игрока
            _game.AddActivity(new FinanceContractChange(time, contract, netSum, -taxSum, amount,
                $"Продажа товара ({(isGameDemand ? "игре" : $"команде {contract.DestinationFactory.Customer.DisplayName}")}) {contract.MaterialWithPrice.Material.DisplayName}, в количестве {amount}, на сумму {totalPrice:C}, из них налог на сумму {taxSum:C}"));

            // добавляем активность о снятом налоге
            _game.AddActivity(taxChange);

            if (!isGameDemand)
            {
                // начисляем материал на склад другой фабрики
                ReferenceData.AddMaterialToStock(contract.DestinationFactory.Stock,
                    new MaterialOnStock {Material = material, Amount = amount});

                // списываем деньги со счёта игрока другой фабрики
                _game.AddActivity(new FinanceCustomerChange(time, contract.DestinationFactory.Customer, -totalPrice, $"Оплата товара (команде {contract.SourceFactory.Customer.DisplayName}) {contract.MaterialWithPrice.Material.DisplayName}, в количестве {amount}, на сумму {totalPrice:C}"));
            }

            // добавляем активность по поставке материала
            materialLogistic = new MaterialLogistic(time,
                new MaterialWithPrice {Amount = amount, Material = material, SellPrice = sellPrice})
            {
                Tax = taxChange,
                SourceFactory = contract.SourceFactory,
                DestinationFactory = contract.DestinationFactory
            };
            _game.AddActivity(materialLogistic);

            if (!isGameDemand)
            {
                if (contract.SrcInsurancePremium > 0)
                {
                    // снимаем деньги за страховку, с исходной команды
                    _game.AddActivity(new FinanceCustomerInsurancePremiumChange(time, contract.SourceFactory.Customer,
                        -contract.SrcInsurancePremium, $"Оплата страховки в размере {contract.SrcInsurancePremium:C}"));
                }

                if (contract.DestInsurancePremium > 0)
                {
                    // снимаем деньги за страховку, с команды получателя
                    _game.AddActivity(new FinanceCustomerInsurancePremiumChange(time, contract.DestinationFactory.Customer,
                        -contract.DestInsurancePremium,
                        $"Оплата страховки в размере {contract.DestInsurancePremium:C}"));
                }

                if (needAmount > 0)
                {
                    // возникли условия не поставки товара, применяем штрафные санкции, если они есть
                    var fine = contract.Fine;
                    var srcInsuranceSum = 0m;
                    var destInsuranceSum = 0m;

                    if (contract.DestInsurancePremium > 0)
                    {
                        destInsuranceSum = contract.DestInsuranceAmount;
                    }

                    if (contract.SrcInsuranceAmount > 0)
                    {
                        srcInsuranceSum = contract.SrcInsuranceAmount;
                    }

                    // производим изменения на счету команды получателя (за счёт команды поставщика)
                    if (fine > 0)
                    {
                        var totalFine = fine * needAmount;

                        _game.AddActivity(new FinanceCustomerFineChange(time, contract.SourceFactory.Customer,
                            -totalFine, $"Оплата штрафа в размере {totalFine:C} ({fine} * {needAmount})"));

                        _game.AddActivity(new FinanceCustomerFineChange(time, contract.DestinationFactory.Customer,
                            totalFine, $"Получение выплат по штрафу в размере {totalFine:C} ({fine} * {needAmount})"));
                    }

                    if (srcInsuranceSum > 0)
                    {
                        _game.AddActivity(new FinanceCustomerInsurancePaymentChange(time, contract.SourceFactory.Customer,
                            srcInsuranceSum,
                            $"Выплата страховки по непоставке товара, в сумме {srcInsuranceSum:C}"));
                    }

                    if (destInsuranceSum > 0)
                    {
                        _game.AddActivity(new FinanceCustomerInsurancePaymentChange(time, contract.DestinationFactory.Customer,
                            destInsuranceSum,
                            $"Выплата страховки по непоставке товара, в сумме {destInsuranceSum:C}"));
                    }
                }
            }
        }
    }
}
