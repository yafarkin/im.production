import { Component, OnInit, Input } from '@angular/core';
import { TeamService } from '../services/team.service';
import { FactoryDto } from '../models/factory.dto';
import { TeamProgressDto } from '../models/team.progress.dto';
import { AuthenticationService } from '../../../services/authentication.service';

@Component({
    selector: 'app-factories',
    templateUrl: './factories.component.html',
    styleUrls: ['./factories.component.scss'],
    providers: [TeamService, AuthenticationService]
})
export class FactoriesComponent implements OnInit {

    @Input() teamProgress: TeamProgressDto;

    factories: FactoryDto[];
    factoriesShowAdditionalInfo: boolean[] = [];

    constructor(private teamService: TeamService, private authService: AuthenticationService) { }

    ngOnInit() {
        let login = this.authService.currentUser.login;
        this.teamService.getFactories(login).subscribe(
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
