import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FactoryDto } from '../models/dtos/factory.dto';
import { Observable } from 'rxjs';

@Injectable()
export class TeamService {
    getFactoriesUrl: string = "/api/team/get-team-factories";
    getContractFactoriesUrl: string = "/api/team/get-team-contract-factories";

    constructor(private httpClient: HttpClient) { }

    getFactories(login: string): Observable<FactoryDto[]> {
        let params = new HttpParams().set('login', login);
        return this.httpClient.get<FactoryDto[]>(this.getFactoriesUrl, {
            params: params
        });
    }

    getContractFactories(login: string): Observable<FactoryDto[][]> {
        let params = new HttpParams().set('login', login);
        return this.httpClient.get<FactoryDto[][]>(this.getContractFactoriesUrl, {
            params: params
        });
    }
}
