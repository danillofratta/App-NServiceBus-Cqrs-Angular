import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment.development';


@Injectable({
  providedIn: 'root'
})
export class API {

  protected _baseurl = "";
  protected _endpoint = "";

  protected _http: HttpClient;
  protected _router: Router

  constructor(
    protected http: HttpClient,
    protected router: Router
  ) {
    this._http = http;
    this._router = router;
   
    //this._baseurl = environment.ApiUrl;
    //console.log(environment.production);    
  }
}
