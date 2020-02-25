import { Injectable } from '@angular/core';
import { UserDto } from '../models/user.dto';
import { SignInDto } from '../models/sign-in.dto';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {
    constructor(private httpClient: HttpClient) { }

    get currentUser(): UserDto {
        return null;
    }

    signIn(value: SignInDto) {
        this.httpClient.post<UserDto>("/api/users/authenticate", value)
            .subscribe(user => {

            });
    }
}
