using System;
using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Конкретная фабрика у команды.
    /// </summary>
    [Serializable]
    public class Factory : BaseProduction, IVisibleEntity
    {
        public Factory()
        {
            Stock = new List<MaterialOnStock>();
            ProductionMaterials = new List<Material>();
            Level = 1;
        }

        /// <summary>
        /// Ссылка на команду, которой принадлежит фабрика.
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Ссылка на описание фабрики.
        /// </summary>
        public FactoryDefinition FactoryDefinition { get; set; }

        /// <summary>
        /// Текущий уровень модернизации фабрики.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Производимые материалы на этой фабрике (из подмножества <see><cref>FactoryDefinition.CanProductionMaterials</cref> </see>).
        /// </summary>
        public List<Material> ProductionMaterials { get; set; }

        /// <summary>
        /// Текущий уровень производительности.
        /// </summary>
        public decimal Performance { get; set; }

        /// <summary>
        /// Актуальное количество рабочих на фабрике в настоящий момент (не может быть меньше 1).
        /// </summary>
        public int Workers { get; set; }

        /// <summary>
        /// Сумма, выделяемая в игровой день на исследования.
        /// </summary>
        public decimal SumOnRD { get; set; }

        /// <summary>
        /// Требуемая сумма для открытия следующего уровня производительности фабрики.
        /// </summary>
        public decimal NeedSumToNextLevelUp { get; set; }

        /// <summary>
        /// Уже потраченная сумма на исследования следующего уровня производительности фабрики.
        /// </summary>
        public decimal SpentSumToNextLevelUp { get; set; }

        /// <summary>
        /// Прогресс в процентах для исследования на следующий уровень.
        /// </summary>
        public decimal RDProgress => SpentSumToNextLevelUp / NeedSumToNextLevelUp;

        public bool ReadyForNextLevel => SpentSumToNextLevelUp >= NeedSumToNextLevelUp;

        /// <summary>
        /// Склад материалов на фабрике.
        /// </summary>
        public List<MaterialOnStock> Stock { get; set; }

        /// <summary>
        /// Отображаемое в интерфейсе описание.
        /// </summary>
        public string DisplayName => $"Фабрика {FactoryDefinition.Name} (L {FactoryDefinition.GenerationLevel}, W {Workers}, P {Performance:P})";
    }
}