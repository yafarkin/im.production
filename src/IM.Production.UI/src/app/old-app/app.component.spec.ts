import { TestBed, async } from '@angular/core/testing';
import { OldAppComponent } from './app.component';

describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        OldAppComponent
      ],
    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(OldAppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'IM-Production-UI'`, () => {
    const fixture = TestBed.createComponent(OldAppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.title).toEqual('IM-Production-UI');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(OldAppComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('.content span').textContent).toContain('IM-Production-UI app is running!');
  });
});
