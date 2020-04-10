import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { NavigationService } from '../../shared/services/navigation.service';

@Injectable()
export class AccessGuard implements CanActivate {
    constructor(private authenticationService: AuthenticationService, private navigationService: NavigationService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this.authenticationService.currentUser;

        if (currentUser) {
            if (route.data.role && route.data.role === currentUser.role) {
                return true;
            }

            return false;
        }

        this.navigationService.navigateToLogin(state.url);
        return false;
    }
}
