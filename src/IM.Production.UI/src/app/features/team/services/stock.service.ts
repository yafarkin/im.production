import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { StockMaterialDto } from '../models/stock.material.dto';

@Injectable()
export class StockService {
    private stockUrl: string = "/api/stock/";

    constructor(private httpClient: HttpClient) { }

    getMaterials(login: string, factoryId: string): Observable<StockMaterialDto[]> {
        let params = new HttpParams().set("login", login).set("factoryId", factoryId);
        return this.httpClient.get<StockMaterialDto[]>(this.stockUrl, {params: params});
    }
}
