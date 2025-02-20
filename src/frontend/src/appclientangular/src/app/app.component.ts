import { Component } from '@angular/core';
import { environment } from '../environments/environment.development';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'AppAngular';
  constructor() {
    console.log('API URL:', environment.ApiUrlSale);
    console.log('API URL:', environment.production);
  }
}
