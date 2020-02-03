import { OperationStatus } from './OperationStatus.enum';
import { BaseBank } from './BaseBank';
import { GameTime } from '../Base/GameTime';
import { Customer } from '../Customer/Customer';

export abstract class BankFinOperation extends BaseBank {
    constructor(
        public sum: number,
        public percent: number,
        public days: number,
        public status: OperationStatus,
        public displayName: string,
        public time: GameTime,
        public customer: Customer,
        public description: string,
        public id: any
    )
    {
        super(displayName, customer, description, time, id);
    }
}