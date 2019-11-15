using System;
using System.Collections.Generic;
using System.Linq;
using Epam.ImitationGames.Production.Common.Production;

namespace Epam.ImitationGames.Production.Common.InitialGameData
{
    /// <summary>
    /// Класс описывает исходные игровые данные. Нужен для десериализации объекта из JSON-файла.
    /// Создание объекта, заполнение полей его методами и сериализация происходит в тесте.
    /// </summary>
    class InitialGameData
    {
        /// <summary>
        /// Список фабрик, которые есть в игре.
        /// </summary>
        public static List<FactoryDefinition> FactoryDefinitions { get; private set; }

        /// <summary>
        /// Типы продукции, с которыми связаны фабрики.
        /// </summary>
        public static List<ProductionType> ProductionTypes { get; private set; }

        /// <summary>
        /// Общий список материалов в игре.
        /// </summary>
        public static List<Material> Materials { get; private set; }

        /// <summary>
        /// Инициализация типов продукции. 
        /// </summary>
        private static void InitProductionTypes()
        {
            ProductionTypes = new List<ProductionType>
            {
                new ProductionType {Key = "metall", DisplayName = "Металлургическая промышленность"},
                new ProductionType {Key = "electronic", DisplayName = "Электронная промышленность"},
                new ProductionType {Key = "chemical", DisplayName = "Химическая промышленность"}
            };
        }

        /// <summary>
        /// Инициализация материалов.
        /// </summary>
        private static void InitMaterials()
        {
            //Материалы металлургии и производства самолетов
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
                        Material = GetMaterialByKey("metall_zelezo"),
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
                        Material = GetMaterialByKey("metall_med"),
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
                        Material = GetMaterialByKey("metall_zoloto"),
                        Amount = 5
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Золотой лист"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_soplo",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 3
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Сопло"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_camera_sgoraniya",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 6
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Камера сгорания"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_gazogenerator",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 10
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Газогенератор"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_compressor",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 4
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Компрессор"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_korpus_legkiy_samolet",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 30
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Корпус легкого самолета"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_korpus_samolet_gruzovoy",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 62
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Корпус грузового самолета"
            });

            Materials.Add(new Material
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

            Materials.Add(new Material
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
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Легкий самолет"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_systema_kondizioner_samolet",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 8
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Система кондиционирования воздуха для самолета"
            });

            Materials.Add(new Material
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
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Грузовой самолет"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_registr_apparat",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 6
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Регистрирующая аппаратура для летающих лабораторий"
            });

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_vichislitel_complex",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 8
                    }
                },
                ProductionType = GetProductionTypeByKey("metall"),
                DisplayName = "Вычислительный комплекс для летающих лабораторий"
            });

            Materials.Add(new Material
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
        }

        /// <summary>
        /// Инициализация фабрик.
        /// </summary>
        private static void InitFactoryDefinition()
        {
            FactoryDefinitions = new List<FactoryDefinition>();

            //Металлургия и производство самолетов
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
                Name = "Производство металлических листов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("metall_zelezo_list")}},
                    {
                        2,
                        new List<Material>
                        {
                            GetMaterialByKey("metall_med_list"),
                            GetMaterialByKey("metall_zoloto_list")
                        }
                    }
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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
        }
    }
}
