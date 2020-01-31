import { Customer } from '../Customer/Customer';
import { FactoryDefinition } from './FactoryDefinition';
import { Material } from './Material';
import { MaterialOnStock } from './MaterialOnStock';

export class Factory {
    constructor(
        /// <summary>
        /// Ссылка на команду, которой принадлежит фабрика.
        /// </summary>
        public customer: Customer,
    
        /// <summary>
        /// Ссылка на описание фабрики.
        /// </summary>
        public factoryDefinition: FactoryDefinition,
    
        /// <summary>
        /// Текущий уровень модернизации фабрики.
        /// </summary>
        public level: number,
    
        /// <summary>
        /// Производимые материалы на этой фабрике (из подмножества <see><cref>FactoryDefinition.CanProductionMaterials</cref> </see>).
        /// </summary>
        public productionMaterials: any, //Material[]
        private _productionMaterials: any, //Material[]

        /// <summary>
        /// Текущий уровень производительности.
        /// </summary>
        public performance: number,
    
        /// <summary>
        /// Актуальное количество рабочих на фабрике в настоящий момент (не может быть меньше 1).
        /// </summary>
        public workers: number,
    
        /// <summary>
        /// Сумма, выделяемая в игровой день на исследования.
        /// </summary>
        public SumOnRD: number,
    
        /// <summary>
        /// Требуемая сумма для открытия следующего уровня производительности фабрики.
        /// </summary>
        public needSumToNextLevelUp: number,
    
        /// <summary>
        /// Уже потраченная сумма на исследования следующего уровня производительности фабрики.
        /// </summary>
        public spentSumToNextLevelUp: number,
    
        /// <summary>
        /// Прогресс в процентах для исследования на следующий уровень.
        /// </summary>
        public rDProgress: number,
    
        public readyForNextLevel: boolean,
    
        /// <summary>
        /// Всего потрачено на RD.
        /// </summary>
        public totalOnRD: number,
    
        /// <summary>
        /// Всего потрачено на налоги.
        /// </summary>
        public totalOnTaxes: number,
    
        /// <summary>
        /// Всего потрачено на зарплату.
        /// </summary>
        public totalOnSalary: number,
    
        /// <summary>
        /// Всего затраты по фабрике.
        /// </summary>
        public totalExpenses: number,
    
        /// <summary>
        /// Склад материалов на фабрике.
        /// </summary>
        public stock: any, // MaterialOnStock[]
        private _stock: any, // MaterialOnStock[]
        
        public displayName: string
    )
    {
    }
}