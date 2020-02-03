import { BaseChanging } from "../Base/BaseChanging";
import { IVisibleEntity } from "../Base/IVisibleEntity";
import { GameTime } from "../Base/GameTime";
import { Customer } from "../Customer/Customer";

export abstract class BaseBank extends BaseChanging implements IVisibleEntity {
    constructor(
        public displayName: string,
        public customer: Customer,
        public description: string,
        public time: GameTime,
        public id: any
    )
    {
        super(time, customer, description, id);
    }
}
