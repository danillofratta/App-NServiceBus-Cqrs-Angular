

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { Observable, map, startWith, BehaviorSubject, catchError, firstValueFrom } from 'rxjs';
import { SaleApi } from '../../../../../domain/api/SaleApi';
import { Router } from '@angular/router';
import { CustomerDto } from '../../../../../domain/dto/CustomerDto';
import { BranchDto } from '../../../../../domain/dto/BranchDto';
import { CreateSaleRequestDto } from '../../../../../domain/dto/sale/create/CreateSaleRequestDto';
import { ProductDto } from '../../../../../domain/dto/ProductDto';
import { CreateSaleItemResponseDto } from '../../../../../domain/dto/sale/create/CreateSaleResponseDto';

@Component({
  selector: 'app-create-sale',
  templateUrl: './create-sale.component.html',
  styleUrls: ['./create-sale.component.css']
})
export class CreateSaleComponent implements OnInit {
  public form: FormGroup;
  public formitem: FormGroup;
  public busy = false;
  public _ListError: string[] = [];

  private productDto: any;

  private saleItemsSubject = new BehaviorSubject<CreateSaleItemResponseDto[]>([]);
  public saleItems$ = this.saleItemsSubject.asObservable();

  public customers: CustomerDto[] = [
    { id: "1", name: 'Customer 1' },
    { id: "2", name: 'Customer 2' },
    { id: "3", name: 'Customer 3' },
  ];
  public branchs: BranchDto[] = [
    { id: "1", name: 'Branch 1' },
    { id: "2", name: 'Branch 2' },
    { id: "3", name: 'Branch 3' },
  ];

  displayedColumns: string[] = ['ProductId', 'ProductName', 'Quantity', 'UnitPrice', 'TotalPrice', 'actions'];
  filteredCustomers: Observable<CustomerDto[]>;
  filteredBranchs: Observable<BranchDto[]>;

  constructor(
    private fb: FormBuilder,
    private fbitem: FormBuilder,
    private api: SaleApi,
    private router: Router
  ) {
    this.form = this.fb.group({
      customerControl: ['', Validators.required],
      branchControl: ['', Validators.required],
    });

    this.formitem = this.fbitem.group({
      nameproduct: [''],
      idproduct: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      saleItems: this.fb.array([])
    });

    this.filteredCustomers = this.form.get('customerControl')!.valueChanges.pipe(
      startWith(''),
      map(value => (typeof value === 'string' ? value : value?.name || '')),
      map(name => name ? this._filter(name, this.customers) : this.customers.slice())
    );

    this.filteredBranchs = this.form.get('branchControl')!.valueChanges.pipe(
      startWith(''),
      map(value => (typeof value === 'string' ? value : value?.name || '')),
      map(name => name ? this._filter(name, this.branchs) : this.branchs.slice())
    );
  }

  ngOnInit() { }

  addItem() {
    const quantity = this.formitem.get('quantity')?.value || 1;
    const unitPrice = this.formitem.get('unitPrice')?.value || 0;

    if (!this.productDto) return;

    const newItem: CreateSaleItemResponseDto = {
      ProductId: this.productDto.id,
      ProductName: this.productDto.name,
      Quantity: quantity,
      UnitPrice: unitPrice,
      TotalPrice: quantity * unitPrice,
    };

    const currentItems = this.saleItemsSubject.value;
    this.saleItemsSubject.next([...currentItems, newItem]);

    // Reset the product selection fields
    this.formitem = this.fbitem.group({
      nameproduct: [''],
      idproduct: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(0)]]
    });

    this.productDto = null;
  }

  removeItem(index: number) {
    // Remove do BehaviorSubject
    const currentItems = this.saleItemsSubject.value;
    const updatedItems = currentItems.filter((_, i) => i !== index);
    this.saleItemsSubject.next(updatedItems);
  }

  private _filter(value: string, list: any[]): any[] {
    const filterValue = value.toLowerCase();
    return list.filter(item => item.name.toLowerCase().includes(filterValue));
  }

  displayCustomer(customer: CustomerDto): string {
    return customer ? `${customer.id} - ${customer.name}` : ''
  }

  displayBranch(branch: BranchDto): string {
    return branch ? `${branch.id} - ${branch.name}` : '';
  }

  async save() {
    this.busy = true;  

    if (this.form.invalid || this.saleItems$ == null) {
      this._ListError.push('Fill in the required fields');
      return;
    }

    const saleData: CreateSaleRequestDto = {
      customerId: this.form.get('customerControl')?.value?.id,
      customerName: this.form.get('customerControl')?.value?.name,
      branchId: this.form.get('branchControl')?.value?.id,
      branchName: this.form.get('branchControl')?.value?.name,
      saleItens: this.saleItemsSubject.value
    };

    try {

      await (await this.api.Create(saleData)).subscribe({
        next: (response) => {
          if (response.success) {
            this.router.navigate(['/sale']);
          } else {
            this._ListError.push(response.message);
          }
        },
        error: (error) => {
          console.error('Error occurred:', error);

          this._ListError.push(error.error.message);
        },
        complete: () => {
          this.busy = false;
        }
      });

      await new Promise(resolve => setTimeout(resolve, 3000));

    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : JSON.stringify(error);

      this._ListError.push(errorMessage);

      console.error('Error during sale creation:', error); // Log do erro para debug
    } finally {
      this.busy = false;
    }
  }

  cancel() {
    this.router.navigate(['/sale']);
  }

  onProductSelected(record: ProductDto) {
    this.productDto = record;

    if (record) {
      this.form.controls['idproduct'].setValue(record.id);
      this.form.controls['nameproduct'].setValue(record.name);
    } else {
      this.form.controls['idproduct'].reset();
      this.form.controls['nameproduct'].reset();
    }
  }
}


