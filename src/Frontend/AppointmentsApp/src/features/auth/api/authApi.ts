import { httpClient } from "../../../shared/api/httpClient";
import type {
  LoginApiResponse,
  LoginRequestDto,
  SelectTenantApiResponse,
  SelectTenantRequestDto,
} from "../types";

export async function loginRequest(
  payload: LoginRequestDto,
): Promise<LoginApiResponse> {
  const response = await httpClient.post<LoginApiResponse>(
    "/api/public/auth/login",
    payload,
  );
  return response.data;
}

export async function selectTenantRequest(
  payload: SelectTenantRequestDto,
): Promise<SelectTenantApiResponse> {
  const response = await httpClient.post<SelectTenantApiResponse>(
    "/api/public/auth/select-tenant",
    payload,
  );
  return response.data;
}
