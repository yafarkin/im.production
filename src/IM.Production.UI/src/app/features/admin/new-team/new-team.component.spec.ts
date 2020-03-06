import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewTeamComponent } from './new-team.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { TeamsService } from '../services/teams.service';
import { MatSnackBar } from '@angular/material';

describe('NewTeamComponent', () => {
    let component: NewTeamComponent;
    let fixture: ComponentFixture<NewTeamComponent>;

    const serviceMock = {};
    const snackBarMock = {};

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [NewTeamComponent],
            providers: [
                { provide: TeamsService, useValue: serviceMock },
                { provide: MatSnackBar, useValue: snackBarMock }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(NewTeamComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
