import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TeamDto } from '../models/team.dto';
import { NewTeamDto } from '../models/newteam.dto';

@Injectable()
export class TeamsService {
    private url = "/api/teams";
    private addNewTeamUrl = "/api/teams/add-team";

    constructor(private http: HttpClient) { }

    getTeams(): Observable<TeamDto[]> {
        return this.http.get<TeamDto[]>(this.url);
    }

    addTeam(team: NewTeamDto): Observable<any> {
        return this.http.post(this.addNewTeamUrl, team);
    }
}
