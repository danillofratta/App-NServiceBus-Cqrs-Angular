import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { StockDto } from '../../../../domain/dto/StockDto';
import { Observable, catchError, finalize, of } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { StockApi } from '../../../../domain/api/StockApi';
import { ProductDto } from '../../../../domain/dto/ProductDto';


@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrl: './stock.component.css'
})
export class StockComponent implements OnInit, AfterViewInit {

  length = 0;
  pageSize = 10;
  pageIndex = 0;

  public _ListError: string[] = [];

  public list$: any[] = [];
  public form: FormGroup;
  public busy = false;
  
  private productDto: any;

  dataSource = new MatTableDataSource<StockDto>();

  displayedColumns = ['id', 'productId', 'productName', 'quantity'];

  @ViewChild(MatPaginator) paginator: any = MatPaginator;

  constructor(
    private api: StockApi,
    private fb: FormBuilder//,
    //private signalRService: SignalRStockService
  ) {
    this.form = this.fb.group({
      quantity: ['', [Validators.required]],
      idproduct: ['', Validators.required],
      nameproduct: ['', Validators.required],
      price: ['', Validators.required]
    });
  }

  ngAfterViewInit(): void {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
  }

  ClearErrorList() {
    this._ListError = [];
  }

  onPageChange(event: PageEvent): void {
    this.busy = true;
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadList();
  }

  async ngOnInit() {
    this.busy = true;

    await this.loadList();
  }

  new() {
    this.form.reset();
  }

  private initForm() {
    this.productDto = null;

    this.form = this.fb.group({
      quantity: ['', [Validators.required]],
      idproduct: ['', Validators.required],
      nameproduct: ['', Validators.required],
      price: ['', Validators.required]
    });
  }

  async save() {
    this.busy = true;

    if (!this.productDto) return;

    if (this.form.valid) {

      const product = this.form.value as ProductDto;

      const stock: StockDto = {
        id: "",
        productid: this.productDto.id,
        productname: this.productDto.name,
        quantity: this.form.get('quantity')?.value,
        price: this.productDto.price
      };

      const observable = await this.api.Save(stock);

      observable.subscribe({
        next: (response) => {
          if (response.success) {
            this.initForm();

            this.loadList();
          } else {
            this._ListError.push(response.message);
          }
        },
        error: (error) => {
          console.error('Error:', error);
          this._ListError.push(error.message);
        },
        complete: () => {
          this.busy = false;
        }
      });
    }
  }

  async loadList() {

    await new Promise(resolve => setTimeout(resolve, 2000));

    this.busy = true;

    const observable = await this.api.GetListAll(this.pageIndex + 1, this.pageSize);
    observable.subscribe({
      next: (response) => {
        if (response.success) {

          this.list$ = response.data.data.items;
          this.length = response.data.data.totalCount;
          this.pageIndex = response.data.data.pageNumber - 1;

          this.paginator.pageIndex = this.pageIndex;
          this.paginator.length = this.length;

          this.dataSource.data = this.list$;
        } else {
          this._ListError.push(response.message);
        }
      },
      error: (error) => {
        console.error('Error:', error);
        this._ListError.push(error.message || 'Erro ao carregar dados');
      },
      complete: () => {
        this.busy = false;
      }
    });

    this.busy = false;
  }

  onProductSelected(record: ProductDto) {
    this.productDto = record;

    if (record) {
      this.form.controls['idproduct'].setValue(record.id);
      this.form.controls['nameproduct'].setValue(record.name);
      this.form.controls['price'].setValue(record.price);
    } else {
      this.form.controls['idproduct'].reset();
      this.form.controls['nameproduct'].reset();
      this.form.controls['price'].reset();
    }
  }
}


