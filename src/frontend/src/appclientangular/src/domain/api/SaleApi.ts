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


@Injectable({
    providedIn: 'root'
})
export class SaleApi extends API {
  
  constructor(
    protected override http: HttpClient,    
    protected override router: Router
  ) {
    super(http, router);

    this._baseurl = environment.ApiUrlSale;

    this._endpoint = "api/v1/sales/";
  }
  
  async GetListAll(pageNumber: number, pageSize: number): Promise<Observable<ApiResponseListPaginationDto<GetSaleResponseDto[]>>> {
    let filter = "GetList?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "& isDescending=false";
    return this._http.get<ApiResponseListPaginationDto<GetSaleResponseDto[]>>(`${this._baseurl + this._endpoint + filter}`);    
  }

  async GetById(id: string) {
    return this._http.get<ApiResponseDto<GetSaleResponseDto>>(`${this._baseurl + this._endpoint + id}`);
  }

  async Create(record: CreateSaleRequestDto) {
    //return this._http.post(`${this._baseurl + this._endpoint}`, record).subscribe();
    return this._http.post<ApiResponseDto<CreateSaleRequestDto>>(`${this._baseurl + this._endpoint}`, record);
  }

  async Modify(record: ModifySaleResponseDto) {
    return this._http.put(`${this._baseurl + this._endpoint}`, record).subscribe();
  }

  async Cancel(record: CancelSaleResponseDto) {
    return this._http.put<ApiResponseDto<CancelSaleResponseDto>>(`${this._baseurl + this._endpoint + 'cancel'}`, record);
  }

}
