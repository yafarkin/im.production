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

    const authenticationServiceMock = {
        authenticate: jasmine.createSpy('authenticate')
    };
    const navigationServiceMock = {
        navigateToUrl: jasmine.createSpy('navigateToUrl')
    };
    const routeMock = {
        snapshot: {
            queryParams: {
                returnUrl: ''
            }
        }
    };

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

    it('should create form group', () => {
        expect(component.formGroup).toBeTruthy();
    });

    it('should create login form field', () => {
        expect(component.formGroup.controls.login).toBeTruthy();
    });

    it('should create required validation for login', () => {
        expect(component.formGroup.controls.login.validator).toBe(Validators.required);
    });

    it('should create password form field', () => {
        expect(component.formGroup.controls.password).toBeTruthy();
    });

    it('should authenticate with set authentication credentials', async () => {
        const login = 'admin';
        const password = 'password';
        component.formGroup.setValue({ login, password });

        await component.login();

        expect(authenticationServiceMock.authenticate).toHaveBeenCalledWith({ login, password });
    });

    it('should navigate to return URL', async () => {
        const routeMock = TestBed.get(ActivatedRoute);
        const returnUrl = "/admin";
        routeMock.snapshot.queryParams.returnUrl = returnUrl;

        await component.login();

        expect(navigationServiceMock.navigateToUrl).toHaveBeenCalledWith(returnUrl);
    });
});
