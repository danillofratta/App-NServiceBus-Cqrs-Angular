import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { SaleApi } from '../../../../../domain/api/SaleApi';
import { GetSaleResponseDto } from '../../../../../domain/dto/sale/get/GetSaleResponseDto';
import { Router } from '@angular/router';
import { CancelSaleResponseDto } from '../../../../../domain/dto/sale/cancel/CancelSaleResponseDto';

@Component({
  selector: 'app-list-sale',
  templateUrl: './list-sale.component.html',
  styleUrl: './list-sale.component.css'
})
export class ListSaleComponent implements OnInit, AfterViewInit {

  length = 0;
  pageSize = 10;
  pageIndex = 0;

  //blic list$: Observable<GetSaleResponseDto[]> = new Observable<GetSaleResponseDto[]>();
  public list$: any[] = [];
    
  public busy = false;

  public _ListError: string[] = [];

  dataSource = new MatTableDataSource<GetSaleResponseDto>();

  displayedColumns = ['actions', 'id', 'number', 'customerName', 'branchName', 'status', 'totalAmount'];

  @ViewChild(MatPaginator) paginator: any = MatPaginator;
  
  constructor(private Api: SaleApi, private router: Router) {
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  async ngOnInit() {
    this.busy = true;

    await this.LoadList();
  }

  ClearErrorList() {
    this._ListError = [];
  }

  onPageChange(event: PageEvent): void {
    this.busy = true;
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.LoadList();
  }

  async LoadList() {

    await new Promise(resolve => setTimeout(resolve, 3000));

    this.busy = true;

    const observable = await this.Api.GetListAll(this.pageIndex+1, this.pageSize);
    observable.subscribe({
      next: (response) => {
        if (response.success) {

          this.list$ = response.data.data.items;
          this.length = response.data.data.totalCount;
          this.pageIndex = response.data.data.pageNumber-1;

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

  async onCancel(id: string) {
    this.busy = true;
    this.ClearErrorList();
    const record: CancelSaleResponseDto = { Id: id }

    await (await this.Api.Cancel(record)).subscribe({
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
        
      }
    });
    
    this.LoadList();    
  }

  async onModify(id: string) {
    this.router.navigate(['/sale/modify/' + id]);    
  }
}


