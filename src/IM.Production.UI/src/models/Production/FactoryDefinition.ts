import { ProductionType } from '../Production/ProductionType';

export class FactoryDefinition {
    constructor(
        /// <summary>
        /// Тип производства.
        /// </summary>
        public productionType: ProductionType,
        /// <summary>
        /// Что умеет производить фабрика.
        /// </summary>
        /// <remarks>Ключ - уровень производства фабрики, Value - что фабрика умеет производить на этому уровне.</remarks>
        public canProductionMaterials: any,
        /// <summary>
        /// Базовое количество рабочих на фабрике, что бы она имела 100% производительность.
        /// </summary>
        public baseWorkers: number,
        /// <summary>
        /// Уровень поколения фабрики.
        /// </summary>
        public generationLevel: number,
        /// <summary>
        /// Название производства.
        /// </summary>
        public name: string
    )
    {
    }
}