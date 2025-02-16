import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class SignalROrderService {
  private hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
    //.withUrl('https://localhost:7091/notificationHub', {
      .withUrl(environment.ApiUrlOrderSignal, {
        withCredentials: false, // Ensure credentials are included
        skipNegotiation: true,  // Importante para evitar erros de negociação
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()      
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR Connected'))
      .catch(err => console.error('Error connecting to SignalR: ', err));

    //this.hubConnection = new signalR.HubConnectionBuilder()
    //  .withUrl('https://localhost:7091/notificationHub') // SignalR hub URL
    //  .build();
  }


  onGetListOrderUpdated(callback: (dataList: any[]) => void): void {
    this.hubConnection.on('GetListOrder', callback);
  }

  onGetListSaleUpdated(callback: (dataList: any[]) => void): void {
    this.hubConnection.on('GetListSale', callback);
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

  receiveMessage(): Observable<string> {
    return new Observable<string>((observer) => {
      this.hubConnection.on('GetListOrder', (message: string) => {
        observer.next(message);
      });
    });
  }

  sendMessage(message: string): void {
    this.hubConnection.invoke('SendMessage', message);
  }

}
