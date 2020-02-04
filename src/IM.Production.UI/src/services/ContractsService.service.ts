import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ContractDto } from '../models/Dtos/ContractDto';

@Injectable()
export class ContractsService
{

    constructor(private httpClient: HttpClient) 
    { 
    }
    
    getAllContracts() : Observable<ContractDto[]>
    {
        return this.httpClient.get<ContractDto[]>(
            "/Contracts/GetAllContracts");
    }
    
}