import { Component, OnInit } from '@angular/core';
import { TeamService } from '../services/team.service';
import { TeamProgressDto } from '../models/team.progress.dto';
import { AuthenticationService } from '../../../services/authentication.service';

@Component({
    selector: 'app-team',
    templateUrl: './team.component.html',
    styleUrls: ['./team.component.scss']
})
export class TeamComponent implements OnInit {

    teamProgress: TeamProgressDto;
    constructor(private teamService: TeamService, private authService: AuthenticationService) { }

    ngOnInit() {
        let login = this.authService.currentUser.login;
        this.teamService.getTeamProgress(login).subscribe(
            success => {
                this.teamProgress = success;
            }
        );
    }

}
