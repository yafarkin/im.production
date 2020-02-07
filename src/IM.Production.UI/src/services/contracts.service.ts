import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ContractDto } from '../models/dtos/contract.dto';

@Injectable()
export class ContractsService {
    private url: string = "/api/contracts/all";

    constructor(private httpClient: HttpClient) {
    }

    getAllContracts(): Observable<ContractDto[]> {
        return this.httpClient.get<ContractDto[]>(this.url);
    }

}
