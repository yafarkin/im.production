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
            _stock = new List<MaterialOnStock>();
            _productionMaterials = new List<Material>();
            Level = 1;
        }

        /// <summary>
        /// Ссылка на команду, которой принадлежит фабрика.
        /// </summary>
        public Customer Customer { get; internal set; }

        /// <summary>
        /// Ссылка на описание фабрики.
        /// </summary>
        public FactoryDefinition FactoryDefinition { get; protected set; }

        /// <summary>
        /// Текущий уровень модернизации фабрики.
        /// </summary>
        public int Level { get; internal set; }

        /// <summary>
        /// Производимые материалы на этой фабрике (из подмножества <see><cref>FactoryDefinition.CanProductionMaterials</cref> </see>).
        /// </summary>
        protected IList<Material> _productionMaterials { get; set; }

        public IEnumerable<Material> ProductionMaterials => _productionMaterials;

        /// <summary>
        /// Текущий уровень производительности.
        /// </summary>
        public decimal Performance { get; internal set; }

        /// <summary>
        /// Актуальное количество рабочих на фабрике в настоящий момент (не может быть меньше 1).
        /// </summary>
        public int Workers { get; internal set; }

        /// <summary>
        /// Сумма, выделяемая в игровой день на исследования.
        /// </summary>
        public decimal SumOnRD { get; internal set; }

        /// <summary>
        /// Требуемая сумма для открытия следующего уровня производительности фабрики.
        /// </summary>
        public decimal NeedSumToNextLevelUp { get; internal set; }

        /// <summary>
        /// Уже потраченная сумма на исследования следующего уровня производительности фабрики.
        /// </summary>
        public decimal SpentSumToNextLevelUp { get; internal set; }

        /// <summary>
        /// Прогресс в процентах для исследования на следующий уровень.
        /// </summary>
        public decimal RDProgress => NeedSumToNextLevelUp != 0 ? SpentSumToNextLevelUp / NeedSumToNextLevelUp : 0;

        public bool ReadyForNextLevel => SpentSumToNextLevelUp >= NeedSumToNextLevelUp;

        /// <summary>
        /// Всего потрачено на RD.
        /// </summary>
        public decimal TotalOnRD { get; internal set; }

        /// <summary>
        /// Всего потрачено на налоги.
        /// </summary>
        public decimal TotalOnTaxes { get; internal set; }

        /// <summary>
        /// Всего потрачено на зарплату.
        /// </summary>
        public decimal TotalOnSalary { get; internal set; }

        /// <summary>
        /// Склад материалов на фабрике.
        /// </summary>
        internal IList<MaterialOnStock> _stock { get; set; }

        public IEnumerable<MaterialOnStock> Stock => _stock;

        /// <summary>
        /// Отображаемое в интерфейсе описание.
        /// </summary>
        public string DisplayName => $"Фабрика {FactoryDefinition.Name} (L {FactoryDefinition.GenerationLevel}, W {Workers}, P {Performance:P})";

        internal void SetProductionMaterials(IEnumerable<Material> materials) =>
            _productionMaterials = new List<Material>(materials);

        public static Factory CreateFactory(Customer customer, FactoryDefinition factoryDefinition)
        {
            var factory = new Factory
            {
                Customer = customer,
                FactoryDefinition = factoryDefinition,
                _productionMaterials = new List<Material>()
            };

            return factory;
        }
    }
}