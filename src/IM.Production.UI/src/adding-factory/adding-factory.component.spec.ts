import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddingFactoryComponent } from './adding-factory.component';

describe('AddingFactoryComponent', () => {
    let component: AddingFactoryComponent;
    let fixture: ComponentFixture<AddingFactoryComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AddingFactoryComponent]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AddingFactoryComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
