import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FactoriesComponent } from './factories.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { AuthenticationService } from '../../access/services/authentication.service';

describe('FactoriesComponent', () => {
    let component: FactoriesComponent;
    let fixture: ComponentFixture<FactoriesComponent>;

    const authenticationServiceMock = {};

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [FactoriesComponent],
            providers: [
                { provide: AuthenticationService, useValue: authenticationServiceMock }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(FactoriesComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
