import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { NavigationService } from '../services/navigation.service';

@Injectable({
    providedIn: 'root'
})
export class AccessGuard implements CanActivate {
    constructor(private authenticationService: AuthenticationService, private navigationService: NavigationService) { }

    canActivate() {
        const currentUser = this.authenticationService.currentUser;

        if (currentUser) {
            //TODO Check the current user roles
            return true;
        }

        this.navigationService.navigateToLogin();

        return false;
    }
}
