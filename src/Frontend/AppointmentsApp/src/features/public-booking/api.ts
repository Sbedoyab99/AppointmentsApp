import { httpClient } from "../../shared/api/httpClient";
import type { ApiResponseData } from "../../shared/types/api";
import type { BusinessProfileResponse } from "../business-profile/types";

export async function getPublicBusinessProfile(
  businessProfileId: string,
): Promise<ApiResponseData<BusinessProfileResponse>> {
  const response = await httpClient.get<ApiResponseData<BusinessProfileResponse>>(
    `/api/public/business-profile?businessProfileId=${businessProfileId}`,
  );
  return response.data;
}
