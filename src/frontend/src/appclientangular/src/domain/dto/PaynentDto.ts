export interface PaynentDto {
  id: string;
  saleId: string;
  total: number;
  status: PaymentStatusDto;
}

export enum PaymentStatusDto {
  Create = 0,
  Waiting = 1,
  Cancelled = 2,
  Sucefull = 3
}

