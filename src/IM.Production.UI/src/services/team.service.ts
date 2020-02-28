import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TeamProgressDto } from '../models/dtos/team.progress.dto';
import { FactoryDto } from '../models/dtos/factory.dto';

@Injectable()
export class TeamService {
    getFactoriesUrl: string = "/api/team/factories";
    getTeamGameProgressUrl: string = "/api/team/get-team-progress";

    constructor(private httpClient: HttpClient) { }

    getFactories(login: string): Observable<FactoryDto[]> {
        let params = new HttpParams().set('login', login);
        return this.httpClient.get<FactoryDto[]>(this.getFactoriesUrl, {
            params: params
        });
    }

    getTeamProgress(login: string): Observable<TeamProgressDto> {
        let params = new HttpParams().set('login', login);
        return this.httpClient.get<TeamProgressDto>(this.getTeamGameProgressUrl, {
            params: params
        });
    }

}
