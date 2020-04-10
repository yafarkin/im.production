import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ContractsComponent } from './contracts.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ContractsService } from '../services/contracts.service';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material';
import { of } from 'rxjs/internal/observable/of';

describe('ContractsComponent', () => {
    let component: ContractsComponent;
    let fixture: ComponentFixture<ContractsComponent>;

    const serviceMock = {
        getAllContracts: jasmine.createSpy('getAllContracts').and.returnValue(of())
    };
    const routerMock = {};

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ContractsComponent],
            providers: [
                { provide: ContractsService, useValue: serviceMock },
                { provide: Router, useValue: routerMock }
            ],
            imports: [MatTableModule],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ContractsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
