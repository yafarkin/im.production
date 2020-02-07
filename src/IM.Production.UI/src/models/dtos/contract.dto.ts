export class ContractDto {
    /// Если указано, то контракт действует до указанной даты.
    public tillDate: number;
    /// Если указано, то контракт действует до поставки определенного количества материала.
    public tillCount: number;
    /// Общая сумма на закупку/продажу, прошедшую по контракту.
    public totalSumm: number;
    /// Исходная фабрика, если не задано - поставляется игрой.
    public sourceFactoryCustomerLogin: string;
    /// Фабрика назначения, если не задано - продается игре.
    public destinationFactoryCustomerLogin: string;
}
