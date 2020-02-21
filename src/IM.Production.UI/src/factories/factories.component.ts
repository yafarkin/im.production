import { Component, OnInit, Input } from '@angular/core';
import { FactoryDto } from '../models/dtos/factory.dto';
import { TeamService } from '../services/team.service';
import { FactoryAndContractFactoriesDto } from '../models/dtos/factory.and.contract.factories.dto';
import { GameProgressDto } from '../models/dtos/game.progress.dto';

@Component({
    selector: 'app-factories',
    templateUrl: './factories.component.html',
    styleUrls: ['./factories.component.scss'],
    providers: [TeamService]
})
export class FactoriesComponent implements OnInit {

    @Input() gameProgress: GameProgressDto;

    login: string = "CustomerLogin1";
    factoriesAndContractFactories: FactoryAndContractFactoriesDto[];
    contractFactories: FactoryDto[][];
    factoriesShowAdditionalInfo: boolean[] = [];

    constructor(private teamService: TeamService) { }

    ngOnInit() {
        this.teamService.getFactories(this.login).subscribe(
            success => {
                this.factoriesAndContractFactories = success;
            }
        );
    }

    setFactoriesShowAdditionalInfo(index: number): void {
        this.factoriesShowAdditionalInfo[index] = !this.factoriesShowAdditionalInfo[index];
    }

    getFactoriesShowAdditionalInfo(index: number): boolean {
        return this.factoriesShowAdditionalInfo[index];
    }

    getFactoryProductionType(contractFactory: FactoryDto): string {
        if (contractFactory != null) {
            return contractFactory.productionTypeKey;
        }
        return "default";
    }

}
