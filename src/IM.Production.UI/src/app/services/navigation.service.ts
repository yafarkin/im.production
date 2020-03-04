import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class NavigationService {
    constructor(private router: Router) { }

    navigateToLogin(returnUrl: string) {
        this.router.navigate(['login'], { queryParams: { returnUrl } });
    }

    navigateToAdmin() {
        this.router.navigate(['admin']);
    }

    navigateToUrl(url: string) {
        this.router.navigate([url]);
    }
}
