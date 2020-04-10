import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractComponent } from './contract.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ContractsService } from '../services/contracts.service';
import { of } from 'rxjs/internal/observable/of';

describe('ContractComponent', () => {
    let component: ContractComponent;
    let fixture: ComponentFixture<ContractComponent>;

    const serviceMock = {
        getContract: jasmine.createSpy('getContract').and.returnValue(of())
    };
    const routeMock = {
        snapshot: { params: {} }
    };
    const routerMock = {};

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ContractComponent],
            providers: [
                { provide: ContractsService, useValue: serviceMock },
                { provide: ActivatedRoute, useValue: routeMock },
                { provide: Router, useValue: routerMock }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ContractComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
