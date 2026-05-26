import { useParams } from "react-router-dom";
import { BusinessHeader } from "../components/BusinessHeader";
import { usePublicBusinessProfile } from "../hooks";

export function PublicBookingPage() {
  const { businessProfileId } = useParams<{ businessProfileId: string }>();

  const {
    data: profileResponse,
    isLoading,
    error,
  } = usePublicBusinessProfile(businessProfileId || "");

  const profile = profileResponse?.data;

  return (
    <main className="min-h-screen bg-gray-50">
      <div className="max-w-4xl mx-auto px-4 py-8">
        <BusinessHeader profile={profile} isLoading={isLoading} />

        {error && (
          <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4 mb-6">
            <p className="text-yellow-800">
              No pudimos cargar la información del negocio. Por favor, intenta de nuevo más tarde.
            </p>
          </div>
        )}

        {!isLoading && (
          <div className="bg-white rounded-lg shadow-md p-8">
            <h2 className="text-2xl font-bold text-gray-900 mb-4">
              Agendar Cita
            </h2>
            <p className="text-gray-600">
              El formulario de reserva será implementado en pasos siguientes.
            </p>
          </div>
        )}
      </div>
    </main>
  );
}
