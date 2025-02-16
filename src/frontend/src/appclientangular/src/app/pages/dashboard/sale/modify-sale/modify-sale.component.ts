import { Component, OnInit } from '@angular/core';
import { SaleApi } from '../../../../../domain/api/SaleApi';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ModifySaleResponseDto } from '../../../../../domain/dto/sale/Modify/ModifySaleResponseDto';
import { Observable, catchError, finalize, lastValueFrom, map, of } from 'rxjs';
import { GetSaleResponseDto } from '../../../../../domain/dto/sale/get/GetSaleResponseDto';
import { ApiResponseDto } from '../../../../../domain/dto/apibase/ApiResponseDto';
import { BasePage } from '../../../BasePage';

@Component({
  selector: 'app-modify-sale',
  templateUrl: './modify-sale.component.html',
  styleUrl: './modify-sale.component.css'
})
export class ModifySaleComponent implements OnInit {

  id: any;
  public record: Observable<ModifySaleResponseDto> = new Observable<ModifySaleResponseDto>();
  public form: FormGroup;
  public busy = false;

  constructor
    (
      private api: SaleApi,
      private fb: FormBuilder,
      private router: Router,
    private activatedRoute: ActivatedRoute)
  {
        
      this.form = this.fb.group({
      id: ['id', Validators.compose([
        Validators.required
      ])],
      customerId: ['customerId', Validators.compose([
        Validators.required
      ])],
      customerName: ['customerName', Validators.compose([
        Validators.required
      ])],

      branchId: ['branchId', Validators.compose([
              Validators.required
            ])],
      branchName: ['branchName', Validators.compose([
        Validators.required
      ])]
    });

  }

  async ngOnInit() {
    this.id = this.activatedRoute.snapshot.paramMap.get("id");
    this.busy = true;
    try {
      (await this.api.GetById(this.id)).subscribe(
      (response: ApiResponseDto<GetSaleResponseDto>) => {        
          if (response.success && response.data) {
            this.form.controls['id'].setValue(response.data.data.id);
            this.form.controls['customerId'].setValue(response.data.data.customerId);
            this.form.controls['customerName'].setValue(response.data.data.customerName);
            this.form.controls['branchId'].setValue(response.data.data.branchId);
            this.form.controls['branchName'].setValue(response.data.data.branchName);
          } else {
            console.error("Erro ao buscar dados:", response.errors);
          }
        },
        (error) => {
          console.error("Erro na requisição:", error);
        }
      );
    }
    catch (error) {

    }
    this.busy = false;
  }

  async modify() {

    if (this.form.valid) {
      this.busy = true;

      const record = this.form.value as ModifySaleResponseDto;

      try {
        await this.api.Modify(record);

        this.form.reset();

        this.router.navigate(['/sale']);    

      } catch (error) {
        console.error('Erro ao salvar o stock:', error);
      } finally {
        this.busy = false;
      }
    }
  }

}
