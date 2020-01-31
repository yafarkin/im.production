import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Contract } from '../models/Customer/Contract';
import { Factory } from '../models/Production/Factory';
import { Customer } from '../models/Customer/Customer';
import { MaterialWithPrice } from '../models/Production/MaterialWithPrice';
import { Material } from '../models/Production/Material';
import { MaterialLogistic } from '../models/Production/MaterialLogistic';

@Injectable()
export class ContractsService
{

    constructor(private httpClient: HttpClient) 
    { 
    }
    
    getAllContracts() : Observable<Customer>
    {
        return this.httpClient.get<Customer>(
            "/Contracts/GetAllContracts");
    }
    
}