import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Team } from '../models/team';
import { Observable } from 'rxjs';

@Injectable()
export class TeamsService {
    private url = "https://localhost:44394/api/teams";
    constructor(private http: HttpClient) { }

    getTeams(): Observable<Team[]> {
        return this.http.get<Team[]>(this.url);
    }
}
