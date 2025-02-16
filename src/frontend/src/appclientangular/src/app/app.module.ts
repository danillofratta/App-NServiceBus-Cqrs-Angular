import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner.component';
import { HttpClientModule } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { SaleApi } from '../domain/api/SaleApi';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { ProductApi } from '../domain/api/ProductApi';
import { ProductAutoCompleteComponent } from './components/product-auto-complete/product-auto-complete.component';
import { ListSaleComponent } from './pages/dashboard/sale/list-sale/list-sale.component';
import { ModifySaleComponent } from './pages/dashboard/sale/modify-sale/modify-sale.component';
import { CreateSaleComponent } from './pages/dashboard/sale/create-sale/create-sale.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MatOptionModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { ShowErrorListComponent } from './components/show-error-list/show-error-list.component';
import { ProductComponent } from './pages/dashboard/product/product.component';
import { StockComponent } from './pages/dashboard/stock/stock.component';

@NgModule({
  declarations: [
    AppComponent,

    DashboardComponent,

    LoadingSpinnerComponent,
    
    ProductAutoCompleteComponent,
    ShowErrorListComponent,

    ProductComponent,
    StockComponent,

    CreateSaleComponent,
    ModifySaleComponent,
    ListSaleComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,

    HttpClientModule,

    MatIconModule,
    MatButtonModule,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    BrowserAnimationsModule,
    MatCardModule,
    MatOptionModule,
    MatAutocompleteModule    
  ],
  providers: [SaleApi, provideAnimationsAsync(),  ProductApi],
  bootstrap: [AppComponent]
})
export class AppModule {
}
