import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { Component, OnInit } from '@angular/core';
import { TeamsService } from '../services/teams.service';
import { Team } from '../models/team';
import { NewTeamComponent } from '../new-team/new-team.component';

@Component({
    selector: 'app-teams',
    templateUrl: './teams.component.html',
    styleUrls: ['./teams.component.scss']
})
export class TeamsComponent implements OnInit {

    displayedColumns: string[] = ['name', 'productionType', 'sum'];
    teams: Team[];

    constructor(private serv: TeamsService, private router: Router, private dialog: MatDialog) { }
    ngOnInit() {
        this.loadTeams();
    }

    loadTeams() {
        this.serv.getTeams().subscribe(data => this.teams = data);
    }

    addNewTeam(): void {
        this.dialog.open(NewTeamComponent);
    }

    //Переход на страницу с детальной информацией о команде.
    goTeamDetails(team: Team) {
        this.router.navigate(
            ['teamDetails'],
            {
                queryParams: {
                    'name': team.name,
                    'productionType': team.productionType,
                    'factories': team.factories,
                    'sum': team.sum,
                    'contracts': team.contracts,
                }
            }
        );
    }
}
