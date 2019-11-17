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

            /*
            //Материалы электроники и производства спутников
            Materials.Add(new Material
            {
                Key = "electronic_bakelit", 
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Бакелит"
            });

            Materials.Add(new Material
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

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "electronic_pechatnaya_plata",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("electronic_textolit"),
                        Amount = 2  // Значение взято по аналогии с металлургией
                    }
                },
                ProductionType = GetProductionTypeByKey("electronic"),
                DisplayName = "Печатная плата"
            });*/

            //Материалы химии и производства ракет.
            Materials.Add(new Material
            {
                Key = "chemical_reagenty",
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Химические реагенты"
            });

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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

            Materials.Add(new Material
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
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Ракета-носитель"
            }); 

            Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "chemical_systema_kislorodoobespecheniya",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_rezinovie_komponenty"),
                        Amount = 50
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("metall_zelezo_list"),
                        Amount = 12
                    },
                    new MaterialOnStock
                    {
                        Material = GetMaterialByKey("chemical_ygleplasticovie_komponenty"),
                        Amount = 15
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Система кислородообеспечения"
            }); 

            Materials.Add(new Material
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
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Пилотируемый космический корабль"
            });

            Materials.Add(new Material
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
                        Amount = 8
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Модуль космической станции"
            }); 

            Materials.Add(new Material
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
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Оборудование для экспериментов в космосе"
            });

            Materials.Add(new Material
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
                    }
                },
                ProductionType = GetProductionTypeByKey("chemical"),
                DisplayName = "Космическая орбитальная станция"
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
                BaseWorkers = 15,
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

            /*
            //Электроника и производство спутников.
            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = GetProductionTypeByKey("electronic"),
                BaseWorkers = 20,
                GenerationLevel = 2,
                Name = "Производство печатных плат",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {GetMaterialByKey("electronic_pechatnaya_plata") }}
                }
            });*/

            //Химия и производство ракет.
            FactoryDefinitions.Add(new FactoryDefinition
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
                            GetMaterialByKey("chemical_rezina")
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
                            GetMaterialByKey("chemical_ygleplastic")
                        }
                    }
                }
            });

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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

            FactoryDefinitions.Add(new FactoryDefinition
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
        }
    }
}
