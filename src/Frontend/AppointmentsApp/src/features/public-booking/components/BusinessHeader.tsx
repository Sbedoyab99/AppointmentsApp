import type { BusinessProfileResponse } from "../../business-profile/types";

interface BusinessHeaderProps {
  profile?: BusinessProfileResponse;
  isLoading?: boolean;
}

export function BusinessHeader({ profile, isLoading = false }: Readonly<BusinessHeaderProps>) {
  if (isLoading) {
    return (
      <div className="bg-blue-50 border border-blue-200 rounded-lg p-6 mb-6 animate-pulse">
        <div className="h-6 bg-blue-200 rounded w-48 mb-2"></div>
        <div className="h-4 bg-blue-200 rounded w-full mb-2"></div>
        <div className="h-4 bg-blue-200 rounded w-3/4"></div>
      </div>
    );
  }

  if (!profile) {
    return null;
  }

  return (
    <div className="bg-gradient-to-r from-blue-50 to-blue-100 border border-blue-200 rounded-lg p-6 mb-6 shadow-sm">
      <h1 className="text-3xl font-bold text-gray-900 mb-2">{profile.name}</h1>

      {profile.description && (
        <p className="text-gray-700 mb-4">{profile.description}</p>
      )}

      <div className="flex flex-col gap-2 text-sm text-gray-600">
        {profile.address && (
          <div className="flex items-center gap-2">
            <span className="text-lg">📍</span>
            <span>{profile.address}</span>
          </div>
        )}

        {profile.phone && (
          <div className="flex items-center gap-2">
            <span className="text-lg">📞</span>
            <a
              href={`tel:${profile.phone}`}
              className="hover:text-blue-600 transition-colors"
            >
              {profile.phone}
            </a>
          </div>
        )}

        {profile.contactEmail && (
          <div className="flex items-center gap-2">
            <span className="text-lg">✉️</span>
            <a
              href={`mailto:${profile.contactEmail}`}
              className="hover:text-blue-600 transition-colors"
            >
              {profile.contactEmail}
            </a>
          </div>
        )}
      </div>
    </div>
  );
}
