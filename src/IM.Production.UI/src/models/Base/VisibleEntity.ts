import { BaseEntity } from "./BaseEntity";
import { IVisibleEntity } from "./IVisibleEntity";

export class VisibleEntity extends BaseEntity implements IVisibleEntity {
    constructor(
        public displayName: string,
        public id: any
    )
    {
        super(id);
    }
}
