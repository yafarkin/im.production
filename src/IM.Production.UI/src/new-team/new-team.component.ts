import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NewTeamDto } from '../models/dtos/newteam.dto';
import { TeamsService } from '../services/teams.service';

import { Md5 } from 'ts-md5/dist/md5';

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
        let team: NewTeamDto = new NewTeamDto();
        team.displayName = this.gameGroup.value.teamName;
        team.login = this.gameGroup.value.teamLogin;
        team.passwordHash = Md5.hashStr(this.gameGroup.value.teamPassword, false).toString();
        console.log("team.displayName: " + team.displayName);
        console.log("team.login: " + team.login);
        console.log("team.passwordHash: " + team.passwordHash);
        this.teamsService.addTeam(team).subscribe(
            success => {
                console.log("[success]");
            }
        );
    }

}
