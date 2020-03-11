import { TestBed } from '@angular/core/testing';

import { NavigationService } from './navigation.service';
import { Router } from '@angular/router';

describe('NavigationService', () => {
    const routerMock = {};
    const navigationServiceMock = {};

    beforeEach(() => TestBed.configureTestingModule({
        providers: [
            { provide: NavigationService, useValue: navigationServiceMock },
            { provide: Router, useValue: routerMock }
        ]
    }));

    it('should be created', () => {
        const service: NavigationService = TestBed.get(NavigationService);
        expect(service).toBeTruthy();
    });
});
