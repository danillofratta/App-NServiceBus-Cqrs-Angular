import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { PaynentDto } from '../../../../../domain/dto/PaynentDto';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { PaymentApi } from '../../../../../domain/api/PaymentApi';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, of } from 'rxjs';
import { SignalRPaymentSevice } from '../../../../../domain/api/SignalRPaymentSevice';

@Component({
  selector: 'app-list-payment',
  templateUrl: './list-payment.component.html',
  styleUrl: './list-payment.component.css'
})
export class ListPaymentComponent implements OnInit, AfterViewInit {

  length = 0;
  pageSize = 10;
  pageIndex = 0;

  public list$: any[] = [];

  public busy = false;

  public _ListError: string[] = [];

  dataSource = new MatTableDataSource<PaynentDto>();

  displayedColumns = [ 'id', 'saleId', 'status', 'total'];

  @ViewChild(MatPaginator) paginator: any = MatPaginator;

  constructor(private Api: PaymentApi, private router: Router, private signal: SignalRPaymentSevice) {
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  async ngOnInit() {
    this.busy = true;

    await this.LoadList();

    this.signal.onGetListUpdated((updatedDataList) => {
      console.log('Lista de dados recebida:', updatedDataList);

      //const payments$: Observable<PaynentDto[]> = of(updatedDataList as PaynentDto[]);
      const payments$: PaynentDto[] = updatedDataList as PaynentDto[];

      console.log(payments$);

      this.list$ = payments$;

      this.dataSource.data = this.list$;

      //this.loadDataSource();
    });
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

    const observable = await this.Api.GetListAll(this.pageIndex + 1, this.pageSize);
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
  
  //async onCancel(id: string) {
  //  this.busy = true;
  //  this.ClearErrorList();
  //  const record: CancelSaleResponseDto = { Id: id }

  //  await (await this.Api.Cancel(record)).subscribe({
  //    next: (response) => {
  //      if (response.success) {
  //        this.router.navigate(['/sale']);
  //      } else {
  //        this._ListError.push(response.message);
  //      }
  //    },
  //    error: (error) => {
  //      console.error('Error occurred:', error);

  //      this._ListError.push(error.error.message);
  //    },
  //    complete: () => {

  //    }
  //  });

  //  this.LoadList();
  //}


}



