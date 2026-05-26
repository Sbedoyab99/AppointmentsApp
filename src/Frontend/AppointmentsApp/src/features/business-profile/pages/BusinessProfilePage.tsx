import { useState } from "react";
import { BusinessProfileForm } from "../components/BusinessProfileForm";
import { useBusinessProfile, useUpdateBusinessProfile } from "../hooks";
import type { UpdateBusinessProfileFormData } from "../schemas";

export function BusinessProfilePage() {
  const {
    data: profileResponse,
    isLoading,
    error: fetchError,
  } = useBusinessProfile();

  const {
    mutate: updateProfile,
    isPending: isMutating,
    error: updateError,
  } = useUpdateBusinessProfile();

  const [successMessage, setSuccessMessage] = useState<string>("");

  const handleSubmit = (data: UpdateBusinessProfileFormData) => {
    updateProfile(data, {
      onSuccess: () => {
        setSuccessMessage("Perfil actualizado correctamente");
        setTimeout(() => setSuccessMessage(""), 3000);
      },
    });
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-screen">
        <p className="text-lg text-gray-600">Cargando perfil del negocio...</p>
      </div>
    );
  }

  if (fetchError) {
    return (
      <div className="p-6">
        <div className="bg-red-50 border border-red-200 rounded-md p-4">
          <h2 className="text-lg font-medium text-red-800">
            Error al cargar el perfil
          </h2>
          <p className="text-red-700 mt-2">
            {fetchError instanceof Error
              ? fetchError.message
              : "Ocurrió un error inesperado"}
          </p>
        </div>
      </div>
    );
  }

  const profile = profileResponse?.data;

  return (
    <div className="max-w-4xl mx-auto">
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-gray-900">
          Perfil del Negocio
        </h1>
        <p className="text-gray-600 mt-1">
          Gestiona la información general de tu negocio
        </p>
      </div>

      {successMessage && (
        <div className="mb-4 p-4 bg-green-50 border border-green-200 rounded-md">
          <p className="text-green-800">{successMessage}</p>
        </div>
      )}

      {updateError && (
        <div className="mb-4 p-4 bg-red-50 border border-red-200 rounded-md">
          <p className="text-red-800">
            {updateError instanceof Error
              ? updateError.message
              : "Error al actualizar el perfil"}
          </p>
        </div>
      )}

      <div className="bg-white rounded-lg shadow">
        {profile && (
          <BusinessProfileForm
            profile={profile}
            isLoading={isLoading}
            isMutating={isMutating}
            onSubmit={handleSubmit}
          />
        )}
      </div>
    </div>
  );
}
