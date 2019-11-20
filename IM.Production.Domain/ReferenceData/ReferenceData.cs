using System;
using System.Collections.Generic;
using System.Linq;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain.ReferenceData
{
    /// <summary>
    /// Класс, хранящий справочные данные и расчитывающий разные показатели
    /// </summary>
    public static class ReferenceData
    {
        /// <summary>
        /// Области производства.
        /// </summary>
        public static List<ProductionType> ProductionTypes { get; private set; }

        /// <summary>
        /// Общий список материалов в игре.
        /// </summary>
        public static List<Material> Materials { get; private set; }

        /// <summary>
        /// Какие материалы игра поставляет изначально.
        /// </summary>
        public static GameSupply Supply { get; private set; }

        /// <summary>
        /// Какие ресурсы игра покупает и по какой цене.
        /// </summary>
        public static GameDemand Demand { get; private set; }

        /// <summary>
        /// Определения фабрик, которые есть в игре.
        /// </summary>
        public static List<FactoryDefinition> FactoryDefinitions { get; private set; }

        /// <summary>
        /// Налог на продажу материалов.
        /// </summary>
        public static readonly List<TaxOnMaterial> MaterialTaxes = new List<TaxOnMaterial>();

        /// <summary>
        /// Налог на фабрики.
        /// </summary>
        public static readonly List<TaxOnFactory> FactoryTaxes = new List<TaxOnFactory>();

        /// <summary>
        /// Справочные данные по производительности фабрики, если количество рабочих больше чем нужно.
        /// </summary>
        /// <remarks>Key - насколько рабочих больше чем нужно; Value - на сколько измениться общая производительность фабрики.</remarks>
        public static readonly List<KeyValuePair<decimal, decimal>> FactoryOverPerformance = new List<KeyValuePair<decimal, decimal>>
        {
            new KeyValuePair<decimal, decimal>(1.1m, 1.1m),
            new KeyValuePair<decimal, decimal>(1.2m, 1.15m),
            new KeyValuePair<decimal, decimal>(1.3m, 1.2m),
            new KeyValuePair<decimal, decimal>(1.4m, 1.25m),
            new KeyValuePair<decimal, decimal>(1.5m, 1.4m),
            new KeyValuePair<decimal, decimal>(1.6m, 1.45m),
            new KeyValuePair<decimal, decimal>(1.8m, 1.5m),
            new KeyValuePair<decimal, decimal>(2m, 1.55m)
        };

        /// <summary>
        /// Стоимость фабрики по умолчанию.
        /// </summary>
        /// <remarks>Key - уровень поколения фабрики, Value - стоимость фабрики.</remarks>
        public static readonly List<KeyValuePair<int, decimal>> DefaultFactoryCost =
            new List<KeyValuePair<int, decimal>>
            {
                new KeyValuePair<int, decimal>(1, 10000),
                new KeyValuePair<int, decimal>(2, 25000),
                new KeyValuePair<int, decimal>(3, 50000),
                new KeyValuePair<int, decimal>(4, 75000),
                new KeyValuePair<int, decimal>(5, 110000),
                new KeyValuePair<int, decimal>(6, 150000),
                new KeyValuePair<int, decimal>(7, 200000),
                new KeyValuePair<int, decimal>(8, 300000),
                new KeyValuePair<int, decimal>(9, 500000),
                new KeyValuePair<int, decimal>(10, 1000000)
            };

        /// <summary>
        /// Стоимость конкретного типа фабрики.
        /// </summary>
        /// <remarks>Key - ID определения фабрики, Value - стоимость фабрики.</remarks>
        public static readonly List<KeyValuePair<Guid, decimal>> FactoryCost = new List<KeyValuePair<Guid, decimal>>();

        /// <summary>
        /// Стоимость исследования следующего поколения фабрики.
        /// </summary>
        /// <remarks>По умолчанию первое поколение уже исследовано. Key - уровень поколения, Value - стоимость исследования.</remarks>
        public static readonly List<KeyValuePair<int, decimal>> GenerationFactoryRDCost = new List<KeyValuePair<int, decimal>>
        {
            new KeyValuePair<int, decimal>(2, 5000),
            new KeyValuePair<int, decimal>(3, 15000),
            new KeyValuePair<int, decimal>(4, 25000),
            new KeyValuePair<int, decimal>(5, 40000),
            new KeyValuePair<int, decimal>(6, 60000),
            new KeyValuePair<int, decimal>(7, 80000),
            new KeyValuePair<int, decimal>(8, 120000),
            new KeyValuePair<int, decimal>(9, 200000),
            new KeyValuePair<int, decimal>(10, 400000)
        };

        /// <summary>
        /// Справочные данные по прокачке уровня на конкретной фабрике
        /// </summary>
        /// <remarks>По умолчанию первый уровень фабрики уже исследован. Key - уровень поколения, Value - процент от стоимости фабрики.</remarks>
        /// <remarks>Т.е. указано - 5, 1.5. Это означает что что бы исследовать фабирку 5 уровня (находясь на 4), надо будет потратить 1.5 цены стоимости этой фабрики</remarks>
        public static readonly List<KeyValuePair<int, decimal>> FactoryLevelUpRDCost =
            new List<KeyValuePair<int, decimal>>
            {
                new KeyValuePair<int, decimal>(2, 0.33m),
                new KeyValuePair<int, decimal>(3, 0.75m),
                new KeyValuePair<int, decimal>(4, 1m),
                new KeyValuePair<int, decimal>(5, 1.5m),
            };

        /// <summary>
        /// Базовая зарплата одного рабочего на фабрике.
        /// </summary>
        public static decimal BaseWorkerSalay = 100m;

        /// <summary>
        /// Налог на фабрику по умолчанию.
        /// </summary>
        public static decimal DefaultFactoryTax = 0.1m;

        /// <summary>
        /// Налог на продажу единицы материала по умолчанию.
        /// </summary>
        public static decimal DefaultMaterialTax = 0.01m;

        /// <summary>
        /// Расчёт общей стоимости для исследования фабрик следующего поколения.
        /// </summary>
        /// <param name="customer">Команда.</param>
        /// <returns>Стоимость исследования фабрик следующего поколения.</returns>
        public static decimal CalculateRDSummToNextGenerationLevel(Customer customer)
        {
            var currentGenerationLevel = customer.FactoryGenerationLevel;
            var cost = GenerationFactoryRDCost.FirstOrDefault(c => c.Key == currentGenerationLevel + 1);
            return cost.Value;
        }

        /// <summary>
        /// Расчёт общей стоимости для исследования следующего уровня производительности фабрики.
        /// </summary>
        /// <param name="factory">Фабрика.</param>
        /// <returns>Стоимость исследования следующего уровня производительности фабрики.</returns>
        public static decimal CalculateRDSummToNextFactoryLevelUp(Factory factory)
        {
            var currentFactoryLevel = factory.Level;
            var kv = FactoryLevelUpRDCost.FirstOrDefault(c => c.Key == currentFactoryLevel + 1);
            if (kv.Key == 0)
            {
                kv = FactoryLevelUpRDCost.Last();
            }

            return CalculateFactoryCost(factory.FactoryDefinition) * kv.Value;
        }

        /// <summary>
        /// Расчёт стоимости фабрики, на основе её описания.
        /// </summary>
        /// <param name="factoryDefinition">Описание фабрики.</param>
        /// <returns>Стоимость фабрики.</returns>
        public static decimal CalculateFactoryCost(FactoryDefinition factoryDefinition)
        {
            decimal cost;
            var costData = FactoryCost.FirstOrDefault(f => f.Key == factoryDefinition.Id);
            if (costData.Key == Guid.Empty)
            {
                // считаем что данные по уровням упорядочены уже в определении
                cost = DefaultFactoryCost.First().Value;
                foreach (var kv in DefaultFactoryCost)
                {
                    if (kv.Key >= factoryDefinition.GenerationLevel)
                    {
                        cost = kv.Value;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                cost = costData.Value;
            }

            return cost;
        }

        /// <summary>
        /// Расчёт налога на фабрику.
        /// </summary>
        /// <param name="factory">Фабрика.</param>
        /// <returns>Сумма налога.</returns>
        public static decimal CalculateTaxOnFactory(Factory factory)
        {
            var tax = FactoryTaxes.FirstOrDefault(f => f.FactoryDefinition.Id == factory.FactoryDefinition.Id)?.Percent ?? DefaultFactoryTax;
            return tax;
        }

        /// <summary>
        /// Расчёт налога на продажу единицы материала.
        /// </summary>
        /// <param name="material">Материал.</param>
        /// <returns>Сумма налога.</returns>
        public static decimal CalculateTaxOnMaterial(Material material)
        {
            var tax = MaterialTaxes.FirstOrDefault(m => m.Material.Id == material.Id)?.Percent ?? DefaultMaterialTax;
            return tax;
        }

        public static void CalculateMaterialInDetails(Material material, out decimal price, out List<MaterialWithPrice> sourceMaterials, decimal amountPerDay = 1)
        {
            price = 0;
            sourceMaterials = new List<MaterialWithPrice>();

            var supplyMaterial = Supply.Materials.FirstOrDefault(m => m.Material.Id == material.Id);
            if (supplyMaterial != null)
            {
                // если материал поставляет игра, то стоимость за единицу материала составляет цену, по которой игра его продаёт * запрашиваемое количество
                price = supplyMaterial.SellPrice * amountPerDay;
                return;
            }

            foreach (var inputMaterial in material.InputMaterials)
            {
                var forOne = new MaterialOnStock
                {
                    Amount = inputMaterial.Amount / material.AmountPerDay * amountPerDay,
                    Material = inputMaterial.Material
                };

                CalculateMaterialInDetails(forOne.Material, out var subPrice, out var subSourceMaterials, forOne.Amount);

                AddMaterialToStock(sourceMaterials,
                    new MaterialWithPrice
                    {
                        Amount = forOne.Amount,
                        Material = forOne.Material,
                        SellPrice = subPrice / forOne.Amount
                    });

                price += subPrice;

                foreach (var subSourceMaterial in subSourceMaterials)
                {
                    AddMaterialToStock(sourceMaterials, subSourceMaterial);
                }
            }

            //price = price / material.AmountPerDay * amountPerDay;
        }

        /// <summary>
        /// Расчёт себестоимости материала.
        /// </summary>
        /// <param name="material">Материал.</param>
        /// <returns>Себестоимость его производства.</returns>
        public static decimal CalculateMaterialCostPrice(Material material)
        {
            var supplyMaterial = Supply.Materials.FirstOrDefault(m => m.Material.Id == material.Id);
            if (supplyMaterial != null)
            {
                // если материал поставляет игра, то стоимость за единицу материала составляет цену, по которой игра его продаёт
                return supplyMaterial.SellPrice;
            }

            // считаем общую стоимость материала - как сумма стоимости входных материалов
            var cost = 0m;
            foreach (var inputMaterial in material.InputMaterials)
            {
                var price = CalculateMaterialCostPrice(inputMaterial.Material);
                cost += price * inputMaterial.Amount;
            }

            // вычисляем стоимость материала за единицу
            var costPerOne = cost / material.AmountPerDay;
            return costPerOne;
        }

        /// <summary>
        /// Производит расчёт цен на материалы, которые покупает игра.
        /// </summary>
        /// <param name="allFactories">Список всех фабрик в игре.</param>
        public static void CalculateDemandPrices(IEnumerable<Factory> allFactories)
        {
            Demand = new GameDemand();
        }

        /// <summary>
        /// Расчёт зарплаты одного рабочего на фабрике.
        /// </summary>
        /// <param name="factory">Фабрика.</param>
        /// <returns>Зарплата рабочего.</returns>
        public static decimal CalculateWorkerSalary(Factory factory)
        {
            // расчитываем как 10% от от уровня фабрики * базовая зарплата
            var salary = (1 + decimal.Divide(1, factory.FactoryDefinition.GenerationLevel)) * BaseWorkerSalay;
            return salary;
        }

        /// <summary>
        /// Расчёт производительности фабрики.
        /// </summary>
        /// <param name="factory">Фабрика.</param>
        public static void CalculateFactoryPerformance(Factory factory)
        {
            var performance = 0m;
            var workers = factory.Workers;
            if (workers > 0)
            {
                var baseWorkers = factory.FactoryDefinition.BaseWorkers;

                // Если количество сотрудников меньше чем требуемое, то зависимость линейная.
                // т.е. скажем если нужно 10 сотрудников на фабрике, а работают 3 - то производительность = 30%
                performance = decimal.Divide(workers, baseWorkers);
                if (workers > baseWorkers)
                {
                    // если же сотрудников больше, то зависимость уже не линейная
                    var lastFoundPerformance = 1m;
                    foreach (var kv in FactoryOverPerformance)
                    {
                        if (performance >= kv.Key)
                        {
                            lastFoundPerformance = kv.Value;
                        }
                        else
                        {
                            // считаем что данные упорядочены уже в определении, поэтому далее будут только
                            // большие значения и смысла тратить время CPU нет.
                            break;
                        }
                    }

                    performance = lastFoundPerformance;
                }
            }

            var factoryLevel = factory.Level;
            factory.Performance = performance * factoryLevel;
        }

        public static ProductionType GetProductionTypeByKey(string key)
        {
            return ProductionTypes.First(x => x.Key.ToLower() == key.ToLower());
        }

        public static Material GetMaterialByKey(string key)
        {
            var result = Materials.FirstOrDefault(x => x.Key.ToLower() == key.ToLower());
            if (null == result)
            {
                //throw new InvalidOperationException($"Отсутствует указанный ключ материала: {key}");
            }

            return result;
        }

        private static void InitProductionTypes()
        {
            ProductionTypes = new List<ProductionType>
            {
                new ProductionType {Key = "metall", DisplayName = "Металлургическая промышленность"},
                new ProductionType {Key = "electronic", DisplayName = "Электронная промышленность"},
                new ProductionType {Key = "derevo", DisplayName = "Дерево-обрататывающая промышленность"},
                new ProductionType {Key = "neft_gaz", DisplayName = "Нефте-газо-химическая промышленность"}
            };
        }

        private static void InitMaterials()
        {
            Materials = new List<Material>();

            Materials.Add(new Material
            {
                Key = "metall_ruda",
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Металлосодержащая руда",
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,      // произведем 1 единицу материала "металл железо"
                Key = "metall_zelezo",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_ruda"),
                        Amount = 10000  // и для производство потребуется 10000 единиц материала "металлосодержащая руда"
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Железная руда",
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_med",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_ruda"),
                        Amount = 100000
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Медная руда",
            });

            Materials.Add(new Material
            {
                AmountPerDay = 0.01m,   // здесь играемся цифрами - мы произведем 0.01 материала "золотая руда"
                Key = "metall_zoloto",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_ruda"),
                        Amount = 100000 // но для этого количества (0.01) потребуется столько же руды, сколько и для меди (которая идёт со скоростю 1 единица в день)
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Золотая руда",
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_zelezo_list",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_ruda"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Стальной лист"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_med_list",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_ruda"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Медный лист"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_zoloto_list",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_ruda"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Золотой лист"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_kuzov_auto",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 12
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Кузов для авто"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_gruz_auto",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 40
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Кузов для грузового авто"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_korpus_kpp",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo"),
                        Amount = 10
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Корпус для коробки переключения передач"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_korpus_dvig",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo"),
                        Amount = 30
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Корпус для двигателя"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_korpus_auto_kpp",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo"),
                        Amount = 12
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Корпус для автоматической коробки переключения передач"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_korpus_turbo_dvig",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo"),
                        Amount = 30
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Корпус для турбо-двигателя"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_korpus_gruz_kpp",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo"),
                        Amount = 30
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Корпус для грузовой коробки переключения передач"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_korpus_gruz_dvig",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo"),
                        Amount = 50
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Корпус для грузового двигателя"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_detali_kpp",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 2
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Детали для коробки переключения передач"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_detali_dvig",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Детали для двигателя"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_detali_transmission",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 6
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Детали для трансмиссии"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_detali_auto_kpp",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 3
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Детали для автоматической коробки переключения передач"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_detali_turbo_dvig",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 8
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Детали для турбо-двигателя"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_detali_gruz_kpp",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 4
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Детали для грузовой коробки переключения передач"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_detali_gruz_dvig",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 14
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Детали для грузового двигателя"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_detali_gruz_transmission",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 20
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Детали для грузовой трансмиссии"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_auto_kpp",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_korpus_kpp"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_detali_kpp"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Коробка переключения передач"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_auto_akpp",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_korpus_auto_kpp"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_detali_auto_kpp"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Автоматическая коробка переключения передач"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_auto_dvig",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_korpus_dvig"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_detali_dvig"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Легковой ДВС"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_auto_turbo_dvig",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_korpus_turbo_dvig"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_detali_turbo_dvig"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Легковой турбированный ДВС"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_car",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_auto_kpp"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_auto_dvig"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_detali_transmission"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("neftgaz_auto_salon"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("neftgaz_auto_steklo"),
                        Amount = 6
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("neftgaz_auto_koleso"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Легковой автомобиль (МКПП и ДВС)"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_car_akpp",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_auto_akpp"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_auto_dvig"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_detali_transmission"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("neftgaz_auto_salon"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("neftgaz_auto_steklo"),
                        Amount = 6
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("neftgaz_auto_koleso"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Легковой автомобиль (АКПП и ДВС)"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "metall_car_turbo",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_auto_akpp"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_auto_turbo_dvig"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_detali_transmission"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("neftgaz_auto_salon"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("neftgaz_auto_steklo"),
                        Amount = 6
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("neftgaz_auto_koleso"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Легковой автомобиль (АКПП и турбо ДВС)"
            });
        }

        private static void InitSupply()
        {
            Supply = new GameSupply
            {
                Materials = new List<MaterialWithPrice>
                {
                    new MaterialWithPrice
                    {
                        Material = GetMaterialByKey("metall_ruda"),
                        SellPrice = 0.01m,
                        Amount = 10000
                    }
                }
            };
        }

        private static void InitFactoryDefinition()
        {
            FactoryDefinitions = new List<FactoryDefinition>();

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 10,
                GenerationLevel = 1,
                Name = "Добыча металлической руды",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_zelezo")}},
                    {2, new List<Material> {GetMaterialByKey("metall_med")}},
                    {3, new List<Material> {GetMaterialByKey("metall_zoloto")}}
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 5,
                GenerationLevel = 2,
                Name = "Производство металлических слитков",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_zelezo_list")}},
                    {
                        2,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_med_list"), GetMaterialByKey("metall_zoloto_list")
                        }
                    }
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 25,
                GenerationLevel = 3,
                Name = "Металло-прокатное производство",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_kuzov_auto")}},
                    {2, new List<Material> {GetMaterialByKey("metall_kuzov_gruz_auto")}}
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 25,
                GenerationLevel = 3,
                Name = "Литейное производство",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {
                        1,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_korpus_kpp"), GetMaterialByKey("metall_korpus_dvig")
                        }
                    },
                    {
                        2,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_korpus_auto_kpp"),
                            GetMaterialByKey("metall_korpus_turbo_dvig")
                        }
                    },
                    {
                        3,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_korpus_gruz_kpp"),
                            GetMaterialByKey("metall_korpus_gruz_dvig")
                        }
                    }
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 30,
                GenerationLevel = 3,
                Name = "Фрезерное производство",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {
                        1,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_detali_kpp"),
                            GetMaterialByKey("metall_detali_dvig"),
                            GetMaterialByKey("metall_detali_transmission")
                        }
                    },
                    {
                        2,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_detali_auto_kpp"),
                            GetMaterialByKey("metall_detali_turbo_dvig")
                        }
                    },
                    {
                        3,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_detali_gruz_kpp"),
                            GetMaterialByKey("metall_detali_gruz_dvig"),
                            GetMaterialByKey("metall_detali_gruz_transmission")
                        }
                    }
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 30,
                GenerationLevel = 4,
                Name = "Сборочное производство",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {
                        1,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_auto_kpp"),
                            GetMaterialByKey("metall_auto_akpp"),
                            GetMaterialByKey("metall_auto_dvig"),
                            GetMaterialByKey("metall_auto_turbo_dvig")
                        }
                    }
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 50,
                GenerationLevel = 4,
                Name = "Автомобильное производство",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_car")}},
                    {
                        2,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_car_akpp"), GetMaterialByKey("metall_car_turbo")
                        }
                    }
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 80,
                GenerationLevel = 5,
                Name = "Грузовое производство",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_gruz_auto")}}
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 40,
                GenerationLevel = 5,
                Name = "Ж/д промышленность",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_poezd")}}
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 100,
                GenerationLevel = 6,
                Name = "Авиа промышленность",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_samolet")}}
                }
            });
        }

        public static void AddMaterialToStock(IList<MaterialWithPrice> materials, MaterialWithPrice materialToAdd)
        {
            var material = materials.FirstOrDefault(m => m.Material.Id == materialToAdd.Material.Id);
            if (null == material)
            {
                materials.Add(materialToAdd);
            }
            else
            {
                material.Amount += materialToAdd.Amount;
            }
        }

        public static void AddMaterialToStock(IList<MaterialOnStock> materials, MaterialOnStock materialToAdd)
        {
            var material = materials.FirstOrDefault(m => m.Material.Id == materialToAdd.Material.Id);
            if (null == material)
            {
                materials.Add(materialToAdd);
            }
            else
            {
                material.Amount += materialToAdd.Amount;
            }
        }

        private static void InitReferences()
        {
            InitProductionTypes();
            InitMaterials();
            InitSupply();
            InitFactoryDefinition();
        }

        static ReferenceData()
        {
            try
            {
                InitReferences();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}