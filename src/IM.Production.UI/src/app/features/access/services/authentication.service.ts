import { Injectable } from '@angular/core';
import { UserDto } from '../models/user.dto';
import { AuthenticationDto } from '../models/authentication.dto';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable()
export class AuthenticationService {
    constructor(private httpClient: HttpClient) { }

    get currentUser(): UserDto {
        return JSON.parse(localStorage.getItem('currentUser'));
    }

    authenticate(value: AuthenticationDto) {
        return this.httpClient.post<UserDto>("/api/authentication", value)
            .pipe(map(user => {
                localStorage.setItem('currentUser', JSON.stringify(user));
            }));
    }
}
