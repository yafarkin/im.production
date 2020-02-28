import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthenticationService } from '../services/authentication.service';

@Component({
    selector: 'imp-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent {
    formGroup: FormGroup;

    constructor(private authenticationService: AuthenticationService) {
        this.initFormGroup();
    }

    login() {
        const value = this.formGroup.getRawValue();
        this.authenticationService.authenticate(value);
    }

    private initFormGroup() {
        this.formGroup = new FormGroup({
            login: new FormControl(null, Validators.required),
            password: new FormControl(null, Validators.required)
        });
    }
}
