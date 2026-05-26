export interface BusinessProfileResponse {
  id: string;
  name: string;
  tradeName: string;
  description?: string;
  phone?: string;
  address?: string;
  timeZone: string;
  contactEmail: string;
}

export interface UpdateBusinessProfileRequest {
  name: string;
  tradeName: string;
  description?: string;
  phone?: string;
  address?: string;
  timeZone: string;
  contactEmail: string;
}
