import { ProductionType } from "./production.type.enum";

export class FactoryDefinition {
    public level: number;
    public workersCount: number;
    public cost: number;
    public productionType: ProductionType;
}
