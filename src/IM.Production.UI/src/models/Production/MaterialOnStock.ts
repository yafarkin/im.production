import { Material } from './Material';

export class MaterialOnStock {
    constructor(
        public amount: number,
        public material: Material
    )
    {
    }
}