import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FactoryDto } from '../models/dtos/factory.dto';
import { Observable } from 'rxjs';
import { FactoryAndContractFactoriesDto } from '../models/dtos/factory.and.contract.factories.dto';

@Injectable()
export class TeamService {
    getFactoriesUrl: string = "/api/team/get-team-factories";
    getContractFactoriesUrl: string = "/api/team/get-team-contract-factories";

    constructor(private httpClient: HttpClient) { }

    getFactories(login: string): Observable<FactoryAndContractFactoriesDto[]> {
        let params = new HttpParams().set('login', login);
        return this.httpClient.get<FactoryAndContractFactoriesDto[]>(this.getFactoriesUrl, {
            params: params
        });
    }

}
