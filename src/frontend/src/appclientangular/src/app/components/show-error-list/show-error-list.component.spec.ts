import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowErrorListComponent } from './show-error-list.component';

describe('ShowErrorListComponent', () => {
  let component: ShowErrorListComponent;
  let fixture: ComponentFixture<ShowErrorListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ShowErrorListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShowErrorListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
