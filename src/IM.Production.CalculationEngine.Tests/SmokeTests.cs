using System;
using System.Collections.Generic;
using System.Linq;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Bank;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IM.Production.CalculationEngine.Tests
{
    [TestClass]
    public class SmokeTests
    {
        public Game Game;
        public CalculationEngine Engine;
        public Logic Logic;

        [TestInitialize]
        public void Init()
        {
            Game = new Game();
            Engine = new CalculationEngine(Game);
            Logic = new Logic(Game);

            InitMaterials();
            InitFactoryDefinition();
        }

        #region init reference data

        private void InitFactoryDefinition()
        {
            ReferenceData.FactoryDefinitions.Clear();

            // металлургия
            // руда -> железный слиток -> лист -> проволока -> электронный блок=блок управления
            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                BaseWorkers = 3,
                GenerationLevel = 1,
                Name = "Добыча металлической руды",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("metall_zelezo_ruda")}},
                    {2, new List<Material> {ReferenceData.GetMaterialByKey("metall_med_ruda")}},
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                BaseWorkers = 3,
                GenerationLevel = 2,
                Name = "Производство железной руды",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("metall_zelezo")}},
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                BaseWorkers = 2,
                GenerationLevel = 3,
                Name = "Производство металлических листов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("metall_zelezo_list")}},
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                BaseWorkers = 1,
                GenerationLevel = 4,
                Name = "Производство проволоки",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("provoloka")}}
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                BaseWorkers = 1,
                GenerationLevel = 5,
                Name = "Производство блоков управления",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("blok_upravlenia")}}
                }
            });

            // электроника
            // кремниевая порода -> кремний -> проволока+кремний=печатная плата -> электронный блок -> компьютер
            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                BaseWorkers = 3,
                GenerationLevel = 1,
                Name = "Добыча кремниевой породы",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("electronic_kremnii_ruda")}}
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                BaseWorkers = 1,
                GenerationLevel = 2,
                Name = "Производство кремния",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("electronic_kremnii")}}
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                BaseWorkers = 4,
                GenerationLevel = 3,
                Name = "Производство печатной платы",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("plata")}}
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                BaseWorkers = 2,
                GenerationLevel = 4,
                Name = "Производство электронных блоков",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("electro_blok")}}
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                BaseWorkers = 2,
                GenerationLevel = 5,
                Name = "Производство компьютеров",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("computer")}}
                }
            });
        }

        private void InitMaterials()
        {
            ReferenceData.Materials.Clear();

            ReferenceData.Materials.Add(new Material
            {
                Key = "ruda",
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Руда",
                AmountPerDay = 20000
            });

            ReferenceData.Materials.Add(new Material
            {
                Key = "metall_zelezo_ruda",
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Металлосодержащая руда",
                AmountPerDay = 20000,
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock {Material = ReferenceData.GetMaterialByKey("ruda"), Amount = 100000}
                }
            });

            ReferenceData.Materials.Add(new Material
            {
                Key = "metall_med_ruda",
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Медесодержащая руда",
                AmountPerDay = 1000,
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock {Material = ReferenceData.GetMaterialByKey("ruda"), Amount = 100000}
                }
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 20m,
                Key = "metall_zelezo",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("metall_zelezo_ruda"),
                        Amount = 10000
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Железная руда"
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_zelezo_list",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("metall_zelezo"),
                        Amount = 5
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Стальной лист"
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 2000m,
                Key = "provoloka",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("metall_zelezo_list"),
                        Amount = 1
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Железная проволока"
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "blok_upravlenia",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("electro_blok"),
                        Amount = 3
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Блок управления"
            });

            ReferenceData.Materials.Add(new Material
            {
                Key = "electronic_kremnii_ruda",
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                DisplayName = "Кремнесодержащая руда",
                AmountPerDay = 25000,
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock {Material = ReferenceData.GetMaterialByKey("ruda"), Amount = 100000}
                }
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 1000,
                Key = "electronic_kremnii",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("electronic_kremnii_ruda"),
                        Amount = 8000
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                DisplayName = "Кремний"
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 2,
                Key = "plata",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("electronic_kremnii"),
                        Amount = 12000
                    },
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("provoloka"),
                        Amount = 7000
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                DisplayName = "Печатная плата"
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "electro_blok",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("plata"),
                        Amount = 3
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                DisplayName = "Электронный блок"
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 1,
                Key = "computer",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("electro_blok"),
                        Amount = 10
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                DisplayName = "Электронный блок"
            });

            ReferenceData.Supply.Materials = new List<MaterialWithPrice>
            {
                new MaterialWithPrice {Material = ReferenceData.GetMaterialByKey("ruda"), SellPrice = 0.002m}
            };
        }

        #endregion

        protected void RunCycles(int cycles = 1)
        {
            for (var i = 0; i < cycles; i++)
            {
                Engine.Calculate();
            }
        }

        [TestMethod]
        public void OneCustomerTest()
        {
            // одна команда
            // 1. открывают 1 фабрику, перерабатывают, сразу всё продают
            // 2. ставят исследования уровня фабрики
            // 3. после завершения исследования, убирают исследования на фабрику, производительность фабрики растёт
            // 4. начинают исследования поколения №2
            // 5. после исследования поколения №2, покупают фабрику №2, заключают контракт на поставку с фабрики №1 и начинают производство №2, продавая излишки игре

            var c = Logic.AddCustomer("c", "1", "команда 1", ReferenceData.GetProductionTypeByKey("metall"));
            Assert.AreEqual(ReferenceData.InitialCustomerBalance, c.Sum);

            var f1 = Logic.BuyFactoryFromGame(c, ReferenceData.GetAvailFactoryDefenitions(c).First());
            Logic.UpdateFactorySettings(f1, null, 33);

            Logic.UpdateFactorySettings(f1, null, null,
                new List<Material> {ReferenceData.GetMaterialByKey("metall_zelezo_ruda")});

            // руда -> команде
            var co1 = Logic.AddContract(new Contract(Game.Time, c,
                new MaterialWithPrice {Amount = 100000, Material = ReferenceData.GetMaterialByKey("ruda")})
            {
                DestinationFactory = f1
            });

            // железная руда -> игре
            var co2 = Logic.AddContract(new Contract(Game.Time, c,
                new MaterialWithPrice { Amount = 999999, Material = ReferenceData.GetMaterialByKey("metall_zelezo_ruda") })
            {
                SourceFactory = f1
            });

            Assert.AreEqual(99000, c.Sum);

            var diff = 155.35m;
            RunCycles(10);

            Assert.AreEqual(99000+diff, c.Sum);

            // теперь фабрика второго уровня, производит в два раза больше. тормозим исследования по ней и обновляем контракт
            // что бы производить больше материалов
            Assert.AreEqual(2, f1.Performance);
            Logic.UpdateFactorySettings(f1, null, 0);

            Logic.CloseContract(co1);
            co1 = Logic.AddContract(new Contract(Game.Time, c,
                new MaterialWithPrice {Amount = 200000, Material = ReferenceData.GetMaterialByKey("ruda")})
            {
                DestinationFactory = f1
            });

            // после модернизации фабрики и отключения затрат на исследования по ней, прибыль значительно выросла
            diff += 1751;
            RunCycles(10);

            Assert.AreEqual(2, f1.Level);
            Assert.AreEqual(99000 + diff, c.Sum);

            // выделяем сумму на исследование следующего поколения фабрик и останавливаем продажу ресурсов игре
            Logic.CloseContract(co2);

            Logic.UpdateCustomerSettings(c, 100);

            // мы поставили исследование и перестали продавать товар игре, а затраты на налоги и покупку остались
            diff -= 2594.5m;
            RunCycles(5);

            Assert.AreEqual(2, c.FactoryGenerationLevel);
            Assert.AreEqual(99000 + diff, c.Sum);

            // покупаем фабрику №2, организовываем поставку товара на вторую фабрику, а продажу с фабрики №2 - игре
            Logic.UpdateCustomerSettings(c, 0);
            var f2 = Logic.BuyFactoryFromGame(c, ReferenceData.GetAvailFactoryDefenitions(c).First());

            // организовываем производство материала на второй фабрике
            Logic.UpdateFactorySettings(f2, null, null,
                new List<Material> {ReferenceData.GetMaterialByKey("metall_zelezo")});

            var materialZelezoRuda = ReferenceData.GetMaterialByKey("metall_zelezo_ruda");
            var nowZelezoRuda = f1.Stock.First(m => m.Material.Id == materialZelezoRuda.Id);

            // разовым контрактом переместим всё что накопилось на складе первой фабрики на вторую фабрику
            var co3 = Logic.AddContract(
                new Contract(Game.Time, c,
                    new MaterialWithPrice {Amount = nowZelezoRuda.Amount, Material = materialZelezoRuda})
                {
                    SourceFactory = f1, DestinationFactory = f2, TillCount = Convert.ToInt32(nowZelezoRuda.Amount)
                });

            // добавляем постоянный контракт на перенос
            var co4 = Logic.AddContract(
                new Contract(Game.Time, c,
                    new MaterialWithPrice {Amount = 40000, Material = materialZelezoRuda})
                {
                    SourceFactory = f1, DestinationFactory = f2
                });

            // и контракт на продажу игре
            var co5 = Logic.AddContract(
                new Contract(Game.Time, c,
                    new MaterialWithPrice {Amount = 999999, Material = ReferenceData.GetMaterialByKey("metall_zelezo")})
                {
                    SourceFactory = f2,
                });

            // мы купили фабрику и идут новые налоги; с другой стороны - организовано производство №2
            diff -= 5420.75m;
            RunCycles(10);
            Assert.AreEqual(99000 + diff, c.Sum);

            // понимаем, что фабрика №2 может перерабатывать только 10к единиц материала, а мы поставляем ей 40к. прокачиваем до третьего уровня, причем сразу
            Logic.UpdateFactorySettings(f2, null, f2.NeedSumToNextLevelUp);
            RunCycles();
            Assert.AreEqual(2, f2.Level);
            Assert.AreEqual(2, f2.Performance);
            Logic.UpdateFactorySettings(f2, null, f2.NeedSumToNextLevelUp);
            RunCycles();
            Assert.AreEqual(3, f2.Level);
            Assert.AreEqual(3, f2.Performance);
            Logic.UpdateFactorySettings(f2, null, f2.NeedSumToNextLevelUp);
            RunCycles();
            Assert.AreEqual(4, f2.Level);
            Assert.AreEqual(4, f2.Performance);
            Logic.UpdateFactorySettings(f2, null, 0);

            diff -= 3658.00625m;
            RunCycles(10);
            Assert.AreEqual(99000 + diff, c.Sum);

            var nowZelezoRudaCnt = f2.Stock.First(m => m.Material.Id == materialZelezoRuda.Id).Amount;
            diff += 7643.125m;
            RunCycles(50);

            // мы стали зарабатывать деньги
            Assert.AreEqual(99000 + diff, c.Sum);

            // количество материала не изменяется, мы всё что пришло - всё и перерабатываем
            Assert.AreEqual(nowZelezoRudaCnt, f2.Stock.First(m => m.Material.Id == materialZelezoRuda.Id).Amount);

            diff += 7643.125m;
            RunCycles(50);

            Assert.AreEqual(99000 + diff, c.Sum);
            Assert.AreEqual(nowZelezoRudaCnt, f2.Stock.First(m => m.Material.Id == materialZelezoRuda.Id).Amount);
        }

        [TestMethod]
        public void GameFlowSmokeTest()
        {
            // дымовой тест, проверяющий все основные функции игры
            // добавляем две команды
            // первая команда берёт кредит
            // обе команды покупают по фабрике
            // обе команды откладывают деньги на апгрейд фабрики и на следующий уровень фабрик
            // обе команды начинают производить первичные материалы
            // и т.д.

            var c1 = Logic.AddCustomer("c1", "1", "команда 1", ReferenceData.GetProductionTypeByKey("metall"));
            var c2 = Logic.AddCustomer("c2", "2", "команда 2", ReferenceData.GetProductionTypeByKey("electronic"));
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreEqual(0, c1.Contracts.Count());
            Assert.AreEqual(0, c1.BankFinanceOperations.Count());
            Assert.AreEqual(0, c1.Factories.Count());
            Assert.AreEqual(1, c1.FactoryGenerationLevel);
            Assert.AreEqual(0, c2.Contracts.Count());
            Assert.AreEqual(0, c2.BankFinanceOperations.Count());
            Assert.AreEqual(0, c2.Factories.Count());
            Assert.AreEqual(1, c2.FactoryGenerationLevel);

            Logic.TakeDebitOrCredit(c1, new BankCredit(Game.Time, c1, 100000));
            Assert.AreEqual(ReferenceData.InitialCustomerBalance + 100000, c1.Sum);
            Assert.AreEqual(ReferenceData.InitialCustomerBalance, c2.Sum);

            Assert.AreEqual(1, c1.BankFinanceOperations.Count());
            Assert.AreEqual(0, c2.BankFinanceOperations.Count());

            var f1_1 = Logic.BuyFactoryFromGame(c1, ReferenceData.GetAvailFactoryDefenitions(c1).First());
            var f2_1 = Logic.BuyFactoryFromGame(c2, ReferenceData.GetAvailFactoryDefenitions(c2).First());
            Assert.IsNotNull(f1_1);
            Assert.IsNotNull(f2_1);
            Assert.AreEqual(1, c1.Factories.Count());
            Assert.AreEqual(1, c2.Factories.Count());

            Assert.AreEqual(199000, c1.Sum);
            Assert.AreEqual(99000, c2.Sum);

            Logic.UpdateFactorySettings(f1_1, null, 50);
            Logic.UpdateFactorySettings(f2_1, null, 25);

            Logic.UpdateCustomerSettings(c1, 150);
            Logic.UpdateCustomerSettings(c2, 125);

            RunCycles();

            // выплаты ушли на налоги и зарплаты
            Assert.AreEqual(198781.25m, c1.Sum);
            Assert.AreEqual(98831.25m, c2.Sum);

            // начинаем организовывать производство
            Logic.UpdateFactorySettings(f1_1, null, null, new List<Material> {ReferenceData.GetMaterialByKey("metall_zelezo_ruda")});
            Logic.UpdateFactorySettings(f2_1, null, null, new List<Material> {ReferenceData.GetMaterialByKey("electronic_kremnii_ruda") });

            RunCycles();

            // ничего не произвели, т.к. "забыли" заключить контракт на поставку материалов от игры
            Assert.AreEqual(0, f1_1.Stock.Count());
            Assert.AreEqual(0, f2_1.Stock.Count());

            // очередь производства не очищается
            Assert.AreEqual(1, f1_1.ProductionMaterials.Count());
            Assert.AreEqual(1, f2_1.ProductionMaterials.Count());

            // заключаем контракты
            Logic.AddContract(new Contract(Game.Time, c1,
                new MaterialWithPrice {Amount = 110000, Material = ReferenceData.GetMaterialByKey("ruda")})
            {
                DestinationFactory = f1_1
            });

            Logic.AddContract(new Contract(Game.Time, c2,
                new MaterialWithPrice {Amount = 120000, Material = ReferenceData.GetMaterialByKey("ruda")})
            {
                DestinationFactory = f2_1,
            });

            Logic.UpdateFactorySettings(f1_1, null, null, new List<Material> { ReferenceData.GetMaterialByKey("metall_zelezo_ruda") });
            Logic.UpdateFactorySettings(f2_1, null, null, new List<Material> { ReferenceData.GetMaterialByKey("electronic_kremnii_ruda") });

            RunCycles(10);

            // прокачались фабрики следующего поколения. покупаем их и начинаем организовывать производство
            Assert.AreEqual(2, c1.FactoryGenerationLevel);
            Assert.AreEqual(2, c2.FactoryGenerationLevel);

            Assert.AreEqual(194174.1m, c1.Sum);
            Assert.AreEqual(94575, c2.Sum);

            var stock = f1_1.Stock.OrderBy(m => m.Material.DisplayName).ToList();
            Assert.AreEqual(2, stock.Count);
            Assert.AreEqual(200000, stock[0].Amount);
            Assert.AreEqual(ReferenceData.GetMaterialByKey("metall_zelezo_ruda").Id, stock[0].Material.Id);
            Assert.AreEqual(100000, stock[1].Amount);
            Assert.AreEqual(ReferenceData.GetMaterialByKey("ruda").Id, stock[1].Material.Id);

            stock = f2_1.Stock.OrderBy(m => m.Material.DisplayName).ToList();
            Assert.AreEqual(2, stock.Count);
            Assert.AreEqual(250000, stock[0].Amount);
            Assert.AreEqual(ReferenceData.GetMaterialByKey("electronic_kremnii_ruda").Id, stock[0].Material.Id);
            Assert.AreEqual(200000, stock[1].Amount);
            Assert.AreEqual(ReferenceData.GetMaterialByKey("ruda").Id, stock[1].Material.Id);

            // закупаем фабрики
            var f1_2 = Logic.BuyFactoryFromGame(c1, ReferenceData.GetAvailFactoryDefenitions(c1).First());
            var f2_2 = Logic.BuyFactoryFromGame(c2, ReferenceData.GetAvailFactoryDefenitions(c2).First());
            Assert.AreEqual(191674.1m, c1.Sum);
            Assert.AreEqual(92075, c2.Sum);

            Logic.UpdateFactorySettings(f1_2, null, null,
                new List<Material> {ReferenceData.GetMaterialByKey("metall_zelezo")});
            Logic.UpdateFactorySettings(f2_2, null, null,
                new List<Material> {ReferenceData.GetMaterialByKey("electronic_kremnii")});

            // организовываем контракты уже между своими фабриками
            Logic.AddContract(new Contract(Game.Time, c1,
                new MaterialWithPrice {Amount = 20000, Material = ReferenceData.GetMaterialByKey("metall_zelezo_ruda")})
            {
                SourceFactory = f1_1, DestinationFactory = f1_2
            });

            Logic.AddContract(new Contract(Game.Time, c2,
                new MaterialWithPrice {Amount = 25000, Material = ReferenceData.GetMaterialByKey("electronic_kremnii_ruda")})
            {
                SourceFactory = f2_1, DestinationFactory = f2_2
            });

            // и начинаем продавать излишки игре, что бы заработать денежек
            Logic.AddContract(new Contract(Game.Time, c1,
                new MaterialWithPrice { Amount = 50000, Material = ReferenceData.GetMaterialByKey("metall_zelezo_ruda") })
            {
                SourceFactory = f1_1
            });

            Logic.AddContract(new Contract(Game.Time, c2,
                new MaterialWithPrice { Amount = 50000, Material = ReferenceData.GetMaterialByKey("electronic_kremnii_ruda") })
            {
                SourceFactory = f2_1
            });

            RunCycles(10);

            // деньги потихоньку начинаем зарабатывать, не смотря на то, что у нас есть новые фабрики
            Assert.AreEqual(190117.5125m, c1.Sum);
            Assert.AreEqual(91386.2m, c2.Sum);
            Assert.AreEqual(3, c1.FactoryGenerationLevel);
            Assert.AreEqual(3, c2.FactoryGenerationLevel);

            // руды у нас уже нет на складе, т.к. излишки продаём игре
            stock = f1_1.Stock.OrderBy(m => m.Material.DisplayName).ToList();
            Assert.AreEqual(1, stock.Count);
            Assert.AreEqual(200000, stock[0].Amount);
            Assert.AreEqual(ReferenceData.GetMaterialByKey("ruda").Id, stock[0].Material.Id);
            stock = f1_2.Stock.OrderBy(m => m.Material.DisplayName).ToList();
            Assert.AreEqual(2, stock.Count);
            Assert.AreEqual(200, stock[0].Amount);
            Assert.AreEqual(ReferenceData.GetMaterialByKey("metall_zelezo").Id, stock[0].Material.Id);
            Assert.AreEqual(20000, stock[1].Amount);
            Assert.AreEqual(ReferenceData.GetMaterialByKey("metall_zelezo_ruda").Id, stock[1].Material.Id);

            stock = f2_1.Stock.OrderBy(m => m.Material.DisplayName).ToList();
            Assert.AreEqual(2, stock.Count);
            Assert.AreEqual(0, stock[0].Amount);
            Assert.AreEqual(ReferenceData.GetMaterialByKey("electronic_kremnii_ruda").Id, stock[0].Material.Id);
            Assert.AreEqual(100000, stock[1].Amount);
            Assert.AreEqual(ReferenceData.GetMaterialByKey("ruda").Id, stock[1].Material.Id);
            stock = f2_2.Stock.OrderBy(m => m.Material.DisplayName).ToList();
            Assert.AreEqual(2, stock.Count);
            Assert.AreEqual(95000, stock[0].Amount);
            Assert.AreEqual(ReferenceData.GetMaterialByKey("electronic_kremnii_ruda").Id, stock[0].Material.Id);
            Assert.AreEqual(10000, stock[1].Amount);
            Assert.AreEqual(ReferenceData.GetMaterialByKey("electronic_kremnii").Id, stock[1].Material.Id);

            // покупаем фабрики третьего уровня
            var f1_3 = Logic.BuyFactoryFromGame(c1, ReferenceData.GetAvailFactoryDefenitions(c1).First());
            var f2_3 = Logic.BuyFactoryFromGame(c2, ReferenceData.GetAvailFactoryDefenitions(c2).First());

            Assert.AreEqual(185117.5125m, c1.Sum);
            Assert.AreEqual(86386.2m, c2.Sum);

            // тормозим исследования
            Logic.UpdateCustomerSettings(c1, 0);
            Logic.UpdateCustomerSettings(c2, 0);

            // команда №2 пока копит денег, а команда №1 запускает производство следующего уровня - начинает делать железные листы
            Logic.AddContract(new Contract(Game.Time, c1,
                new MaterialWithPrice { Amount = 50, Material = ReferenceData.GetMaterialByKey("metall_zelezo") })
            {
                SourceFactory = f1_2,
                DestinationFactory = f1_3
            });

            Logic.AddContract(new Contract(Game.Time, c1,
                new MaterialWithPrice { Amount = 10000, Material = ReferenceData.GetMaterialByKey("metall_zelezo_ruda") })
            {
                SourceFactory = f1_2
            });

            Logic.AddContract(new Contract(Game.Time, c1,
                new MaterialWithPrice { Amount = 10, Material = ReferenceData.GetMaterialByKey("metall_zelezo_list") })
            {
                SourceFactory = f1_3
            });

            Logic.UpdateFactorySettings(f1_3, null, null, new List<Material> { ReferenceData.GetMaterialByKey("metall_zelezo_list") });

            RunCycles(20);

            Assert.AreEqual(183205.125m, c1.Sum);
            Assert.AreEqual(82501.3m, c2.Sum);
        }
    }
}