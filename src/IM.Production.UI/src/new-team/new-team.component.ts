import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CustomerDto } from '../models/dtos/customer.dto';
import { TeamsService } from '../services/teams.service';

@Component({
    selector: 'app-new-team',
    templateUrl: './new-team.component.html',
    styleUrls: ['./new-team.component.scss'],
    providers: [TeamsService]
})
export class NewTeamComponent implements OnInit {
    gameGroup = new FormGroup({
        teamName: new FormControl('', [
            Validators.required, Validators.minLength(3), Validators.maxLength(30)
        ]),
        teamLogin: new FormControl('', [
            Validators.required, Validators.minLength(3), Validators.maxLength(30)
        ]),
        teamPassword: new FormControl('', [
            Validators.required, Validators.pattern("[0-9]*"), Validators.minLength(3), Validators.maxLength(30)
        ])
    });

    constructor(private teamsService: TeamsService) { }

    ngOnInit() {
    }

    addNewTeam(): void {
        console.log("Add new team!");
        let team: CustomerDto = new CustomerDto();
        team.name = this.gameGroup.value.teamName;
        team.login = this.gameGroup.value.teamLogin;
        team.password = this.gameGroup.value.teamPassword;
        console.log("team.name: " + team.name);
        console.log("team.login: " + team.login);
        console.log("team.passwordHash: " + team.password);
        this.teamsService.addTeam(team).subscribe(
            success => {
                console.log("[success]");
            }
        );
    }

}
