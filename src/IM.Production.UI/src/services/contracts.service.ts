import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ContractDto } from '../models/dtos/contract.dto';

@Injectable()
export class ContractsService {

    constructor(private httpClient: HttpClient) {
    }

    getContracts(): Observable<ContractDto[]> {
        return this.httpClient.get<ContractDto[]>(
            "/api/contracts/all");
    }

}
