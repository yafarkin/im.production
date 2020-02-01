import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { Team } from '../models/team';
import { TeamsService } from '../services/teams.service';
import { DataSource } from '@angular/cdk/table';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-teams',
    templateUrl: './teams.component.html',
    styleUrls: ['./teams.component.scss']
})

export class TeamsComponent implements OnInit {

    displayedColumns: string[] = ['name', 'productionType', 'factories', 'sum', 'contracts'];
    teams: Team[];

    constructor(private serv: TeamsService) { }
    ngOnInit() {
        this.loadTeams();
    }

    loadTeams() {
        this.serv.getTeams().subscribe(data => this.teams = data);
    }
}
