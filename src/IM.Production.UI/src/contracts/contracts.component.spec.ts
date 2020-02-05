/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ContractsComponent } from './contracts.component';

describe('ContractsComponent', () => {
    let component: ContractsComponent;
    let fixture: ComponentFixture<ContractsComponent>;
    let httpClientMock: HttpClient = {};

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ContractsComponent],
            providers: [
                { provide: HttpClient, useValue: httpClientMock }
            ]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ContractsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    //it('weird stuff', () => {
    //  expect(component.arrayData.length).toBeCloseTo(1);
    //});

});
