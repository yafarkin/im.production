import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
 
@Injectable()
export class ContractsService
{
    constructor(private httpClient: HttpClient) 
    { 
    }

    getAllContracts() : Observable<string>
    {
        return this.httpClient.get<string>("http://localhost:4200/Contracts/GetAllContracts");
    }
    
}