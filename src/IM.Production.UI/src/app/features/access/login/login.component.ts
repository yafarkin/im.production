import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthenticationService } from '../services/authentication.service';
import { NavigationService } from '../../shared/services/navigation.service';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'imp-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent {
    formGroup: FormGroup;

    constructor(
        private authenticationService: AuthenticationService,
        private navigationService: NavigationService,
        private route: ActivatedRoute) {
        this.initFormGroup();
    }

    async login() {
        const authentication = this.formGroup.getRawValue();
        await this.authenticationService.authenticate(authentication);

        this.navigationService.navigateToUrl(this.route.snapshot.queryParams.returnUrl);
    }

    private initFormGroup() {
        this.formGroup = new FormGroup({
            login: new FormControl(null, Validators.required),
            password: new FormControl(null, Validators.required)
        });
    }
}
