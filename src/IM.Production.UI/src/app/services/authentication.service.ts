import { Injectable } from '@angular/core';
import { UserDto } from '../models/user.dto';
import { AuthenticationDto } from '../models/authentication';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {
    constructor(private httpClient: HttpClient) { }

    get currentUser(): UserDto {
        return JSON.parse(localStorage.getItem('currentUser'));
    }

    authenticate(value: AuthenticationDto) {
        this.httpClient.post<UserDto>("/api/authentication", value)
            .subscribe(user => {
                localStorage.setItem('currentUser', JSON.stringify(user));
            });
    }
}
