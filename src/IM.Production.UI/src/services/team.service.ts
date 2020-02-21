import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FactoryAndContractFactoriesDto } from '../models/dtos/factory.and.contract.factories.dto';
import { GameProgressDto } from '../models/dtos/game.progress.dto';

@Injectable()
export class TeamService {
    getFactoriesUrl: string = "/api/team/get-team-factories";
    getTeamGameProgressUrl: string = "/api/team/get-team-game-progress";

    constructor(private httpClient: HttpClient) { }

    getFactories(login: string): Observable<FactoryAndContractFactoriesDto[]> {
        let params = new HttpParams().set('login', login);
        return this.httpClient.get<FactoryAndContractFactoriesDto[]>(this.getFactoriesUrl, {
            params: params
        });
    }

    getTeamGameProgress(login: string) : Observable<GameProgressDto> {
        let params = new HttpParams().set('login', login);
        return this.httpClient.get<GameProgressDto>(this.getTeamGameProgressUrl, {
            params: params
        });
    }

}
