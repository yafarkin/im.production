import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ContractDto } from '../models/dtos/contract.dto';

@Injectable()
export class ContractsService {
    private contractUrl: string = "/api/contracts/single";
    private allUrl: string = "/api/contracts/all";

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
}
