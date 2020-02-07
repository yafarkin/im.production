import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ContractDto } from '../models/dtos/contract.dto';

@Injectable()
export class ContractsService {
    private contractUrl: string = "/api/contracts/single";
    private allUrl: string = "/api/contracts/all";

    constructor(private httpClient: HttpClient) {
    }

    getContract(): Observable<ContractDto> {
        return this.httpClient.get<ContractDto>(this.contractUrl);
    }

    getAllContracts(): Observable<ContractDto[]> {
        return this.httpClient.get<ContractDto[]>(this.allUrl);
    }
}
