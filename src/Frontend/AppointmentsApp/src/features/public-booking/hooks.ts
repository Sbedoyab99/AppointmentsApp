import { useQuery } from "@tanstack/react-query";
import { getPublicBusinessProfile } from "./api";

const PUBLIC_BUSINESS_PROFILE_QUERY_KEY = (id: string) => [
  "public-business-profile",
  id,
];

export function usePublicBusinessProfile(businessProfileId: string) {
  return useQuery({
    queryKey: PUBLIC_BUSINESS_PROFILE_QUERY_KEY(businessProfileId),
    queryFn: () => getPublicBusinessProfile(businessProfileId),
    retry: 1,
    staleTime: 10 * 60 * 1000, // 10 minutos
    enabled: !!businessProfileId, // Solo hacer la query si hay un ID válido
  });
}
