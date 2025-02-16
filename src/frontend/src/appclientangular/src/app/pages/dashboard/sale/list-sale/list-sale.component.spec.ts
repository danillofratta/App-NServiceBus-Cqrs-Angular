import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListSaleComponent } from './list-sale.component';

describe('ListSaleComponent', () => {
  let component: ListSaleComponent;
  let fixture: ComponentFixture<ListSaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListSaleComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListSaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
