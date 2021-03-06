import { MatDialog } from '@angular/material/dialog';
import { Component, OnInit } from '@angular/core';
import { TeamsService } from '../services/teams.service';
import { NewTeamComponent } from '../new-team/new-team.component';
import { TeamDto } from '../models/team.dto';

@Component({
    selector: 'app-teams',
    templateUrl: './teams.component.html',
    styleUrls: ['./teams.component.scss']
})
export class TeamsComponent implements OnInit {

    displayedColumns: string[] = ['name', 'productionType', 'sum'];
    teams: TeamDto[];

    constructor(private teamService: TeamsService, private dialog: MatDialog) { }
    ngOnInit() {
        this.loadTeams();
    }

    loadTeams() {
        this.teamService.getTeams().subscribe(data => this.teams = data);
    }

    addNewTeam(): void {
        const dialog = this.dialog.open(NewTeamComponent);
        dialog.afterClosed().subscribe(
            success => {
                this.loadTeams();
            }
        );
    }
}
