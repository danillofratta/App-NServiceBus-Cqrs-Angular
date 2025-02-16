import { SaleItemStatusDto } from "../enum/SaleItemStatusDto";
import { SaleStatusDto } from "../enum/SaleStatusDto";

export interface GetSaleResponseDto {
  id: string;
  number: number;
  customerId: string;
  customerName: string;
  branchId: string;
  branchName: string;
  Status: SaleStatusDto;
  totalAmount: number;
  saleItems: GetSaleItemResponseDto[];
}

export interface GetSaleItemResponseDto {
  Id: string;
  ProductId: string;
  ProductName: string;
  Quantity: number;
  UnitPrice: number;
  Discount: number;
  TotalPrice: number;
  Status: SaleItemStatusDto;
}

