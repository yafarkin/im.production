import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { OnInit } from '@angular/core';
import { Team } from '../models/team';
import { TeamsService } from '../services/teams.service';

@Component({
    selector: 'app-teams',
    templateUrl: './teams.component.html',
    styleUrls: ['./teams.component.scss']
})

export class TeamsComponent implements OnInit {

    displayedColumns: string[] = ['name', 'productionType', 'factories', 'sum', 'contracts'];
    teams: Team[];

    constructor(private serv: TeamsService, private router: Router) { }
    ngOnInit() {
        this.loadTeams();
    }

    loadTeams() {
        this.serv.getTeams().subscribe(data => this.teams = data);
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
