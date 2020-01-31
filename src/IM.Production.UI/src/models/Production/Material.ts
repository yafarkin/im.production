import { ProductionType } from './ProductionType';
import { MaterialOnStock } from './MaterialOnStock';

export class Material {
    constructor(
        public key: string,
        public productionType: ProductionType,
        public inputMaterials: MaterialOnStock[],
        public amountPerDay: number
    )
    {
    }
}