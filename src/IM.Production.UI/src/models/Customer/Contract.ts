import { MaterialLogistic } from '../Production/MaterialLogistic';
import { MaterialWithPrice } from '../Production/MaterialWithPrice';
import { Factory } from '../Production/Factory';
import { TaxFactoryChange } from '../Production/TaxFactoryChange';

export class Contract extends MaterialLogistic {
    constructor(
        //only for contract table presentation
        public position: number,

        /// Штрафы за не поставку материала, за каждую единицу, если предусмотрено.
        public fine: number,
        
        /// Сумма страховой премии (производителю), за не поставку материала, если предусмотрено.
        public srcInsurancePremium: number,
        
        /// Страховая выплата (производителю), если материал не был поставлен, если предусмотрено.
        public srcInsuranceAmount: number,
        
        /// Сумма страховой премии (получателю), за не поставку материала, если предусмотрено.
        public destInsurancePremium: number,
        
        /// Страховая выплата (получателю), если материал не был поставлен, если предусмотрено.
        public destInsuranceAmount: number,
        
        /// Если указано, то контракт действует до указанной даты.
        public tillDate: number,
        
        /// Если указано, то контракт действует до поставки определенного количества материала.
        public tillCount: number,

        /// Количество материала, уже поставленное по контракту.
        public totalCountCompleted: number,
        
        /// Общая сумма на закупку/продажу, прошедшую по контракту.
        public totalSumm: number,
        
        /// Общая сумма уплаченных налогов по контракту.
        public totalOnTaxes: number,

        public sourceFactory: Factory,
        public destinationFactory: Factory,
        public materialWithPrice: MaterialWithPrice,
        public tax: TaxFactoryChange)
    {
        /*
            constructor(
                SourceFactory: Factory,
                DestinationFactory: Factory,
                MaterialWithPrice: MaterialWithPrice,
                Tax: TaxFactoryChange
            )
        */
        super(sourceFactory, destinationFactory, materialWithPrice, tax);
    }
}