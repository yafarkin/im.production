export class ContractDto {
    /// Общая сумма на закупку/продажу, прошедшую по контракту.
    public totalSumm: number;
    /// Количество материала, уже поставленное по контракту.
    public totalCountComplete: number;
    /// Если указано, то контракт действует до поставки определенного количества материала.
    public tillCount: number;
    /// Если указано, то контракт действует до указанной даты.
    public tillDate: number;

    /// Source Cusomer info
    public sourceCustomerLogin: string;
    public sourceFactoryName: string;
    public sourceGenerationLevel: number;
    public sourceWorkers: number;

    /// Destination Cusomer info
    public destinationCustomerLogin: string;
    public destinationFactoryName: string;
    public destinationGenerationLevel: number;
    public destinationWorkers: number;
}