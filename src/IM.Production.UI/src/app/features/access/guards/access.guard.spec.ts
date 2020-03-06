import { TestBed, async, inject } from '@angular/core/testing';

import { AccessGuard } from './access.guard';
import { AuthenticationService } from '../services/authentication.service';

describe('AccessGuard', () => {
    const authenticationServiceMock = {};
    const accessGuardMock = {};

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                { provide: AuthenticationService, useValue: authenticationServiceMock },
                { provide: AccessGuard, useValue: accessGuardMock }
            ]
        });
    });

    it('should ...', inject([AccessGuard], (guard: AccessGuard) => {
        expect(guard).toBeTruthy();
    }));
});
