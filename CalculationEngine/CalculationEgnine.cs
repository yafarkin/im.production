using Epam.ImitationGames.Production.Common;
using Epam.ImitationGames.Production.Common.Bank;
using Epam.ImitationGames.Production.Common.Base;
using Epam.ImitationGames.Production.Common.Production;
using Epam.ImitationGames.Production.Common.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using Epam.ImitationGames.Production.Common.ReferenceData;

namespace CalculationEngine
{
    public class CalculationEgnine
    {
        public Game Game { get; set; }

        public void Calculate()
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
            foreach (var customer in Game.Customers)
            {
                var gameSupplyContracts = customer.Contracts.Where(c => c.SourceFactory == null);
                foreach (var contract in gameSupplyContracts)
                {
                    ProcessContract(contract);
                }
            }

            // п. 2. Выплачиваем суммы за исследования на следующее поколение мероприятий
            foreach (var customer in Game.Customers)
            {
                ProcessCustomerRD(customer);
            }

            // п. 3. Выплачиваем суммы за модернизацию предприятия и осуществляем его модернизацию
            foreach (var customer in Game.Customers)
            {
                foreach (var factory in customer.Factories)
                {
                    ProcessFactoryRD(factory, customer);
                }
            }

            // п. 4. Выплачиваем налоги на фабрики
            foreach (var customer in Game.Customers)
            {
                foreach (var factory in customer.Factories)
                {
                    PaidTaxes(factory, customer);
                }
            }

            // п.5. Выполняем расчёт цен, по которым игра покупает материалы
            var allFactories = Game.Customers.SelectMany(c => c.Factories);
            ReferenceData.CalculateDemandPrices(allFactories);

            // п.6. Осуществление производства
            var maxFactoryLevel = 1;
            var currentFactoryLevel = 1;
            while (true)
            {
                foreach (var customer in Game.Customers)
                {
                    var maxCustomerLevel = customer.Factories.Max(f => f.FactoryDefinition.GenerationLevel);
                    if (maxCustomerLevel > maxFactoryLevel)
                    {
                        maxFactoryLevel = maxCustomerLevel;
                    }

                    var factories = customer.Factories.Where(f => f.FactoryDefinition.GenerationLevel == currentFactoryLevel).ToList();
                    foreach (var factory in factories)
                    {
                        // п. 6.1. Выполняем производство согласно настройкам фабрики и помещаем на склад.
                        ProduceFactory(factory);

                        // п. 6.2. Осуществляем передачу материалов согласно контрактам этой фабрики (другим игрокам или игре), включая выплаты штрафов, страховым премий, получения страховок
                        var contracts = customer.Contracts.Where(c => c.SourceFactory.Id == factory.Id);
                        foreach (var contract in contracts)
                        {
                            ProcessContract(contract);
                        }
                    }
                }

                currentFactoryLevel++;
                if (currentFactoryLevel > maxFactoryLevel)
                {
                    break;
                }
            }

            // п.7. Осуществляем поставки по контрактам от игры на фабрику
            foreach (var customer in Game.Customers)
            {
                var gameDemandContracts = customer.Contracts.Where(c => c.DestinationFactory == null);
                foreach (var contract in gameDemandContracts)
                {
                    ProcessContract(contract);
                }
            }

            // п. 8. Осуществляем банковские операции
            // п. 8.1. Получаем проценты на счёт от банка по вкладам
            // п. 8.2. Выплачиваем проценты и тело кредита банку
            foreach (var customer in Game.Customers)
            {
                DepositProcessing(customer);
            }

            // п. 9. В последний игровой день автоматически закрываем все кредиты и вклады в банках.
            if (Game.Time.Day == Game.TotalGameDays)
            {
                throw new NotImplementedException();
            }

            // п. 10. Прибавляем счётчик игровых дней.
            Game.Time.Day++;
            Game.Time.When = DateTime.UtcNow;
        }

        /// <summary>
        /// Обновление данных текущей игры для упрощенного создания объектов.
        /// </summary>
        protected void UpdateCurrentGameProperties()
        {
            CurrentGameProps.GameDay = Game.Time.Day;
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
                    var debitAction = new BankFinAction(time, customer, "Начисление процентов по вкладу")
                    {
                        FinOperation = debit,
                        Sum = 0,
                        PercentSum = debit.Sum / 100 * debit.Percent,
                        Fine = 0
                    };

                    customer.BankFinanceActions.Add(debitAction);

                    debit.Sum += debitAction.TotalSum;

                    // Выплачиваем деньги игроку, если срок вклада подошел к концу и закрываем вклад.
                    if (debit.Time.Day + debit.Days == Game.Time.Day)
                    {
                        var closeDebitOperation = new BankCloseFinOperation(time, customer)
                        {
                            SourceOperation = debit,
                            Sum = debit.Sum,
                            Percent = debit.Percent,
                            Days = debit.Days
                        };

                        customer.BankFinanceOperations.Add(closeDebitOperation);

                        debit.Status = OperationStatus.Close;

                        var closeDebitAction = new BankFinAction(time, customer)
                        {
                            FinOperation = debit,
                            Sum = closeDebitOperation.Sum,
                            PercentSum = 0,
                            Fine = 0
                        };

                        customer.BankFinanceActions.Add(closeDebitAction);
                        customer.Sum += closeDebitAction.TotalSum;

                        var customerChange =
                            new CustomerChange(time, customer,
                                $"Закрытие вклада, начисление {closeDebitAction.TotalSum}")
                            {
                                SumChange = closeDebitAction.TotalSum
                            };
                        Game.AddActivity(customerChange);
                    }
                }
            }
        }

        protected void ProcessCustomerRD(Customer customer)
        {
            if (0 == customer.FactoryGenerationLevel)
            {
                customer.FactoryGenerationLevel = 1;
            }

            if (0 == customer.NeedSumToNextGenerationLevel)
            {
                customer.NeedSumToNextGenerationLevel = ReferenceData.CalculateRDSummToNextGenerationLevel(customer);
            }

            if (customer.SumOnRD <= 0)
            {
                return;
            }

            if (customer.FactoryGenerationLevel >= ReferenceData.GenerationFactoryRDCost.Max(kv => kv.Key))
            {
                return;
            }

            var time = new GameTime();
            var rdSum = customer.SumOnRD;

            // списываем сумму на R&D со счёта
            customer.Sum -= rdSum;
            customer.SpentSumToNextGenerationLevel += rdSum;

            // добавляем активность по изменению состояния команды
            var customerChange = new CustomerChange(time, customer, $"Оплата исследований следующего поколения фабрик, сумма {rdSum};")
            {
                SumChange = -rdSum
            };
            Game.AddActivity(customerChange);
            customerChange = new CustomerChange(time, customer, $"Процент исследования следующего поколения фабрик: {customer.RDProgress:P}")
            {
                RDProgressChange = customerChange.RDProgressChange
            };
            Game.AddActivity(customerChange);

            if (customer.SpentSumToNextGenerationLevel > customer.NeedSumToNextGenerationLevel)
            {
                // достигли следующего уровня
                customer.FactoryGenerationLevel++;
                customer.SpentSumToNextGenerationLevel -= customer.NeedSumToNextGenerationLevel;
                customer.NeedSumToNextGenerationLevel = ReferenceData.CalculateRDSummToNextGenerationLevel(customer);

                // добавляем активность по изменению уровня доступных фабрик
                customerChange = new CustomerChange(time, customer, $"Исследован новый уровень поколения фабрик: {customer.FactoryGenerationLevel}")
                {
                    FactoryGenerationLevelChange = customer.FactoryGenerationLevel
                };
                Game.AddActivity(customerChange);

                customerChange = new CustomerChange(time, customer, $"Процент исследования следующего поколения фабрик: {customer.RDProgress:P}")
                {
                    RDProgressChange = customerChange.RDProgressChange
                };
                Game.AddActivity(customerChange);
            }
        }

        protected void ProcessFactoryRD(Factory factory, Customer customer)
        {
            if (0 == factory.Level)
            {
                factory.Level = 1;
            }

            if (0 == factory.NeedSumToNextLevelUp)
            {
                factory.NeedSumToNextLevelUp = ReferenceData.CalculateRDSummToNextFactoryLevelUp(factory);
            }

            if (factory.SumOnRD <= 0)
            {
                return;
            }

            if (factory.Level >= ReferenceData.FactoryLevelUpRDCost.Max(kv => kv.Key))
            {
                return;
            }

            var time = new GameTime();
            var rdSum = factory.SumOnRD;

            // списываем сумму на R&D со счёта
            customer.Sum -= rdSum;
            factory.SpentSumToNextLevelUp += rdSum;

            // добавляем активность по изменению суммы на счету игрока
            var customerChange = new CustomerChange(time, customer, $"Оплата исследований на фабрике {factory.DisplayName} ({factory.FactoryDefinition.ProductionType.DisplayName}), сумма {rdSum}")
            {
                SumChange = -rdSum
            };
            Game.AddActivity(customerChange);

            var factoryChange = new FactoryChange(time, factory) {RDProgressChange = factory.RDProgress};
            Game.AddActivity(factoryChange);

            if (factory.SpentSumToNextLevelUp >= factory.NeedSumToNextLevelUp)
            {
                // достигли следующего уровня производительности фабрики
                factory.Level++;
                factory.SpentSumToNextLevelUp -= factory.NeedSumToNextLevelUp;
                factory.NeedSumToNextLevelUp = ReferenceData.CalculateRDSummToNextFactoryLevelUp(factory);

                // добавляем активность по изменению уровня производительности фабрики
                factoryChange = new FactoryChange(time, factory) {LevelChange = factory.Level};
                Game.AddActivity(factoryChange);
            }
        }

        protected void PaidTaxes(Factory factory, Customer customer)
        {
            var time = new GameTime();
            var tax = ReferenceData.CalculateTaxOnFactory(factory);
            var taxSum = customer.Sum * tax;
            var taxChange = new Tax(time, factory) {Sum = taxSum};

            // списываем налог со счёта
            customer.Sum -= taxSum;

            // добавляем активность по изменению суммы на счету игрока
            var customerChange = new CustomerChange(time, customer, $"Оплата налога на фабрику {factory.DisplayName} ({factory.FactoryDefinition.ProductionType.DisplayName}), % налога {tax}, сумма налога {taxSum}")
            {
                SumChange = -taxSum
            };
            Game.AddActivity(customerChange);

            // добавляем активность о снятом налоге
            Game.AddActivity(taxChange);
        }

        protected void ProduceFactory(Factory factory)
        {
            var time = new GameTime();

            // 1. определяем уровень производительности фабрики
            ReferenceData.CalculateFactoryPerformance(factory);

            // 2. выполняем производство материалов и помещаем их на склад

            // 2.1. определяем, что вообще сможем произвести
            var willProductionMaterials = factory.ProductionMaterials;
            var usedMaterialsOnStock = new List<MaterialOnStock>();
            decimal performancePerMaterial = 0;
            while (willProductionMaterials.Any())
            {
                performancePerMaterial = decimal.Divide(1, willProductionMaterials.Count);
                foreach (var material in willProductionMaterials)
                {
                    var currentUsedMaterialsOnStock = new List<MaterialOnStock>();
                    // смотрим - есть ли все необходимые исходные материалы на складе
                    var allMaterialsOnStock = true;
                    foreach (var inputMaterial in material.InputMaterials)
                    {
                        var inputAmount = inputMaterial.Amount * factory.Performance * performancePerMaterial;
                        var materialOnStock = factory.Stock.FirstOrDefault(m => m.Id == inputMaterial.Id);
                        if (null == materialOnStock || materialOnStock.Amount < inputAmount)
                        {
                            allMaterialsOnStock = false;
                            break;
                        }

                        currentUsedMaterialsOnStock.Add(new MaterialOnStock
                        {
                            Material = inputMaterial.Material, Amount = inputAmount
                        });
                    }

                    if (!allMaterialsOnStock)
                    {
                        // не хватает материалов. выполняем полный пересчёт, с пересчётом производительности в т.ч.
                        willProductionMaterials.Remove(material);
                        usedMaterialsOnStock.Clear();

                        var infoChanging = new InfoChanging(time, factory.Customer, $"Не хватает ресурсов для производства материала {material.DisplayName}");
                        Game.AddActivity(infoChanging);

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
            foreach (var material in willProductionMaterials)
            {
                var producedAmount = material.AmountPerDay * performancePerMaterial * factory.Performance;
                var producedMaterial = new MaterialOnStock {Amount = producedAmount, Material = material};

                // добавляем произведенный материал на склад
                ReferenceData.AddMaterialToStock(factory.Stock, producedMaterial);

                var infoChanging = new InfoChanging(time, factory.Customer, $"Произведен материал {material.DisplayName} в количестве {producedMaterial.Amount}");
                Game.AddActivity(infoChanging);
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

            // 3. списываем ФОТ.
            var totalSalary = factory.Workers * ReferenceData.CalculateWorkerSalary(factory);

            // списываем деньги со счёта игрока
            factory.Customer.Sum -= totalSalary;
            // добавляем активность по изменению суммы на счету игрока
            var customerChange = new CustomerChange(time, factory.Customer, $"Выплата зарплаты на фабрике {factory.DisplayName}, рабочих {factory.Workers}, общая сумма {totalSalary:C}")
            {
                SumChange = -totalSalary
            };
            Game.AddActivity(customerChange);
        }

        /// <summary>
        /// Выполнение заключенных контрактов.
        /// </summary>
        /// <param name="contract">Контракт.</param>
        protected void ProcessContract(Contract contract)
        {
            // определяем, покупается материал (т.е. зачисляется на указанную фабрику) или продается игре (то тогда берем исходную фабрику)
            var time = new GameTime();

            var factory = contract.DestinationFactory ?? contract.SourceFactory;

            var material = contract.MaterialWithPrice.Material;

            var materialOnStock =
                factory.Stock.FirstOrDefault(m => m.Material.Id == material.Id) ??
                new MaterialOnStock { Material = material };

            MaterialLogistic materialLogistic;

            CustomerChange customerChange;

            int amount;
            decimal totalPrice;

            if (null == contract.SourceFactory)
            {
                // поставка идёт от игры
                amount = Convert.ToInt32(contract.MaterialWithPrice.Amount);
                totalPrice = -(ReferenceData.Supply.Materials.First(m => m.Material.Id == material.Id).SellPrice * amount);

                // начисляем материал на склад
                materialOnStock.Amount += amount;

                // списываем деньги со счёта игрока
                contract.DestinationFactory.Customer.Sum += totalPrice;

                // добавляем активность по поставке материала
                materialLogistic =
                    new MaterialLogistic(time, contract.MaterialWithPrice)
                    {
                        SourceFactory = contract.SourceFactory, DestinationFactory = contract.DestinationFactory
                    };
                Game.AddActivity(materialLogistic);

                // добавляем активность по изменению суммы на счету игрока
                customerChange = new CustomerChange(time, contract.DestinationFactory.Customer, $"Оплата товара (игре) {contract.MaterialWithPrice.Material.DisplayName}, в количестве {amount}, на сумму {totalPrice}")
                {
                    SumChange = totalPrice
                };
                Game.AddActivity(customerChange);
            }
            else
            {
                // идёт поставка игре или другой команде
                var isGameDemand = null == contract.DestinationFactory;
                var needAmount = 0;

                var sellPrice = isGameDemand ? ReferenceData.Demand.Materials.First(m => m.Material.Id == material.Id).SellPrice : contract.MaterialWithPrice.SellPrice;

                amount = Convert.ToInt32(contract.MaterialWithPrice.Amount);

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
                var taxChange = new Tax(time, contract.SourceFactory) {Sum = taxSum};

                // чистая сумма
                var netSum = totalPrice - taxSum;

                // списываем материал со склада
                materialOnStock.Amount -= amount;

                // начисляем деньги на счёт игрока
                contract.SourceFactory.Customer.Sum += netSum;
                // добавляем активность по изменению суммы на счету игрока
                customerChange = new CustomerChange(time, contract.SourceFactory.Customer, $"Продажа товара ({(isGameDemand ? "игре" : $"команде {contract.DestinationFactory.Customer.DisplayName}")}) {contract.MaterialWithPrice.Material.DisplayName}, в количестве {amount}, на сумму {totalPrice}, из них налог на сумму {taxSum}")
                {
                    SumChange = netSum
                };
                Game.AddActivity(customerChange);

                // добавляем активность о снятом налоге
                Game.AddActivity(taxChange);

                if (!isGameDemand)
                {
                    // начисляем материал на склад другой фабрики
                    ReferenceData.AddMaterialToStock(contract.DestinationFactory.Stock, new MaterialOnStock { Material = material, Amount = amount});

                    // списываем деньги со счёта игрока другой фабрики
                    contract.DestinationFactory.Customer.Sum -= totalPrice;

                    // добавляем активность по изменению суммы на счету игрока
                    customerChange = new CustomerChange(time, contract.DestinationFactory.Customer, $"Оплата товара (команде {contract.SourceFactory.Customer.DisplayName}) {contract.MaterialWithPrice.Material.DisplayName}, в количестве {amount}, на сумму {totalPrice}")
                    {
                        SumChange = -totalPrice
                    };
                    Game.AddActivity(customerChange);
                }

                // добавляем активность по поставке материала
                materialLogistic = new MaterialLogistic(time,
                    new MaterialWithPrice {Amount = amount, Material = material, SellPrice = sellPrice})
                {
                    Tax = taxChange,
                    SourceFactory = contract.SourceFactory,
                    DestinationFactory = contract.DestinationFactory
                };
                Game.AddActivity(materialLogistic);

                if (!isGameDemand)
                {
                    if (contract.SrcInsurancePremium > 0)
                    {
                        // снимаем деньги за страховку, с исходной команды
                        contract.SourceFactory.Customer.Sum -= contract.SrcInsurancePremium;

                        // добавляем активность по изменению суммы на счету игрока
                        customerChange = new CustomerChange(time, contract.SourceFactory.Customer, $"Оплата страховки в размере {contract.SrcInsurancePremium}")
                        {
                            SumChange = -contract.SrcInsurancePremium
                        };
                        Game.AddActivity(customerChange);
                    }

                    if (contract.DestInsurancePremium > 0)
                    {
                        // снимаем деньги за страховку, с команды получателя
                        contract.DestinationFactory.Customer.Sum -= contract.DestInsurancePremium;

                        // добавляем активность по изменению суммы на счету игрока
                        customerChange = new CustomerChange(time, contract.DestinationFactory.Customer, $"Оплата страховки в размере {contract.DestInsurancePremium}")
                        {
                            SumChange = -contract.DestInsurancePremium
                        };
                        Game.AddActivity(customerChange);
                    }

                    if (needAmount > 0)
                    {
                        // возникли условия не поставки товара, применяем штрафные санкции, если они есть
                        var fine = contract.Fine;
                        var srcInsuranceAmount = 0m;
                        var destInsuranceAmount = 0m;

                        if (contract.DestInsurancePremium > 0)
                        {
                            destInsuranceAmount = contract.DestInsuranceAmount;
                        }

                        if (contract.SrcInsuranceAmount > 0)
                        {
                            srcInsuranceAmount = contract.SrcInsuranceAmount;
                        }

                        // производим изменения на счету команды получателя (за счёт команды поставщика)
                        if (fine > 0)
                        {
                            var totalFine = fine * needAmount;

                            contract.SourceFactory.Customer.Sum -= totalFine;
                            customerChange = new CustomerChange(time, contract.SourceFactory.Customer, $"Оплата штрафа в размере {totalFine} ({fine} * {needAmount})")
                            {
                                SumChange = -totalFine
                            };
                            Game.AddActivity(customerChange);

                            contract.DestinationFactory.Customer.Sum += totalFine;
                            customerChange = new CustomerChange(time, contract.SourceFactory.Customer, $"Получение выплат по штрафу в размере {totalFine} ({fine} * {needAmount})")
                            {
                                SumChange = totalFine
                            };
                            Game.AddActivity(customerChange);
                        }

                        if (srcInsuranceAmount > 0)
                        {
                            contract.SourceFactory.Customer.Sum += srcInsuranceAmount;
                            customerChange = new CustomerChange(time, contract.SourceFactory.Customer, $"Выплата страховки по непоставке товара, в сумме {srcInsuranceAmount}")
                            {
                                SumChange = srcInsuranceAmount
                            };
                            Game.AddActivity(customerChange);
                        }

                        if (destInsuranceAmount > 0)
                        {
                            contract.DestinationFactory.Customer.Sum += destInsuranceAmount;
                            customerChange = new CustomerChange(time, contract.DestinationFactory.Customer, $"Выплата страховки по непоставке товара, в сумме {destInsuranceAmount}")
                            {
                                SumChange = srcInsuranceAmount
                            };
                            Game.AddActivity(customerChange);
                        }
                    }
                }
            }
        }
    }
}
