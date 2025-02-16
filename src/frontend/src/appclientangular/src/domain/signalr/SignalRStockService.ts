import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class SignalRStockService {
  private hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      //.withUrl('http://localhost:5285/notificationHub', {
      .withUrl(environment.ApiUrlStockSignal, {
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


  onGetListStockUpdated(callback: (dataList: any[]) => void): void {
    this.hubConnection.on('GetListStock', callback);
  }

  startConnection(): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection
        .start()
        .then(() => {
          console.log('Connection established with SignalR hub');
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          console.error('Error connecting to SignalR hub:', error);
          observer.error(error);
        });
    });
  }

}
