import { Component, OnInit } from '@angular/core';
import { TeamService } from '../services/team.service';
import { GameProgressDto } from '../models/dtos/game.progress.dto';

@Component({
    selector: 'app-team',
    templateUrl: './team.component.html',
    styleUrls: ['./team.component.scss'],
    providers: [TeamService]
})
export class TeamComponent implements OnInit {

    gameProgress: GameProgressDto;
    constructor(private teamService: TeamService) { }

    ngOnInit() {
        this.teamService.getTeamGameProgress("CustomerLogin1").subscribe(
            success => {
                this.gameProgress = success;
            }
        );
    }

}
