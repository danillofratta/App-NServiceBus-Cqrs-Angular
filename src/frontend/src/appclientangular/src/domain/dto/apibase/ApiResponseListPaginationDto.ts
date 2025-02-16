export interface ApiResponseListPaginationDto<T> {
  items: T[];
  pageNumber: number;
  totalCount: number;
  totalPages: number;
}

export interface ApiResponseListPaginationDto<T> {
  success: boolean;
  message: string;
  data: ApiResponseListPaginationDto<T>;
}
