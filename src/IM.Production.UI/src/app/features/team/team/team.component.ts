import { Component, OnInit } from '@angular/core';
import { TeamService } from '../services/team.service';
import { TeamProgressDto } from '../models/team.progress.dto';

@Component({
    selector: 'app-team',
    templateUrl: './team.component.html',
    styleUrls: ['./team.component.scss'],
    providers: [TeamService]
})
export class TeamComponent implements OnInit {

    teamProgress: TeamProgressDto;
    constructor(private teamService: TeamService) { }

    ngOnInit() {
        this.teamService.getTeamProgress("CustomerLogin1").subscribe(
            success => {
                this.teamProgress = success;
            }
        );
    }

}
