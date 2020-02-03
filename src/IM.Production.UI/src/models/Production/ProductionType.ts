import { BaseProduction } from "./BaseProduction";
import { IVisibleEntity } from "../Base/IVisibleEntity";

export class ProductionType extends BaseProduction implements IVisibleEntity {
    constructor(
        public key: string,
        public displayName: string,
        public id: any
    )
    {
        super(id);
    }
}
