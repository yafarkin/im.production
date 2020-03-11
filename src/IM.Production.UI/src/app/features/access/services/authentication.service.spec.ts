import { TestBed } from '@angular/core/testing';

import { AuthenticationService } from './authentication.service';
import { HttpClient } from '@angular/common/http';

describe('AuthenticationService', () => {
    let service: AuthenticationService;
    const httpClientMock = {};

    beforeEach(() => TestBed.configureTestingModule({
        providers: [
            { provide: AuthenticationService, useClass: AuthenticationService },
            { provide: HttpClient, useValue: httpClientMock }
        ]
    }));

    beforeEach(() => {
        service = TestBed.get(AuthenticationService)
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
