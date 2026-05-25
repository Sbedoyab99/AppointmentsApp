const DEFAULT_API_BASE_URL = 'http://localhost:5000'
const DEFAULT_API_KEY = '00000000-0000-0000-0000-000000000000'

export const appEnv = {
  apiBaseUrl: import.meta.env.VITE_API_BASE_URL ?? DEFAULT_API_BASE_URL,
  apiKey: import.meta.env.VITE_API_KEY ?? DEFAULT_API_KEY,
} as const
