import { ProductionType } from '../Production/ProductionType';
import { Factory } from '../Production/Factory';
import { Contract } from './Contract';
import { BankFinOperation } from '../Bank/BankFinOperation';
import { VisibleEntity } from '../Base/VisibleEntity';

export class Customer extends VisibleEntity {
    constructor(
        public login: string,
        public passwordHash: string,
        public productionType: ProductionType,
        public factories:  Factory[], //Factory[]
        private _factories: Factory[], //Factory[]
        public contracts: Contract[], //Contract[]
        private _contracts: Contract[], //Contract[]
        public bankFinanceOperations: BankFinOperation[],
        private _bankFinanceOperations: BankFinOperation[], //BankFinOperation[]
        public factoryGenerationLevel: number,
        public sumOnRD: number,
        public sumToNextGeneration: number,
        public spentSumToNextGenerationLevel: number,
        public readyForNextGenerationLevel: boolean,
        public rDProgress: number, 
        public sum: number,
        public displayName: string,
        public id: any
    )
    {
        super(displayName, id);
    }
}