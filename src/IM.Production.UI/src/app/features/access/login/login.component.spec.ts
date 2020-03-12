import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { NavigationService } from '../../shared/services/navigation.service';
import { ActivatedRoute } from '@angular/router';
import { Validators } from '@angular/forms';

describe('LoginComponent', () => {
    let component: LoginComponent;
    let fixture: ComponentFixture<LoginComponent>;

    const authenticationServiceMock = {};
    const navigationServiceMock = {};
    const routeMock = {};

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [LoginComponent],
            providers: [
                { provide: AuthenticationService, useValue: authenticationServiceMock },
                { provide: NavigationService, useValue: navigationServiceMock },
                { provide: ActivatedRoute, useValue: routeMock }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(LoginComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should create the form group', () => {
        expect(component.formGroup).toBeTruthy();
    });

    it('should create the login form field', () => {
        expect(component.formGroup.controls.login).toBeTruthy();
    });

    it('should create the required validation for login', () => {
        expect(component.formGroup.controls.login.validator).toBe(Validators.required);
    });

    it('should create the password form field', () => {
        expect(component.formGroup.controls.password).toBeTruthy();
    });


});
