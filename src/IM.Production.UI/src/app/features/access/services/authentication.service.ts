import { Injectable } from '@angular/core';
import { UserDto } from '../models/user.dto';
import { AuthenticationDto } from '../models/authentication.dto';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AuthenticationService {
    constructor(private httpClient: HttpClient) { }

    get currentUser(): UserDto {
        return JSON.parse(localStorage.getItem('currentUser'));
    }

    async authenticate(authentication: AuthenticationDto) {
        const user = await this.httpClient.post<UserDto>('/api/authentication', authentication).toPromise();
        localStorage.setItem('currentUser', JSON.stringify(user));
    }
}
