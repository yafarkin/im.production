import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { GameManagementComponent } from './game-management.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { GameManagementService } from '../services/game.management.service';
import { of } from 'rxjs/internal/observable/of';

describe('GameManagementComponent', () => {
    let component: GameManagementComponent;
    let fixture: ComponentFixture<GameManagementComponent>;

    const serviceMock = {
        getGameConfig: jasmine.createSpy('getGameConfig').and.returnValue(of()),
        getCurrentDay: jasmine.createSpy('getCurrentDay').and.returnValue(of())
    };

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [GameManagementComponent],
            providers: [
                { provide: GameManagementService, useValue: serviceMock }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(GameManagementComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
