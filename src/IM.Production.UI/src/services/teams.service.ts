import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Team } from '../models/team';
import { Observable } from 'rxjs';

import { NewTeamDto } from '../models/dtos/newteam.dto';

@Injectable()
export class TeamsService {
    private url = "/api/teams";
    private addNewTeamUrl = "/api/teams/addteam";

    constructor(private http: HttpClient) { }

    getTeams(): Observable<Team[]> {
        return this.http.get<Team[]>(this.url);
    }

    addTeam(team: NewTeamDto):  Observable<any> {
        return this.http.post(this.addNewTeamUrl, team);
    }
}
