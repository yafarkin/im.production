import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamComponent } from './team.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { TeamService } from '../services/team.service';
import { AuthenticationService } from '../../access/services/authentication.service';
import { of } from 'rxjs/internal/observable/of';

describe('TeamComponent', () => {
    let component: TeamComponent;
    let fixture: ComponentFixture<TeamComponent>;

    const teamServiceMock = {
        getTeamProgress: jasmine.createSpy('getTeamProgress').and.returnValue(of())
    };
    const authenticationServiceMock = {
        currentUser: jasmine.createSpy('currentUser')
    };

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [TeamComponent],
            providers: [
                { provide: TeamService, useValue: teamServiceMock },
                { provide: AuthenticationService, useValue: authenticationServiceMock }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(TeamComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
