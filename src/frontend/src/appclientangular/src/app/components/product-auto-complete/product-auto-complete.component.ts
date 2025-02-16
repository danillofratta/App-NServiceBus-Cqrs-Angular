import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ProductDto } from '../../../domain/dto/ProductDto';
import { Observable, Subject } from 'rxjs';
import { ProductApi } from '../../../domain/api/ProductApi';

@Component({
  selector: 'app-product-auto-complete',
  templateUrl: './product-auto-complete.component.html',
  styleUrl: './product-auto-complete.component.css'
})
export class ProductAutoCompleteComponent {
  //@Input() apiUrl: string; // API endpoint
  @Input() placeholder: string = 'Search product...'; // Input placeholder
  @Output() productSelected = new EventEmitter<ProductDto>(); // Emit selected customer

  public list$: Observable<ProductDto[]> = new Observable<ProductDto[]>();

  searchText: string = '';
  filteredCustomers: Observable<ProductDto[]> = new Observable<ProductDto[]>();
  loading: boolean = false;
  showDropdown: boolean = false;
  selectedIndex: number = -1;

  private searchSubject = new Subject<string>();

  constructor(private api: ProductApi) { }

  ngOnInit() {
  }

  async onSearch() {
    this.loading = true;

    if (this.searchText.length > 3)
      this.list$ = await this.api.GetByName(this.searchText);

    this.loading = false;
  }

  async getListProducts(name: string) {
    return this.api.GetByName(name);
  }

  selectProduct(record: ProductDto) {
    this.list$ = new Observable<ProductDto[]>();
    this.searchText = record.name;
    this.productSelected.emit(record);
    this.showDropdown = false;
  }
}
