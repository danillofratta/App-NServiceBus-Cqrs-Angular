import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ProductDto } from '../../../../domain/dto/ProductDto';
import { catchError, delay, finalize, firstValueFrom, Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { ProductApi } from '../../../../domain/api/ProductApi';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ProductComponent implements OnInit, AfterViewInit {
  public list$: Observable<ProductDto[]> = new Observable<ProductDto[]>();
  public form: FormGroup;
  public busy = false;

  public _ListError: string[] = [];

  dataSource = new MatTableDataSource<ProductDto>();
  displayedColumns = ['actions', 'id', 'name', 'price'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private api: ProductApi,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    //this.initForm();
        this.form = this.fb.group({
          id: [''],
          name: ['', Validators.required],
          price: ['', [Validators.required, Validators.min(0)]],
        });
  }

  private initForm() {
    this.form = this.fb.group({
      id: [null],
      name: ['', Validators.required],
      price: [null, [Validators.required, Validators.min(0)]],
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  async ngOnInit() {
    await this.loadList();
  }

  new() {  
    this.form.reset();
    this.form.patchValue({
      id: null,
      name: '',
      price: null
    });
    this.cdr.detectChanges();
  }

  async save() {
    if (this.form.valid) {
      this.busy = true;
      const product = this.form.value as ProductDto;

      const observable = product.id ? await this.api.Update(product) : await this.api.Save(product);

      observable.subscribe({
        next: (response) => {
          if (response.success) {
            this.initForm(); 
            this.cdr.detectChanges();
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
    this.busy = true;

    try {
      const products = await firstValueFrom(await this.api.GetListAll());
      this.dataSource.data = [...products]; // Cria nova referência do array
      this.cdr.detectChanges(); // Força a atualização da view
    } catch (error) {
      console.error('Erro ao carregar lista de produtos:', error);
      this.dataSource.data = [];
    } finally {
      this.busy = false;
    }
  }

  async onUpdate(id: number) {
    this.busy = true;

    try {
      const product = await firstValueFrom(await this.api.GetById(id));
      if (product) {
        this.form.patchValue(product);
      }
    } catch (error) {
      console.error('Erro ao carregar produto para atualização:', error);
    } finally {
      this.busy = false;
    }
  }

  async onDelete(id: number) {
    this.busy = true;

    try {
      await firstValueFrom(await this.api.Delete(id));
      await this.loadList();
    } catch (error) {
      console.error('Erro ao deletar o produto:', error);
    } finally {
      this.busy = false;
    }
  }
}


