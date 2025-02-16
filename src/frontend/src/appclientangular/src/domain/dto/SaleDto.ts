export interface SaleDto {
    id: number;
    idcustomer: number;
    idproduct: number;
    idorder: number;
    nameproduct: string;
    value: number;
    createAt: Date;
    idstatus: number;
    namestatus: string;
    amount: number;
}
