import type { ApiResponseData } from "../../shared/types/api";

export interface LoginRequestDto {
  email: string;
  password: string;
}

export interface LoginCandidateTenantDto {
  tenantId: string;
  tenantName: string;
  isOwner: boolean;
}

export interface LoginResponseDto {
  userId: string;
  requiresTenantSelection: boolean;
  tenants: LoginCandidateTenantDto[];
  message?: string;
}

export interface SelectTenantRequestDto {
  userId: string;
  tenantId: string;
}

export interface AuthSessionResponseDto {
  accessToken: string;
  refreshToken: string;
  expiresAtUtc: string;
  tenantId: string;
}

export interface AuthSession {
  accessToken: string;
  refreshToken: string;
  expiresAtUtc: string;
  tenantId: string;
  tenantName?: string;
}

export type LoginApiResponse = ApiResponseData<LoginResponseDto>;
export type SelectTenantApiResponse = ApiResponseData<AuthSessionResponseDto>;

const AUTH_SESSION_STORAGE_KEY = "appointmentsapp.auth.session";

export function saveAuthSession(session: AuthSession): void {
  localStorage.setItem(AUTH_SESSION_STORAGE_KEY, JSON.stringify(session));
}

export function getAuthSession(): AuthSession | null {
  const rawSession = localStorage.getItem(AUTH_SESSION_STORAGE_KEY);
  if (!rawSession) {
    return null;
  }

  try {
    return JSON.parse(rawSession) as AuthSession;
  } catch {
    localStorage.removeItem(AUTH_SESSION_STORAGE_KEY);
    return null;
  }
}
