export class Contract {
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
        public totalOnTaxes: number)
    {
    }
}