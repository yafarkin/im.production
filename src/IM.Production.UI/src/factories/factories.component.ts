import { Component, OnInit, Input } from '@angular/core';
import { FactoryDto } from '../models/dtos/factory.dto';
import { TeamService } from '../services/team.service';
import { FactoryAndContractFactoriesDto } from '../models/dtos/factory.and.contract.factories.dto';

@Component({
    selector: 'app-factories',
    templateUrl: './factories.component.html',
    styleUrls: ['./factories.component.scss'],
    providers: [TeamService]
})
export class FactoriesComponent implements OnInit {

    login: string = "CustomerLogin1";
    factoriesAndContractFactories: FactoryAndContractFactoriesDto[];
    contractFactories: FactoryDto[][];
    factoriesShowAdditionalInfo: boolean[] = [];

    constructor(private teamService: TeamService) { }

    ngOnInit() {

        this.teamService.getFactories(this.login).subscribe(
            success => {
                this.factoriesAndContractFactories = success;
                this.factoriesAndContractFactories.forEach(element => {
                    this.factoriesShowAdditionalInfo.push(true);
                });
            }
        );
    }

    setFactoriesShowAdditionalInfo(index: number): void {
        this.factoriesShowAdditionalInfo[index] = !this.factoriesShowAdditionalInfo[index];
    }

    getFactoriesShowAdditionalInfo(index: number): boolean {
        return this.factoriesShowAdditionalInfo[index];
    }

    isProductionTypeMetall(index: number) {
        return this.factoriesAndContractFactories[index].factory.productionTypeKey == "metall" || true;
    }

    isProductionTypeOil(index: number) {
        return this.factoriesAndContractFactories[index].factory.productionTypeKey == "neft_gaz";
    }

    isProductionTypeElectronic(index: number) {
        return this.factoriesAndContractFactories[index].factory.productionTypeKey == "electronic";
    }

    isProductionTypeWooden(index: number) {
        return this.factoriesAndContractFactories[index].factory.productionTypeKey == "derevo";
    }

}
