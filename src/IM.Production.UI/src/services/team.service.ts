import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FactoryDefinition } from '../models/dtos/factory.definition';
import { Observable } from 'rxjs';
import { PurchaseFactoryDataDto } from '../models/dtos/purchase.factory.data.dto';

@Injectable()
export class TeamService {
    private buyFactoryUrl: string = "/api/team/buy-factory";
    private getFactoriesDefinitionsUrl: string = "/api/team/get-factories-definitions";

    constructor(private httpClient: HttpClient) { }

    buyFactory(login: string, definitionIndex: number): Observable<any> {
        let data: PurchaseFactoryDataDto = new PurchaseFactoryDataDto();
        data.login = login;
        data.definitionIndex = definitionIndex;
        return this.httpClient.post<PurchaseFactoryDataDto>(this.buyFactoryUrl, data);
    }

    getFactoriesDefinitions(login: string): Observable<FactoryDefinition[]> {
        let params = new HttpParams().set('login', login);
        return this.httpClient.get<FactoryDefinition[]>(this.getFactoriesDefinitionsUrl, {
            params: params
        });
    }

}
