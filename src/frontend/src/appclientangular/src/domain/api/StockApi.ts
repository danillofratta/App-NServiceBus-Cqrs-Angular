import { Injectable } from '@angular/core';
import { API } from './API';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { StockDto } from '../dto/StockDto';
import { environment } from '../../environments/environment.development';
import { ApiResponseListPaginationDto } from '../dto/apibase/ApiResponseListPaginationDto';
import { Observable } from 'rxjs';
import { ApiResponseDto } from '../dto/apibase/ApiResponseDto';


@Injectable({
    providedIn: 'root'
})
export class StockApi extends API {
  
  constructor(
    protected override http: HttpClient,    
    protected override router: Router
  ) {
    super(http, router);

    this._baseurl = environment.ApiUrlStock;   
    this._endpoint = "api/v1/stock/";
  }
  

  async GetListAll(pageNumber: number, pageSize: number): Promise<Observable<ApiResponseListPaginationDto<StockDto[]>>> {
    let filter = "GetList?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "& isDescending=false";
    return this._http.get<ApiResponseListPaginationDto<StockDto[]>>(`${this._baseurl + this._endpoint + filter}`);
  }

  async Save(dto: StockDto): Promise<Observable<ApiResponseDto<StockDto>>> {
    return this._http.post<ApiResponseDto<StockDto>>(`${this._baseurl + this._endpoint }`, dto);
  }  
}
