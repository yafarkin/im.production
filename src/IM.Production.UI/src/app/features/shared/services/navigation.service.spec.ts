import { TestBed } from '@angular/core/testing';

import { NavigationService } from './navigation.service';
import { Router } from '@angular/router';

describe('NavigationService', () => {
    let service: NavigationService;

    const routerMock = {
        navigate: jasmine.createSpy('navigate')
    };

    beforeEach(() => TestBed.configureTestingModule({
        providers: [
            { provide: NavigationService, useClass: NavigationService },
            { provide: Router, useValue: routerMock }
        ]
    }));

    beforeEach(() => {
        service = TestBed.get(NavigationService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should navigate to the admin route', () => {
        service.navigateToAdmin();

        expect(routerMock.navigate).toHaveBeenCalledWith(['admin']);
    });

    it('should navigate to a URL', () => {
        const url = '/admin';

        service.navigateToUrl(url);

        expect(routerMock.navigate).toHaveBeenCalledWith([url]);
    });

    it('should navigate to the login route with a return URL', () => {
        const returnUrl = '/admin';

        service.navigateToLogin(returnUrl);

        expect(routerMock.navigate).toHaveBeenCalledWith(['login'], { queryParams: { returnUrl } });
    });
});
