import { SaleItemStatusDto } from "../enum/SaleItemStatusDto";
import { SaleStatusDto } from "../enum/SaleStatusDto";

export interface CreateSaleResponseDto {
    Id: string;
    customerId: string;
    customerName: string;
    branchId: string;
    branchName: string;
    Status: SaleStatusDto;
    saleItems: CreateSaleItemResponseDto[];
}

export interface CreateSaleItemResponseDto {  
  ProductId: string;
  ProductName: string;
  Quantity: number;
  UnitPrice: number;
  TotalPrice: number
}
