import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { GameManagementComponent } from './game-management.component';

describe('GameManagementComponent', () => {
  let component: GameManagementComponent;
  let fixture: ComponentFixture<GameManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GameManagementComponent ]
    })
    .compileComponents();
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
