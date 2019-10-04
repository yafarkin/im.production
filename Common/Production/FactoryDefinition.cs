using System.Collections.Generic;
using Epam.ImitationGames.Production.Common.Base;

namespace Epam.ImitationGames.Production.Common.Production
{
    /// <summary>
    /// Общее описание фабрики.
    /// </summary>
    public class FactoryDefinition : BaseProduction, IVisibleEntity
    {
        /// <summary>
        /// Тип производства.
        /// </summary>
        public ProductionType ProductionType { get; set; }

        /// <summary>
        /// Что умеет производить фабрика.
        /// </summary>
        /// <remarks>Ключ - уровень производства фабрики, Value - что фабрика умеет производить на этому уровне.</remarks>
        public Dictionary<int, List<Material>> CanProductionMaterials { get; set; }

        /// <summary>
        /// Базовое количество рабочих на фабрике, что бы она имела 100% производительность.
        /// </summary>
        public int BaseWorkers { get; set; }

        /// <summary>
        /// Уровень поколения фабрики.
        /// </summary>
        public int GenerationLevel { get; set; }

        /// <summary>
        /// Название производства.
        /// </summary>
        public string Name { get; set; }

        public string DisplayName => $"Описание производства {Name}, область {ProductionType.DisplayName}, поколение {GenerationLevel}, базовое количество рабочих {BaseWorkers}";
    }
}