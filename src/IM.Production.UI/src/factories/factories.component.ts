import { Component, OnInit, Input } from '@angular/core';
import { FactoryDto } from '../models/dtos/factory.dto';
import { TeamService } from '../services/team.service';

@Component({
    selector: 'app-factories',
    templateUrl: './factories.component.html',
    styleUrls: ['./factories.component.scss'],
    providers: [TeamService]
})
export class FactoriesComponent implements OnInit {

    login: string = "CustomerLogin1";
    factories: FactoryDto[];
    contractFactories: FactoryDto[][];
    factoriesShowAdditionalInfo: boolean[] = [];

    constructor(private teamService: TeamService) { }

    ngOnInit() {
        this.teamService.getFactories(this.login).subscribe(
            success => {
                this.factories = success;
            }
        );
        this.teamService.getContractFactories(this.login).subscribe(
            success => {
                this.contractFactories = success;
            }
        );
    }

    setFactoriesShowAdditionalInfo(index: number): void {
        this.factoriesShowAdditionalInfo[index]= !this.factoriesShowAdditionalInfo[index];
    }

    getFactoriesShowAdditionalInfo(index: number): boolean {
        return this.factoriesShowAdditionalInfo[index];
    }

    isProductionTypeMetall(index: number) {
        return this.factories[index].productionTypeKey == "metall" || true;
    }

    isProductionTypeOil(index: number) {
        return this.factories[index].productionTypeKey == "neft_gaz";
    }

    isProductionTypeElectronic(index: number) {
        return this.factories[index].productionTypeKey == "electronic";
    }

    isProductionTypeWooden(index: number) {
        return this.factories[index].productionTypeKey == "derevo";
    }

}
