import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TeamDto } from '../models/dtos/team';
import { Observable } from 'rxjs';

import { NewTeamDto } from '../models/dtos/newteam.dto';

@Injectable()
export class TeamsService {
    private url = "/api/teams";
    private addNewTeamUrl = "/api/teams/addteam";

    constructor(private http: HttpClient) { }

    getTeams(): Observable<TeamDto[]> {
        return this.http.get<TeamDto[]>(this.url);
    }

    addTeam(team: NewTeamDto):  Observable<any> {
        return this.http.post(this.addNewTeamUrl, team);
    }
}
