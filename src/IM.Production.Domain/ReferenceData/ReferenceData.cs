﻿using System;
using System.Collections.Generic;
using System.Linq;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain.ReferenceData
{
    /// <summary>
    /// Класс, хранящий справочные данные и рассчитывающий разные показатели
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
        /// Начальная сумма на счету команды.
        /// </summary>
        public static readonly decimal InitialCustomerBalance = 100000m;

        /// <summary>
        /// На какое количество дней открывается кредит.
        /// </summary>
        public static readonly uint CreditDaysDefault = 25;

        /// <summary>
        /// Под какой процент (в игровой день) выдается кредит.
        /// </summary>
        public static readonly decimal CreditPercentDefault = 0.1m;

        /// <summary>
        /// На какое количество дней открывается вклад.
        /// </summary>
        public static readonly uint DebitDaysDefault = 50;

        /// <summary>
        /// На какой процент (в игровой день) открывается вклад.
        /// </summary>
        public static readonly decimal DebitPercentDefault = 0.03m;

        /// <summary>
        /// Справочные данные по производительности фабрики, если количество рабочих больше чем нужно.
        /// </summary>
        /// <remarks>Key - насколько рабочих больше чем нужно; Value - на сколько измениться общая производительность фабрики.</remarks>
        public static readonly IDictionary<decimal, decimal> FactoryOverPerformance = new Dictionary<decimal, decimal>
        {
            {1.1m, 1.1m},
            {1.2m, 1.15m},
            {1.3m, 1.2m},
            {1.4m, 1.25m},
            {1.5m, 1.4m},
            {1.6m, 1.45m},
            {1.8m, 1.5m},
            {2m, 1.55m}
        };

        /// <summary>
        /// Стоимость фабрики по умолчанию.
        /// </summary>
        /// <remarks>Key - уровень поколения фабрики, Value - стоимость фабрики.</remarks>
        public static readonly IDictionary<int, decimal> DefaultFactoryCost = new Dictionary<int, decimal>
        {
            {1, 1000},
            {2, 2500},
            {3, 5000},
            {4, 7500},
            {5, 11000},
            {6, 15000},
            {7, 20000},
            {8, 30000},
            {9, 50000},
            {10, 100000}
        };

        /// <summary>
        /// Стоимость конкретного типа фабрики.
        /// </summary>
        /// <remarks>Key - ID определения фабрики, Value - стоимость фабрики.</remarks>
        public static readonly IDictionary<Guid, decimal> FactoryCost = new Dictionary<Guid, decimal>();

        /// <summary>
        /// Стоимость исследования следующего поколения фабрики.
        /// </summary>
        /// <remarks>По умолчанию первое поколение уже исследовано. Key - уровень поколения, Value - стоимость исследования.</remarks>
        public static readonly IDictionary<int, decimal> GenerationFactoryRDCost = new Dictionary<int, decimal>
        {
            {2, 500},
            {3, 1500},
            {4, 2500},
            {5, 4000},
            {6, 6000},
            {7, 8000},
            {8, 12000},
            {9, 20000},
            {10, 40000}
        };

        /// <summary>
        /// Справочные данные по прокачке уровня на конкретной фабрике.
        /// </summary>
        /// <remarks>По умолчанию первый уровень фабрики уже исследован. Key - уровень поколения, Value - процент от стоимости фабрики.</remarks>
        /// <remarks>Т.е. указано - 5, 1.5. Это означает что что бы исследовать фабирку 5 уровня (находясь на 4), надо будет потратить 1.5 цены стоимости этой фабрики</remarks>
        public static readonly IDictionary<int, decimal> FactoryLevelUpRDCost = new Dictionary<int, decimal>
        {
            {2, 0.33m}, {3, 0.75m}, {4, 1m}, {5, 1.5m}
        };

        /// <summary>
        /// Справочные данные, по которой игра покупает фабрику.
        /// </summary>
        public static readonly decimal FactorySellDiscount = 0.75m;

        /// <summary>
        /// Справочные данные надбавки к цене фабрики при продаже, в зависимости от уровня.
        /// </summary>
        /// <remarks>Key - уровень поколения фабрики, Value - процент надбавки при продаже.</remarks>
        public static readonly IDictionary<int, decimal> FactorySellCoeff = new Dictionary<int, decimal>
        {
            {1, 0},
            {2, 20},
            {3, 25},
            {4, 30},
            {5, 40},
            {6, 50},
            {7, 100}
        };

        /// <summary>
        /// Базовая зарплата одного рабочего на фабрике.
        /// </summary>
        public static decimal BaseWorkerSalay = 6m;

        /// <summary>
        /// Налог на фабрику по умолчанию.
        /// </summary>
        public static decimal DefaultFactoryTax = 0.001m;

        /// <summary>
        /// Налог на продажу единицы материала по умолчанию.
        /// </summary>
        public static decimal DefaultMaterialTax = 0.01m;

        private static T FindNearest<T>(int key, IDictionary<int, T> source)
        {
            if (source.ContainsKey(key))
            {
                return source[key];
            }

            var nearKey = 0;
            var value = default(T);
            foreach (var kv in source)
            {
                if (0 == nearKey || (key >= kv.Key && key - kv.Key < key - nearKey))
                {
                    nearKey = kv.Key;
                    value = kv.Value;
                }
            }

            return value;
        }

        private static T FindNearest<T>(decimal key, IDictionary<decimal, T> source)
        {
            if (source.ContainsKey(key))
            {
                return source[key];
            }

            var nearKey = 0m;
            var value = default(T);
            foreach (var kv in source)
            {
                if (0 == nearKey || (key >= kv.Key && key - kv.Key < key - nearKey))
                {
                    nearKey = kv.Key;
                    value = kv.Value;
                }
            }

            return value;
        }

        /// <summary>
        /// Расчёт общей стоимости для исследования фабрик следующего поколения.
        /// </summary>
        /// <param name="customer">Команда.</param>
        /// <returns>Стоимость исследования фабрик следующего поколения.</returns>
        public static decimal CalculateRDSummToNextGenerationLevel(Customer customer, int level = 0)
        {
            var currentGenerationLevel = 0 == level ? customer.FactoryGenerationLevel : level;
            var cost = FindNearest(currentGenerationLevel + 1, GenerationFactoryRDCost);
            return cost;
        }

        /// <summary>
        /// Расчёт общей стоимости для исследования следующего уровня производительности фабрики.
        /// </summary>
        /// <param name="factory">Фабрика.</param>
        /// <returns>Стоимость исследования следующего уровня производительности фабрики.</returns>
        public static decimal CalculateRDSummToNextFactoryLevelUp(Factory factory, int level = 0)
        {
            if (null == factory || null == factory.FactoryDefinition)
            {
                return 0;
            }

            var currentFactoryLevel = 0 == level ? factory.Level : level;
            var cost = FindNearest(currentFactoryLevel + 1, FactoryLevelUpRDCost);
            return CalculateFactoryCostForBuy(factory.FactoryDefinition) * cost;
        }

        /// <summary>
        /// Расчёт стоимости покупки фабрики, на основе её описания.
        /// </summary>
        /// <param name="factoryDefinition">Описание фабрики.</param>
        /// <returns>Стоимость фабрики для покупки.</returns>
        public static decimal CalculateFactoryCostForBuy(FactoryDefinition factoryDefinition)
        {
            var cost = FactoryCost.ContainsKey(factoryDefinition.Id)
                ? FactoryCost[factoryDefinition.Id]
                : FindNearest(factoryDefinition.GenerationLevel, DefaultFactoryCost);

            return cost;
        }

        /// <summary>
        /// Расчёт стоимости продажи фабрики.
        /// </summary>
        /// <param name="factory">Фабрика.</param>
        /// <returns>Стоимость фабрики для продажи.</returns>
        public static decimal CalculateFactoryCostForSell(Factory factory)
        {
            var buyCost = CalculateFactoryCostForBuy(factory.FactoryDefinition);
            var baseCost = buyCost * FactorySellDiscount;
            var byLevelCost = baseCost * (FindNearest(factory.Level, FactorySellCoeff) / 100);
            var cost = baseCost + byLevelCost;
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
            var factoryCost = CalculateFactoryCostForSell(factory);
            var taxSumm = factoryCost * tax;
            return taxSumm;
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

        public static decimal CalculateFactoryExtraChargePercent(Factory factory, IEnumerable<Factory> factories)
        {
            const decimal defaultExtraChargePercent = 0.1M;
            const decimal advancedExtraChargePercent = 0.5M;

            var maxGeneration = factories.Max(f => f.FactoryDefinition.GenerationLevel);
            var isMaxGeneration = factory.FactoryDefinition.GenerationLevel == maxGeneration;

            return isMaxGeneration ? advancedExtraChargePercent : defaultExtraChargePercent;
        }

        // TODO It might become redundant
        public static void UpdateGameDemand(IList<Factory> factories)
        {
            Demand.Materials.Clear();

            foreach (var factory in factories)
            {
                var extraChargePercent = CalculateFactoryExtraChargePercent(factory, factories);

                foreach (var material in factory.ProductionMaterials)
                {
                    var demand = Demand.Materials.FirstOrDefault(m => m.Material.Id == material.Id);

                    if (demand == null)
                    {
                        var costPrice = CalculateMaterialCostPrice(material);
                        var extraCharge = costPrice * extraChargePercent;

                        Demand.Materials.Add(new MaterialWithPrice
                        {
                            Material = material,
                            SellPrice = costPrice + extraCharge
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Расчёт зарплаты одного рабочего на фабрике.
        /// </summary>
        /// <param name="factory">Фабрика.</param>
        /// <returns>Зарплата рабочего.</returns>
        /// <remarks>Рассчитывается, как 10% от поколения фабрики, умноженные на базовую зарплату.</remarks>
        public static decimal CalculateWorkerSalary(Factory factory)
        {
            var decile = 1 + (factory.FactoryDefinition.GenerationLevel - 1) * 0.1m;
            var salary = decile * BaseWorkerSalay;

            return salary;
        }

        /// <summary>
        /// Расчёт производительности фабрики.
        /// </summary>
        /// <param name="factory">Фабрика.</param>
        public static decimal CalculateFactoryPerformance(Factory factory)
        {
            var performance = 0M;
            var workers = factory.Workers;

            if (workers > 0)
            {
                var baseWorkers = factory.FactoryDefinition.BaseWorkers;
                performance = decimal.Divide(workers, baseWorkers);

                // Если количество сотрудников меньше чем требуемое, то зависимость линейная.
                // т.е. скажем если нужно 10 сотрудников на фабрике, а работают 3 - то производительность = 30%
                if (workers > baseWorkers)
                {
                    // если же сотрудников больше, то зависимость уже не линейная
                    performance = FindNearest(performance, FactoryOverPerformance);
                }
            }

            return performance * factory.Level;
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
                DisplayName = "Металлосодержащая руда"
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

        private static void InitDemand() => Demand = new GameDemand();

        private static void InitFactoryDefinition()
        {
            FactoryDefinitions = new List<FactoryDefinition>();

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 3,
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
                    {2, new List<Material>{GetMaterialByKey("metall_med_list"), GetMaterialByKey("metall_zoloto_list")}}
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 5,
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
                BaseWorkers = 7,
                GenerationLevel = 3,
                Name = "Литейное производство",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1,new List<Material>{GetMaterialByKey("metall_korpus_kpp"), GetMaterialByKey("metall_korpus_dvig")}},
                    {2,new List<Material>{GetMaterialByKey("metall_korpus_auto_kpp"),GetMaterialByKey("metall_korpus_turbo_dvig")}},
                    {3,new List<Material>{GetMaterialByKey("metall_korpus_gruz_kpp"),GetMaterialByKey("metall_korpus_gruz_dvig")}}
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 10,
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
                BaseWorkers = 10,
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
                BaseWorkers = 18,
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
                BaseWorkers = 25,
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
                BaseWorkers = 12,
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
                BaseWorkers = 32,
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
            if (material == null)
            {
                materials.Add(materialToAdd);
            }
            else
            {
                material.Amount += materialToAdd.Amount;
            }
        }

        /// <summary>
        /// Возвращает список доступных к покупке от игры фабрик.
        /// </summary>
        /// <param name="customer">Команда.</param>
        /// <param name="onlyTopLevel">Только последнего уровня, иначе вообще всех доступных.</param>
        /// <returns>Список доступных к покупке фабрик.</returns>
        public static IList<FactoryDefinition> GetAvailFactoryDefenitions(Customer customer, bool onlyTopLevel = true)
        {
            var productionType = customer.ProductionType;
            var factoryDefinitions = FactoryDefinitions.Where(f => f.ProductionType.Id == productionType.Id && f.GenerationLevel <= customer.FactoryGenerationLevel);
            if (onlyTopLevel)
            {
                factoryDefinitions =  factoryDefinitions.Where(f => f.GenerationLevel == customer.FactoryGenerationLevel);
            }

            return factoryDefinitions.ToList();
        }

        /// <summary>
        /// Расчёт процентной ставки страхования для продавца.
        /// </summary>
        /// <param name="customer">Команда продавца.</param>
        /// <returns>Процентная ставка.</returns>
        public static decimal CalculateInsurancePercentForSeller(Customer customer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Расчёт процентной ставки страхования для покупателя.
        /// </summary>
        /// <param name="customer">Команда покупателя.</param>
        /// <returns>Процентная ставка.</returns>
        public static decimal CalculateInsurancePercentForBuyer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public static void InitReferences()
        {
            InitProductionTypes();
            InitMaterials();
            InitSupply();
            InitDemand();
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