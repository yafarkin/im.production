import { Component, OnInit, Input } from '@angular/core';
import { TeamService } from '../services/team.service';
import { FactoryDto } from '../models/factory.dto';
import { TeamProgressDto } from '../models/team.progress.dto';

@Component({
    selector: 'app-factories',
    templateUrl: './factories.component.html',
    styleUrls: ['./factories.component.scss'],
    providers: [TeamService]
})
export class FactoriesComponent implements OnInit {

    @Input() teamProgress: TeamProgressDto;

    login: string = "CustomerLogin1";
    factories: FactoryDto[];
    factoriesShowAdditionalInfo: boolean[] = [];

    constructor(private teamService: TeamService) { }

    ngOnInit() {
        this.teamService.getFactories(this.login).subscribe(
            success => {
                this.factories = success;
            }
        );
    }

    setFactoriesShowAdditionalInfo(index: number): void {
        this.factoriesShowAdditionalInfo[index] = !this.factoriesShowAdditionalInfo[index];
    }

    getFactoriesShowAdditionalInfo(index: number): boolean {
        return this.factoriesShowAdditionalInfo[index];
    }

}
