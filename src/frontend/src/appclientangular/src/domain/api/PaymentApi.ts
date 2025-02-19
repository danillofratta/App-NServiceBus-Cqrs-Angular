import { Injectable } from '@angular/core';
import { API } from './API';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment.development';
import { CreateSaleRequestDto } from '../dto/sale/create/CreateSaleRequestDto';
import { ModifySaleResponseDto } from '../dto/sale/Modify/ModifySaleResponseDto';
import { GetSaleResponseDto } from '../dto/sale/get/GetSaleResponseDto';
import { ApiResponseListPaginationDto } from '../dto/apibase/ApiResponseListPaginationDto';
import { ApiResponseDto } from '../dto/apibase/ApiResponseDto';
import { CancelSaleResponseDto } from '../dto/sale/cancel/CancelSaleResponseDto';
import { Observable } from 'rxjs';
import { PaynentDto } from '../dto/PaynentDto';


@Injectable({
    providedIn: 'root'
})
export class PaymentApi extends API {
  
  constructor(
    protected override http: HttpClient,    
    protected override router: Router
  ) {
    super(http, router);

    this._baseurl = environment.ApiUrlPayment;

    this._endpoint = "api/v1/payment/";
  }
  
  async GetListAll(pageNumber: number, pageSize: number): Promise<Observable<ApiResponseListPaginationDto<PaynentDto[]>>> {
    let filter = "GetList?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&isDescending=false";
    return this._http.get<ApiResponseListPaginationDto<PaynentDto[]>>(`${this._baseurl + this._endpoint + filter}`);    
  }

}
