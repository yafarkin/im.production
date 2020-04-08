import { TestBed } from '@angular/core/testing';

import { AuthenticationService } from './authentication.service';
import { HttpClient } from '@angular/common/http';
import { AuthenticationDto } from '../models/authentication.dto';
import { UserDto } from '../models/user.dto';
import { of } from 'rxjs/internal/observable/of';
import { Roles } from '../models/roles';

describe('AuthenticationService', () => {
    let service: AuthenticationService;
    const httpClientMock = {
        post: jasmine.createSpy('post').and.returnValue(of())
    };

    beforeEach(() => TestBed.configureTestingModule({
        providers: [
            { provide: AuthenticationService, useClass: AuthenticationService },
            { provide: HttpClient, useValue: httpClientMock }
        ]
    }));

    beforeEach(() => {
        service = TestBed.get(AuthenticationService);
        localStorage.clear();
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should call to authenticate with authentication credentials', async () => {
        const authentication = new AuthenticationDto();

        await service.authenticate(authentication);

        expect(httpClientMock.post).toHaveBeenCalledWith('/api/authentication', authentication);
    });

    it('should put a user into local storage', async () => {
        const user = <UserDto>{};
        httpClientMock.post.and.returnValue(of(user));

        await service.authenticate(<AuthenticationDto>{});

        expect(localStorage.getItem('currentUser')).toBeDefined();
    });

    it('should put a user into local storage stringified', async () => {
        const user = <UserDto>{ login: 'admin' };
        httpClientMock.post.and.returnValue(of(user));

        await service.authenticate(<AuthenticationDto>{});

        expect(localStorage.getItem('currentUser')).toBe(JSON.stringify(user));
    });

    it('should return the current user if local storage contains', () => {
        const user = <UserDto>{ login: 'admin' };
        localStorage.setItem('currentUser', JSON.stringify(user));

        expect(service.currentUser).toEqual(user);
    });

    it('should not return the current user if local storage does not contain', () => {
        expect(service.currentUser).toBeNull();
    });
});
