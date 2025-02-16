import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSaleComponent } from './CreateSaleComponent';

describe('CreateSaleComponent', () => {
  let component: CreateSaleComponent;
  let fixture: ComponentFixture<CreateSaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateSaleComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateSaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
