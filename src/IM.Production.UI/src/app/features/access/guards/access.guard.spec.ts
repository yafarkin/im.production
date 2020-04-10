import { TestBed } from '@angular/core/testing';

import { AccessGuard } from './access.guard';
import { AuthenticationService } from '../services/authentication.service';
import { NavigationService } from '../../shared/services/navigation.service';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { UserDto } from '../models/user.dto';
import { Roles } from '../models/roles';

describe('AccessGuard', () => {
    let accessGuard: AccessGuard;
    let authenticationServiceMock: any;

    const routeMock = <ActivatedRouteSnapshot>{
        data: {}
    };
    const stateMock = <RouterStateSnapshot>{};
    const navigationServiceMock = {
        navigateToLogin: jasmine.createSpy('navigateToLogin')
    };

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                { provide: AccessGuard, useClass: AccessGuard },
                { provide: NavigationService, useValue: navigationServiceMock },
                { provide: AuthenticationService, useValue: {} }
            ]
        });
    });

    beforeEach(() => {
        accessGuard = TestBed.get(AccessGuard);
        authenticationServiceMock = TestBed.get(AuthenticationService);
    });

    it('should be created', () => {
        expect(accessGuard).toBeTruthy();
    });

    it('should navigate to login if the current user is not authenticated', () => {
        stateMock.url = '/admin';

        accessGuard.canActivate(routeMock, stateMock);

        expect(navigationServiceMock.navigateToLogin).toHaveBeenCalledWith(stateMock.url);
    });

    it('should not activate if the current user is not authenticated', () => {
        expect(accessGuard.canActivate(routeMock, stateMock)).toBeFalsy();
    });

    it('should not activate if the current user is not authorized', () => {
        authenticationServiceMock.currentUser = <UserDto>{};

        expect(accessGuard.canActivate(routeMock, stateMock)).toBeFalsy();
    });

    it('should activate if the current user is authorized', () => {
        const role = Roles.admin;
        routeMock.data.role = role;
        authenticationServiceMock.currentUser = <UserDto>{ role: role };

        expect(accessGuard.canActivate(routeMock, stateMock)).toBeTruthy();
    });

    it('should not activate if the current user is unable to', () => {
        routeMock.data.role = Roles.admin;
        authenticationServiceMock.currentUser = <UserDto>{ role: Roles.team };

        expect(accessGuard.canActivate(routeMock, stateMock)).toBeFalsy();
    });

    it('should not activate if the role is not specified for a route', () => {
        authenticationServiceMock.currentUser = <UserDto>{ role: Roles.team };

        expect(accessGuard.canActivate(routeMock, stateMock)).toBeFalsy();
    });
});
