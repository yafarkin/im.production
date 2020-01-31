import { ProductionType } from '../Production/ProductionType';
import { Factory } from '../Production/Factory';
import { Contract } from './Contract';
import { BankFinOperations } from '../Bank/BankFinOperations';

export class Customer {
    constructor(
        public login: string,
        public passwordHash: string,
        public productionType: ProductionType,
        public factories:  any, //Factory[]
        private _factories: any, //Factory[]
        public contracts: any, //Contract[]
        private _contracts: any, //Contract[]
        public bankFinanceOperations: BankFinOperations,
        private _bankFinanceOperations: any, //BankFinOperation[]
        public factoryGenerationLevel: number,
        public sumOnRD: number,
        public sumToNextGeneration: number,
        public spentSumToNextGenerationLevel: number,
        public readyForNextGenerationLevel: boolean,
        public rDProgress: number,
        public sum: number
    )
    {
    }
}