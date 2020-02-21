export class FactoryContractDto {
    /// Source Cusomer info
    public sourceCustomerLogin: string;
    /// Destination Cusomer info
    public destinationCustomerLogin: string;
    //Имя материала человеческим языком
    public materialKey: string;
    /// Количество материала, уже поставленное по контракту.
    public totalCountCompleted: number;
    /// Общая сумма на закупку/продажу, прошедшую по контракту.
    public totalSumm: number;
}
