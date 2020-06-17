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

    navigateToFactoryStock(factoryId: string) {
        this.router.navigate(["team/factory-stock/", factoryId], { queryParams: {id: factoryId} });
    }

    navigateToUrl(url: string) {
        this.router.navigate([url]);
    }
}
