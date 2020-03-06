import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ContractDto } from '../models/contract.dto';
import { FactoryContractDto } from '../models/factory.contract.dto';

@Injectable()
export class ContractsService {
    private contractUrl: string = "/api/contracts/single";
    private allUrl: string = "/api/contracts/all";
    private getOneTimeContractsUrl: string = "/api/contracts/get-one-time-contracts";
    private getMultiTimeContractsUrl: string = "/api/contracts/get-multi-time-contracts";

    constructor(private httpClient: HttpClient) {
    }

    getContract(id: string): Observable<ContractDto> {
        let params = new HttpParams().set('id', id);
        return this.httpClient.get<ContractDto>(this.contractUrl, {
            params: params
        });
    }

    getAllContracts(): Observable<ContractDto[]> {
        return this.httpClient.get<ContractDto[]>(this.allUrl);
    }

    getOneTimeContracts(login: string): Observable<FactoryContractDto[]> {
        let params = new HttpParams().set('login', login);
        return this.httpClient.get<FactoryContractDto[]>(this.getOneTimeContractsUrl, {
            params: params
        });
    }

    getMultiTimeContracts(login: string): Observable<FactoryContractDto[]> {
        let params = new HttpParams().set('login', login);
        return this.httpClient.get<FactoryContractDto[]>(this.getMultiTimeContractsUrl, {
            params: params
        });
    }

}
