import { httpClient } from "../../shared/api/httpClient";
import type { ApiResponseData } from "../../shared/types/api";
import type {
  BusinessProfileResponse,
  UpdateBusinessProfileRequest,
} from "./types";

export async function getBusinessProfile(): Promise<
  ApiResponseData<BusinessProfileResponse>
> {
  const response = await httpClient.get<ApiResponseData<BusinessProfileResponse>>(
    "/api/admin/business-profile",
  );
  return response.data;
}

export async function updateBusinessProfile(
  payload: UpdateBusinessProfileRequest,
): Promise<ApiResponseData<BusinessProfileResponse>> {
  const response = await httpClient.put<ApiResponseData<BusinessProfileResponse>>(
    "/api/admin/business-profile",
    payload,
  );
  return response.data;
}
