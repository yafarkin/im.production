using System;
using System.Collections.Generic;
using System.Linq;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain.ReferenceData
{
    /// <summary>
    /// Класс хранит начальные данные об игре и сериализуется в json-файл.
    /// </summary>
    public class InitialData
    {
        /// <summary>
        /// Области производства.
        /// </summary>
        public List<ProductionType> ProductionTypes { get; set; }

        /// <summary>
        /// Общий список материалов в игре.
        /// </summary>
        public List<Material> Materials { get; set; }

        /// <summary>
        /// Определения фабрик, которые есть в игре.
        /// </summary>
        public List<FactoryDefinition> FactoryDefinitions { get; set; }

        /// <summary>
        /// Какие материалы игра поставляет изначально.
        /// </summary>
        public GameSupply Supply { get; set; }

        /// <summary>
        /// Какие ресурсы игра покупает и по какой цене.
        /// </summary>
        public GameDemand Demand { get; set; }

        /// <summary>
        /// Налог на продажу материалов.
        /// </summary>
        public readonly List<TaxOnMaterial> MaterialTaxes = new List<TaxOnMaterial>();

        /// <summary>
        /// Налог на фабрики.
        /// </summary>
        public readonly List<TaxOnFactory> FactoryTaxes = new List<TaxOnFactory>();

        /// <summary>
        /// Справочные данные по производительности фабрики, если количество рабочих больше чем нужно.
        /// </summary>
        /// <remarks>Key - насколько рабочих больше чем нужно; Value - на сколько измениться общая производительность фабрики.</remarks>
        public readonly List<KeyValuePair<decimal, decimal>> FactoryOverPerformance = new List<KeyValuePair<decimal, decimal>>
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
        public readonly List<KeyValuePair<int, decimal>> DefaultFactoryCost =
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
        public readonly List<KeyValuePair<Guid, decimal>> FactoryCost = new List<KeyValuePair<Guid, decimal>>();

        /// <summary>
        /// Стоимость исследования следующего поколения фабрики.
        /// </summary>
        /// <remarks>По умолчанию первое поколение уже исследовано. Key - уровень поколения, Value - стоимость исследования.</remarks>
        public readonly List<KeyValuePair<int, decimal>> GenerationFactoryRDCost = new List<KeyValuePair<int, decimal>>
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
        public readonly List<KeyValuePair<int, decimal>> FactoryLevelUpRDCost =
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
        public decimal BaseWorkerSalay = 100m;

        /// <summary>
        /// Налог на фабрику по умолчанию.
        /// </summary>
        public decimal DefaultFactoryTax = 0.1m;

        /// <summary>
        /// Налог на продажу единицы материала по умолчанию.
        /// </summary>
        public decimal DefaultMaterialTax = 0.01m;
    }
}
