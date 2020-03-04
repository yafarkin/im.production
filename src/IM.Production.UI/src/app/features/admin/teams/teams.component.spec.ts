import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamsComponent } from './teams.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('TeamsComponent', () => {
    let component: TeamsComponent;
    let fixture: ComponentFixture<TeamsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [TeamsComponent],
            schemas: [NO_ERRORS_SCHEMA]
        })
            .compileComponents();
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
