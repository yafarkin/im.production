import { OperationStatus } from './OperationStatus.enum';

export class BankFinOperations {
    constructor(
        public sum: number,
        public percent: number,
        public days: number,
        public status: OperationStatus
    )
    {
    }
}