import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  getBusinessProfile,
  updateBusinessProfile,
} from "./api";
import type { UpdateBusinessProfileRequest } from "./types";

const BUSINESS_PROFILE_QUERY_KEY = ["business-profile"];

export function useBusinessProfile() {
  return useQuery({
    queryKey: BUSINESS_PROFILE_QUERY_KEY,
    queryFn: getBusinessProfile,
    retry: 1,
    staleTime: 5 * 60 * 1000, // 5 minutos
  });
}

export function useUpdateBusinessProfile() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (payload: UpdateBusinessProfileRequest) =>
      updateBusinessProfile(payload),
    onSuccess: () => {
      // Invalidar la query para refrescar los datos
      queryClient.invalidateQueries({
        queryKey: BUSINESS_PROFILE_QUERY_KEY,
      });
    },
  });
}
