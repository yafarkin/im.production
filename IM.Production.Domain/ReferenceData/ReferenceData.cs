﻿using System;
using System.Collections.Generic;
using System.Linq;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;

namespace Epam.ImitationGames.Production.Domain.ReferenceData
{
    /// <summary>
    /// Класс, хранящий справочные данные и расчитывающий разные показатели
    /// </summary>
    public static class ReferenceData
    {
        public static InitialData Data = new InitialData();

        /// <summary>
        /// Инициализация данных.
        /// </summary>
        public static InitialData GetData()
        {
            ReferenceData.InitReferences();
            return Data;
        }
        
        /// <summary>
        /// Расчёт общей стоимости для исследования фабрик следующего поколения.
        /// </summary>
        /// <param name="customer">Команда.</param>
        /// <returns>Стоимость исследования фабрик следующего поколения.</returns>
        public static decimal CalculateRDSummToNextGenerationLevel(Customer customer)
        {
            var currentGenerationLevel = customer.FactoryGenerationLevel;
            var cost = Data.GenerationFactoryRDCost.FirstOrDefault(c => c.Key == currentGenerationLevel + 1);
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
            var cost = Data.FactoryLevelUpRDCost.FirstOrDefault(c => c.Key == currentFactoryLevel + 1);
            return CalculateFactoryCost(factory.FactoryDefinition) * cost.Value;
        }

        /// <summary>
        /// Расчёт стоимости фабрики, на основе её описания.
        /// </summary>
        /// <param name="factoryDefinition">Описание фабрики.</param>
        /// <returns>Стоимость фабрики.</returns>
        public static decimal CalculateFactoryCost(FactoryDefinition factoryDefinition)
        {
            decimal cost;
            var factoryCost = Data.FactoryCost.FirstOrDefault(f => f.Key == factoryDefinition.Id);
            if (factoryCost.Key == Guid.Empty)
            {
                // считаем что данные по уровням упорядочены уже в определении
                cost = Data.DefaultFactoryCost.First().Value;
                foreach (var defaultCost in Data.DefaultFactoryCost)
                {
                    if (defaultCost.Key >= factoryDefinition.GenerationLevel)
                    {
                        cost = defaultCost.Value;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                cost = factoryCost.Value;
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
            var tax = Data.FactoryTaxes.FirstOrDefault(f => f.FactoryDefinition.Id == factory.FactoryDefinition.Id)?.Percent ?? Data.DefaultFactoryTax;
            return tax;
        }

        /// <summary>
        /// Расчёт налога на продажу единицы материала.
        /// </summary>
        /// <param name="material">Материал.</param>
        /// <returns>Сумма налога.</returns>
        public static decimal CalculateTaxOnMaterial(Material material)
        {
            var tax = Data.MaterialTaxes.FirstOrDefault(m => m.Material.Id == material.Id)?.Percent ?? Data.DefaultMaterialTax;
            return tax;
        }

        public static void CalculateMaterialInDetails(Material material, out decimal price, out List<MaterialWithPrice> sourceMaterials, decimal amountPerDay = 1)
        {
            price = 0;
            sourceMaterials = new List<MaterialWithPrice>();

            var supplyMaterial = Data.Supply.Materials.FirstOrDefault(m => m.Material.Id == material.Id);
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
            var supplyMaterial = Data.Supply.Materials.FirstOrDefault(m => m.Material.Id == material.Id);
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
            var developed = factory.FactoryDefinition.GenerationLevel > 1 && factory.FactoryDefinition.GenerationLevel == maxGeneration;

            return developed ? advancedExtraChargePercent : defaultExtraChargePercent;
        }

        // TODO It might become redundant
        public static void UpdateGameDemand(IEnumerable<Factory> factories)
        {
            foreach (var factory in factories)
            {
                var extraChargePercent = CalculateFactoryExtraChargePercent(factory, factories);

                foreach (var material in factory.ProductionMaterials)
                {
                    var demand = Data.Demand.Materials.FirstOrDefault(m => m.Material.Id == material.Id);

                    if (demand == null)
                    {
                        var costPrice = CalculateMaterialCostPrice(material);                        
                        var extraCharge = costPrice * extraChargePercent;

                        Data.Demand.Materials.Add(new MaterialWithPrice
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
        public static decimal CalculateWorkerSalary(Factory factory)
        {
            // расчитываем как 10% от от уровня фабрики * базовая зарплата
            var salary = (1 + decimal.Divide(1, factory.FactoryDefinition.GenerationLevel)) * Data.BaseWorkerSalay;
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
                    foreach (var kv in Data.FactoryOverPerformance)
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
            return Data.ProductionTypes.First(x => x.Key.ToLower() == key.ToLower());
        }

        public static Material GetMaterialByKey(string key)
        {
            var result = Data.Materials.FirstOrDefault(x => x.Key.ToLower() == key.ToLower());
            if (null == result)
            {
                //throw new InvalidOperationException($"Отсутствует указанный ключ материала: {key}");
            }

            return result;
        }

        private static void InitProductionTypes()
        {
            Data.ProductionTypes = new List<ProductionType>
            {
                new ProductionType {Key = "metall", DisplayName = "Металлургическая промышленность"},
                new ProductionType {Key = "electronic", DisplayName = "Электронная промышленность"},
                new ProductionType {Key = "chemical", DisplayName = "Химическая промышленность"}
            };
        }

        private static void InitMaterials()
        {
            //Материалы металлургии и производства самолетов
            Data.Materials = new List<Material>();

            Data.Materials.Add(new Material
            {
                Key = "metall_ruda",
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Металлосодержащая руда",
            });

            Data.Materials.Add(new Material
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

            Data.Materials.Add(new Material
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

            Data.Materials.Add(new Material
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

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_zelezo_list",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Стальной лист"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_med_list",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_med"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Медный лист"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_zoloto_list",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zoloto"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Золотой лист"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_soplo",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 3
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_detaley"),
                        Amount = 3
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Сопло"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_camera_sgoraniya",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 6
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_detaley"),
                        Amount = 6
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Камера сгорания"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_gazogenerator",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 10
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_detaley"),
                        Amount = 10
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Газогенератор"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_compressor",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_detaley"),
                        Amount = 4
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Компрессор"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_korpus_legkiy_samolet",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 30
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_samoletov"),
                        Amount = 20
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 25
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_stecloplasticovie_komponenty"),
                        Amount = 12
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 6
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Корпус легкого самолета"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_korpus_samolet_gruzovoy",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 62
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_samoletov"),
                        Amount = 35
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 80
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_stecloplasticovie_komponenty"),
                        Amount = 32
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 16
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Корпус грузового самолета"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_react_dvigatel_samolet",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 2
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_camera_sgoraniya"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_soplo"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_compressor"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Реактивный двигатель самолета"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_legkiy_samolet",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_korpus_legkiy_samolet"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_react_dvigatel_samolet"),
                        Amount = 2
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_bortovoy_computer"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Легкий самолет"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_systema_kondizioner_samolet",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 12
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_stecloplasticovie_komponenty"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_vichislitelnaya_apparatura"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Система кондиционирования воздуха для самолета"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_gruz_samolet",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_korpus_samolet_gruzovoy"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_systema_kondizioner_samolet"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_react_dvigatel_samolet"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_bortovoy_computer"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_systema_navigazii"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Грузовой самолет"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_registr_apparat",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 6
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_vichislitelnaya_apparatura"),
                        Amount = 2
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribory_controlya"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_izmeritelnie_pribori"),
                        Amount = 14
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Регистрирующая аппаратура для летающих лабораторий"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_vichislitel_complex",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_vichislitelnaya_apparatura"),
                        Amount = 16
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribory_controlya"),
                        Amount = 10
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 10
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 7
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Вычислительный комплекс для летающих лабораторий"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_laboratoriya_samolet",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_gruz_samolet"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_registr_apparat"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_vichislitel_complex"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Самолет - Летающая лаборатория"
            });

            //Материалы химии и производства ракет.
            Data.Materials.Add(new Material
            {
                Key = "chemical_reagenty",
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Химические реагенты"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_reagenty_s_zashitnimy_svoystvami",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_reagenty"),
                        Amount = 10000  // Значение взято по аналогии с металлургией
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Реагенты с защитными свойствами"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_plastic",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_reagenty"),
                        Amount = 15000  // Значение взято по аналогии с металлургией
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Пластик"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_kremniy",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_reagenty"),
                        Amount = 50000  // Значение взято по аналогии с металлургией
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Кремний"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_rezina",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_reagenty"),
                        Amount = 15000  // Значение взято по аналогии с металлургией
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Резина"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_stecloplastic",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_reagenty"),
                        Amount = 50000  // Значение взято по аналогии с металлургией
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Стеклопластик"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_ygleplastic",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_reagenty"),
                        Amount = 100000  // Значение взято по аналогии с металлургией
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Углепластик"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_plytoniy_238",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_reagenty"),
                        Amount = 200000  // Значение взято по аналогии с металлургией
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Плутоний-238"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_rastvor_dlya_pokritiya_detaley",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_reagenty_s_zashitnimy_svoystvami"),
                        Amount = 3
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Раствор для покрытия деталей"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_rastvor_dlya_pokritiya_samoletov",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_reagenty_s_zashitnimy_svoystvami"),
                        Amount = 6
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Раствор для покрытия самолетов"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_rastvor_dlya_pokritiya_kosmicheskih_apparatov",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_reagenty_s_zashitnimy_svoystvami"),
                        Amount = 10
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Раствор для покрытия космических аппаратов"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_rezinovie_komponenty",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezina"),
                        Amount = 6
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Резиновые компоненты"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_plasticovie_komponenty",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_plastic"),
                        Amount = 6
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Пластиковые компоненты"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_stecloplasticovie_komponenty",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_stecloplastic"),
                        Amount = 4
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Стеклопластиковые компоненты"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_ygleplasticovie_komponenty",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplastic"),
                        Amount = 2
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Углепластиковые компоненты"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_korpus_raketa_nositelya",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 70
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_kosmicheskih_apparatov"),
                        Amount = 18
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 26
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_stecloplasticovie_komponenty"),
                        Amount = 13
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Корпус ракеты-носителя"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_raketniy_dvigatel",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 5
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_camera_sgoraniya"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_soplo"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_gazogenerator"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Ракетный двигатель"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_raketa_nositel",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_raketniy_dvigatel"),
                        Amount = 3
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_korpus_raketa_nositelya"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribory_controlya"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Ракета-носитель"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_systema_kislorodoobespecheniya",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 12
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 16
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_stecloplasticovie_komponenty"),
                        Amount = 9
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_vichislitelnaya_apparatura"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Система кислородообеспечения"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_pilot_kosmicheskiy_korabl",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_kosmicheskih_apparatov"),
                        Amount = 40
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_systema_kislorodoobespecheniya"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 80
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 15
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_bortovoy_computer"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_systema_navigazii"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Пилотируемый космический корабль"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_modul_kosmicheskoy_stanzii",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_kosmicheskih_apparatov"),
                        Amount = 15
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_systema_kislorodoobespecheniya"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 30
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 20
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_vichislitelnaya_apparatura"),
                        Amount = 2
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribory_controlya"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_izmeritelnie_pribori"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Модуль космической станции"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_oborudovaniye_dlya_experimentov_v_kosmose",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 10
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zoloto_list"),
                        Amount = 5
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 20
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_stecloplasticovie_komponenty"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_vichislitelnaya_apparatura"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribory_controlya"),
                        Amount = 8
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Оборудование для экспериментов в космосе"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_kosmicheskaya_orbitalnaya_stanziya",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_oborudovaniye_dlya_experimentov_v_kosmose"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_modul_kosmicheskoy_stanzii"),
                        Amount = 3
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_bortovoy_computer"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_systema_navigazii"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_solnechnaya_batareya"),
                        Amount = 6
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Космическая орбитальная станция"
            });

            //Материалы электроники и производства спутников
            Data.Materials.Add(new Material
            {
                Key = "electronic_bakelit",
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Бакелит"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_textolit",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_bakelit"),
                        Amount = 10000  // Значение взято по аналогии с металлургией
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Текстолит"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_pechatnaya_plata",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_textolit"),
                        Amount = 2
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Печатная плата"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_komponenty",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_med_list"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zoloto_list"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_plasticovie_komponenty"),
                        Amount = 2
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Электронные компоненты"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_vichislitelnaya_apparatura",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_textolit"),
                        Amount = 2
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_komponenty"),
                        Amount = 14
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_plasticovie_komponenty"),
                        Amount = 25
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 18
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Вычислительная аппаратура"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_bortovoy_computer",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_textolit"),
                        Amount = 10
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_komponenty"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_plasticovie_komponenty"),
                        Amount = 14
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 20
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_stecloplasticovie_komponenty"),
                        Amount = 8
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Бортовой компьютер"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_korpus_vishki_svyasi",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 17
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_detaley"),
                        Amount = 5
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 12
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Корпус вышки связи"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_korpus_spytnika_svyasi",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 30
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_kosmicheskih_apparatov"),
                        Amount = 10
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_stecloplasticovie_komponenty"),
                        Amount = 12
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 6
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Корпус спутника связи"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_korpus_issledovatelyskogo_sputnika",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 25
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_kosmicheskih_apparatov"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 5
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_stecloplasticovie_komponenty"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 15
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Корпус исследовательского спутника"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_pribori_svyasi",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_komponenty"),
                        Amount = 10
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_plasticovie_komponenty"),
                        Amount = 20
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_med_list"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zoloto_list"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Приборы связи"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_izmeritelnie_pribori",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_komponenty"),
                        Amount = 18
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_plasticovie_komponenty"),
                        Amount = 20
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_med_list"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zoloto_list"),
                        Amount = 2
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Измерительные приборы"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_pribory_controlya",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_komponenty"),
                        Amount = 12
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_vichislitelnaya_apparatura"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_plasticovie_komponenty"),
                        Amount = 20
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_med_list"),
                        Amount = 3
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zoloto_list"),
                        Amount = 6
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Приборы контроля"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_vishka_svyasi",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribori_svyasi"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_korpus_vishki_svyasi"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Вышка связи"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_systema_navigazii",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribori_svyasi"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 5
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 10
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 5
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_vichislitelnaya_apparatura"),
                        Amount = 1
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Система навигации"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_solnechnaya_batareya",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribory_controlya"),
                        Amount = 2
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_izmeritelnie_pribori"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 7
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_kremniy"),
                        Amount = 21
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Солнечная батарея"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_spytnik_svyasi",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_korpus_issledovatelyskogo_sputnika"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_vichislitelnaya_apparatura"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribory_controlya"),
                        Amount = 3
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_systema_navigazii"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_solnechnaya_batareya"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rastvor_dlya_pokritiya_kosmicheskih_apparatov"),
                        Amount = 15
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Спутник связи"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_oborudovaniye_distanzionnogo_zondirovaniya",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_vichislitelnaya_apparatura"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribory_controlya"),
                        Amount = 10
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribori_svyasi"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 5
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 10
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Оборудование дистанционного зондирования"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_radioizotopniy_termoelectricheskiy_generator",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribory_controlya"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_izmeritelnie_pribori"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 15
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zoloto_list"),
                        Amount = 6
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_plytoniy_238"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 12
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Радиоизотопный термоэлектрический генератор"
            });

            Data.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_issledovatelyskiy_sputnik",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_radioizotopniy_termoelectricheskiy_generator"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_oborudovaniye_distanzionnogo_zondirovaniya"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_korpus_issledovatelyskogo_sputnika"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_systema_navigazii"),
                        Amount = 1
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_vichislitelnaya_apparatura"),
                        Amount = 8
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribory_controlya"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_pribori_svyasi"),
                        Amount = 3
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_izmeritelnie_priborii"),
                        Amount = 8
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Исследовательский спутник"
            });
        }

        private static void InitSupply()
        {
            Data.Supply = new GameSupply
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

        private static void InitDemand() => Data.Demand = new GameDemand();

        private static void InitFactoryDefinition()
        {
            Data.FactoryDefinitions = new List<FactoryDefinition>();

            //Металлургия и производство самолетов
            Data.FactoryDefinitions.Add(new FactoryDefinition
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

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 15,
                GenerationLevel = 2,
                Name = "Производство металлических листов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_zelezo_list")}},
                    {2, new List<Material>{GetMaterialByKey("metall_med_list"), GetMaterialByKey("metall_zoloto_list")}}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 20,
                GenerationLevel = 2,
                Name = "Производство сложных металлических компонентов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {   1,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_camera_sgoraniya"),
                            GetMaterialByKey("metall_soplo")
                        }
                    },
                    {
                        2,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_compressor"),
                            GetMaterialByKey("metall_gazogenerator")
                        }
                    }
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 30,
                GenerationLevel = 3,
                Name = "Производство самолетных корпусов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_korpus_samolet_dvuhmotor")}},
                    {2, new List<Material> {GetMaterialByKey("metall_korpus_samolet_gruzovoy")}}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 25,
                GenerationLevel = 4,
                Name = "Производство реактивных двигателей",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_react_dvigatel_samolet")}}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 40,
                GenerationLevel = 5,
                Name = "Производство легких самолетов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_legkiy_samolet") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 30,
                GenerationLevel = 6,
                Name = "Производство систем кондиционирования воздуха для самолетов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_systema_kondizioner_samolet") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 60,
                GenerationLevel = 7,
                Name = "Производство грузовых самолетов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_gruz_samolet") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 40,
                GenerationLevel = 8,
                Name = "Производство регистрирующей аппаратуры",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_registr_apparat") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 50,
                GenerationLevel = 9,
                Name = "Производство вычислительных комплексов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_vichislitel_complex") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("metall"),
                BaseWorkers = 80,
                GenerationLevel = 10,
                Name = "Производство самолетов - Летающая лаборатория",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_laboratoriya_samolet") }}
                }
            });

            //Химия и производство ракет.
            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("chemical"),
                BaseWorkers = 15,
                GenerationLevel = 1,
                Name = "Производство базовых химических материалов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {
                        1,
                        new List<Material>
                        {
                            GetMaterialByKey("chemical_reagenty_s_zashitnimy_svoystvami"),
                            GetMaterialByKey("chemical_plastic"),
                            GetMaterialByKey("chemical_rezina"),
                            GetMaterialByKey("chemical_kremniy")
                        }
                    },
                    {
                        2,
                        new List<Material>
                        {
                            GetMaterialByKey("chemical_stecloplastic")
                        }
                    },
                    {
                        3,
                        new List<Material>
                        {
                            GetMaterialByKey("chemical_ygleplastic"),
                            GetMaterialByKey("chemical_plytoniy_238")
                        }
                    }
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("chemical"),
                BaseWorkers = 20,
                GenerationLevel = 2,
                Name = "Производство защитных покрытий",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {
                        1,
                        new List<Material>
                        {
                            GetMaterialByKey("chemical_rastvor_dlya_pokritiya_detaley")
                        }
                    },
                    {
                        2,
                        new List<Material>
                        {
                            GetMaterialByKey("chemical_rastvor_dlya_pokritiya_kosmicheskih_apparatov"),
                            GetMaterialByKey("chemical_rastvor_dlya_pokritiya_samoletov")
                        }
                    }
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("chemical"),
                BaseWorkers = 25,
                GenerationLevel = 2,
                Name = "Производство сложных химических компонентов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {
                        1,
                        new List<Material>
                        {
                            GetMaterialByKey("chemical_rezinovie_komponenty"),
                            GetMaterialByKey("chemical_plasticovie_komponenty")
                        }
                    },
                    {
                        2,
                        new List<Material>
                        {
                            GetMaterialByKey("chemical_stecloplasticovie_komponenty")
                        }
                    },
                    {
                        3,
                        new List<Material>
                        {
                            GetMaterialByKey("chemical_ygleplasticovie_komponenty")
                        }
                    }
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("chemical"),
                BaseWorkers = 40,
                GenerationLevel = 3,
                Name = "Производство корпусов ракет-носителей",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("chemical_korpus_raketa_nositelya") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("chemical"),
                BaseWorkers = 30,
                GenerationLevel = 4,
                Name = "Производство ракетных двигателей",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("chemical_raketniy_dvigatel") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("chemical"),
                BaseWorkers = 60,
                GenerationLevel = 5,
                Name = "Производство ракет-носителей",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("chemical_raketa_nositel") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("chemical"),
                BaseWorkers = 45,
                GenerationLevel = 6,
                Name = "Производство систем кислородообеспечения",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("chemical_systema_kislorodoobespecheniya") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("chemical"),
                BaseWorkers = 60,
                GenerationLevel = 7,
                Name = "Производство пилотируемых космических кораблей",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("chemical_pilot_kosmicheskiy_korabl") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("chemical"),
                BaseWorkers = 80,
                GenerationLevel = 8,
                Name = "Производство модулей космической станции",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("chemical_modul_kosmicheskoy_stanzii") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("chemical"),
                BaseWorkers = 35,
                GenerationLevel = 9,
                Name = "Производство оборудования для экспериментов в космосе",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("chemical_oborudovaniye_dlya_experimentov_v_kosmose") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("chemical"),
                BaseWorkers = 90,
                GenerationLevel = 10,
                Name = "Производство космических орбитальных станций",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("chemical_kosmicheskaya_orbitalnaya_stanziya") }}
                }
            });

            //Электроника и производство спутников.
            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 10,
                GenerationLevel = 1,
                Name = "Производство текстолита",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_textolit") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 20,
                GenerationLevel = 2,
                Name = "Производство печатных плат электронных компонентов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_pechatnaya_plata") }},
                    {2, new List<Material> {GetMaterialByKey("electronic_komponenty") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 28,
                GenerationLevel = 3,
                Name = "Производство ЭВМ",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_vichislitelnaya_apparatura") }},
                    {2, new List<Material> {GetMaterialByKey("electronic_bortovoy_computer") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 40,
                GenerationLevel = 3,
                Name = "Производство корпусов электронных конструкций",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_korpus_vishki_svyasi") }},
                    {2, new List<Material> {GetMaterialByKey("electronic_korpus_spytnika_svyasi") }},
                    {3, new List<Material> {GetMaterialByKey("electronic_korpus_issledovatelyskogo_sputnika") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 30,
                GenerationLevel = 4,
                Name = "Производство приборов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_pribori_svyasi") }},
                    {2, new List<Material> {GetMaterialByKey("electronic_izmeritelnie_pribori") }},
                    {3, new List<Material> {GetMaterialByKey("electronic_pribory_controlya") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 34,
                GenerationLevel = 5,
                Name = "Производство вышек связи",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_vishka_svyasi") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 40,
                GenerationLevel = 6,
                Name = "Производство простого вспомогательного оборудования",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_systema_navigazii") }},
                    {2, new List<Material> {GetMaterialByKey("electronic_solnechnaya_batareya") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 55,
                GenerationLevel = 7,
                Name = "Производство спутников связи",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_spytnik_svyasi") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 35,
                GenerationLevel = 8,
                Name = "Производство оборудования дистанционного зондирования",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_oborudovaniye_distanzionnogo_zondirovaniya") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 45,
                GenerationLevel = 9,
                Name = "Производство радиоизотопного термоэлектрического генератора",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_radioizotopniy_termoelectricheskiy_generator") }}
                }
            });

            Data.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 70,
                GenerationLevel = 10,
                Name = "Производство исследовательских спутников",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_issledovatelyskiy_sputnik") }}
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