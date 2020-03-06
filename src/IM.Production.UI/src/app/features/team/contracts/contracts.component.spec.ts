import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ContractsComponent } from './contracts.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ContractsService } from '../services/contracts.service';

fdescribe('ContractsComponent', () => {
    let component: ContractsComponent;
    let fixture: ComponentFixture<ContractsComponent>;

    const serviceMock = {};

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ContractsComponent],
            providers: [
                {provide: ContractsService, useValue: serviceMock}
            ],
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
