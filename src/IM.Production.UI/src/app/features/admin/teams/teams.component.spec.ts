import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamsComponent } from './teams.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { MatTableModule, MatDialog } from '@angular/material';
import { TeamsService } from '../services/teams.service';
import { of } from 'rxjs/internal/observable/of';

describe('TeamsComponent', () => {
    let component: TeamsComponent;
    let fixture: ComponentFixture<TeamsComponent>;

    const serviceMock = {
        getTeams: jasmine.createSpy('getTeams').and.returnValue(of())
    };
    const dialogMock = {};

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [TeamsComponent],
            imports: [MatTableModule],
            providers: [
                { provide: TeamsService, useValue: serviceMock },
                { provide: MatDialog, useValue: dialogMock }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(TeamsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
