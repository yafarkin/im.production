import { Factory } from './Factory';
import { MaterialWithPrice } from './MaterialWithPrice';
import { TaxFactoryChange } from './TaxFactoryChange';
import { BaseChanging } from '../Base/BaseChanging';

export class MaterialLogistic extends BaseChanging {
    constructor(
        public SourceFactory: Factory,
        /// <summary>
        /// Фабрика назначения, если не задано - продается игре.
        /// </summary>
        public DestinationFactory: Factory,
        /// <summary>
        /// Конкретный материал с указанием цены (за единицу) и количества.
        /// </summary>
        public MaterialWithPrice: MaterialWithPrice,
        /// <summary>
        /// Ссылка на списание налога за поставку материала.
        /// </summary>
        public Tax: TaxFactoryChange
    )
    {
        super(null, null, null);
    }
}