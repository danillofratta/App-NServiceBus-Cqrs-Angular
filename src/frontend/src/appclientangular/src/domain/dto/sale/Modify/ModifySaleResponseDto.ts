import { SaleItemStatusDto } from "../enum/SaleItemStatusDto";

export interface ModifySaleResponseDto {
  Id: string;
  customerId: string;
  customerName: string;
  branchId: string;
  branchName: string;
  saleItems: ModifySaleItemResponseDto[];
}

export interface ModifySaleItemResponseDto {

  ProductId: string;
  ProductName: string;
  Quantity: number;
  UnitPrice: number;
  Discount: number
  TotalPrice: number
  Status: SaleItemStatusDto;
}

