import { Injectable } from '@angular/core';
import { User } from '../models/user';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {
    get currentUser(): User {
        return null;
    }

}
