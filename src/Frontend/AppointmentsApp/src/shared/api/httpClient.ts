import axios from "axios";
import { getAuthSession } from "../../features/auth/types";
import { appEnv } from "../config/env";

export const httpClient = axios.create({
  baseURL: appEnv.apiBaseUrl,
  headers: {
    "Content-Type": "application/json",
  },
});

httpClient.interceptors.request.use((config) => {
  config.headers["x-api-key"] = appEnv.apiKey;

  const session = getAuthSession();
  if (session?.accessToken) {
    config.headers.Authorization = `Bearer ${session.accessToken}`;
  }

  return config;
});
