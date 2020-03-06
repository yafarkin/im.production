import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Md5 } from 'ts-md5/dist/md5';
import { NewTeamDto } from '../models/newteam.dto';
import { TeamsService } from '../services/teams.service';

@Component({
    selector: 'app-new-team',
    templateUrl: './new-team.component.html',
    styleUrls: ['./new-team.component.scss']
})
export class NewTeamComponent implements OnInit {
    //TODO Extract to a method
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

    constructor(private teamsService: TeamsService, private snackBar: MatSnackBar) { }

    ngOnInit() {
    }

    addNewTeam(): void {
        let team: NewTeamDto = new NewTeamDto();
        team.name = this.gameGroup.value.teamName;
        team.login = this.gameGroup.value.teamLogin;
        team.passwordHash = Md5.hashStr(this.gameGroup.value.teamPassword, false).toString();
        this.teamsService.addTeam(team).subscribe(
            success => {
                this.snackBar.open("Команда добавлена!", "", {
                    duration: 3 * 1000
                });
            },
            error => {
                this.snackBar.open("Команда не добавлена!", "", {
                    duration: 3 * 1000
                });
            }
        );
    }

}
