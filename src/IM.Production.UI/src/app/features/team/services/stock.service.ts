import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { StockMaterialDto } from '../models/dtos/stock.material.dto';

@Injectable()
export class StockService {
    private contractUrl: string = "/api/stock/";

    constructor(private httpClient: HttpClient) { }

    getMaterials(id: string): Observable<StockMaterialDto[]> {
        return this.httpClient.get<StockMaterialDto[]>(this.contractUrl + id);
    }
}
