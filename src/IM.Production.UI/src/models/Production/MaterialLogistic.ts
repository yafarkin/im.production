import { Factory } from './Factory';
import { MaterialWithPrice } from './MaterialWithPrice';
import { TaxFactoryChange } from './TaxFactoryChange';
import { BaseChanging } from '../Base/BaseChanging';

export class MaterialLogistic extends BaseChanging {
    constructor(
        public sourceFactory: Factory,
        /// <summary>
        /// Фабрика назначения, если не задано - продается игре.
        /// </summary>
        public destinationFactory: Factory,
        /// <summary>
        /// Конкретный материал с указанием цены (за единицу) и количества.
        /// </summary>
        public materialWithPrice: MaterialWithPrice,
        /// <summary>
        /// Ссылка на списание налога за поставку материала.
        /// </summary>
        public tax: TaxFactoryChange,
        public id: any
    )
    {
        super(null, null, null, id);
    }
}