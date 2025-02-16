import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModifySaleComponent } from './modify-sale.component';

describe('ModifySaleComponent', () => {
  let component: ModifySaleComponent;
  let fixture: ComponentFixture<ModifySaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ModifySaleComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ModifySaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
