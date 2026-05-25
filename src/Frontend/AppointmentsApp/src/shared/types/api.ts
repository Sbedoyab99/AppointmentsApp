export interface ApiResponse {
  message?: string;
  statusCode: number;
}

export interface ApiResponseData<T> extends ApiResponse {
  data?: T;
}
