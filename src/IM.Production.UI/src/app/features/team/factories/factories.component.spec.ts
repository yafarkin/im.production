import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FactoriesComponent } from './factories.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { AuthenticationService } from '../../access/services/authentication.service';
import { TeamService } from '../services/team.service';
import { of } from 'rxjs/internal/observable/of';

describe('FactoriesComponent', () => {
    let component: FactoriesComponent;
    let fixture: ComponentFixture<FactoriesComponent>;

    const authenticationServiceMock = {
        currentUser: { login: {} }
    };
    const teamServiceMock = {
        getFactories: jasmine.createSpy('getFactories').and.returnValue(of())
    };

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [FactoriesComponent],
            providers: [
                { provide: TeamService, useValue: teamServiceMock },
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
