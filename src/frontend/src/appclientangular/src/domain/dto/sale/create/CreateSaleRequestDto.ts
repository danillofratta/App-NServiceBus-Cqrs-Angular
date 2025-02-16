export interface CreateSaleRequestDto {
  customerId: string;
  customerName: string;
  branchId: string;
  branchName: string;
  saleItens: CreateSaleItemRequestDto[];
}

export interface CreateSaleItemRequestDto {

  ProductId: string;
  ProductName: string;
  Quantity: number;
  UnitPrice: number;
  TotalPrice: number             
}


