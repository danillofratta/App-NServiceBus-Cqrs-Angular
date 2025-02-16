import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class SignalRSaleService {
  private hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      //.withUrl('https://localhost:7276/notificationHub', {
      .withUrl(environment.ApiUrlSaleSignal, {
        withCredentials: false, // Ensure credentials are included
        skipNegotiation: true,  // Importante para evitar erros de negociação
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()      
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR Connected'))
      .catch(err => console.error('Error connecting to SignalR: ', err));
  }

  onGetListSaleUpdated(callback: (dataList: any[]) => void): void {
    this.hubConnection.on('GetListSale', callback);
  }

}
