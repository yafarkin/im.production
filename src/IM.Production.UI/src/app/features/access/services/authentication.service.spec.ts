import { TestBed } from '@angular/core/testing';

import { AuthenticationService } from './authentication.service';
import { HttpClient } from '@angular/common/http';

describe('AuthenticationService', () => {
    const httpClientMock = {};
    const authenticationServiceMock = {};

    beforeEach(() => TestBed.configureTestingModule({
        providers: [
            { provide: HttpClient, useValue: httpClientMock },
            { provide: AuthenticationService, useValue: authenticationServiceMock }
        ]
    }));

    it('should be created', () => {
        const service: AuthenticationService = TestBed.get(AuthenticationService);
        expect(service).toBeTruthy();
    });
});
