import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AddingFactoryDto } from '../models/dtos/adding-factory.dto';

@Injectable()
export class AddingFactoryService {
    private url = "/api/addingFactory";

    constructor(private http: HttpClient) { }

    addFactory(factory: AddingFactoryDto): Observable<any> {
        return this.http.post(this.url, factory);
    }
}
