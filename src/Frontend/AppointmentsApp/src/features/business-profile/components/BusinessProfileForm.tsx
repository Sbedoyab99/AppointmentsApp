import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import {
  updateBusinessProfileSchema,
  type UpdateBusinessProfileFormData,
} from "../schemas";
import type { BusinessProfileResponse } from "../types";

interface BusinessProfileFormProps {
  profile?: BusinessProfileResponse;
  isLoading?: boolean;
  isMutating?: boolean;
  onSubmit: (data: UpdateBusinessProfileFormData) => void;
}

export function BusinessProfileForm({
  profile,
  isLoading = false,
  isMutating = false,
  onSubmit,
}: Readonly<BusinessProfileFormProps>) {
  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<UpdateBusinessProfileFormData>({
    resolver: zodResolver(updateBusinessProfileSchema),
  });

  useEffect(() => {
    if (profile) {
      reset({
        name: profile.name,
        tradeName: profile.tradeName,
        description: profile.description,
        phone: profile.phone,
        address: profile.address,
        timeZone: profile.timeZone,
        contactEmail: profile.contactEmail,
      });
    }
  }, [profile, reset]);

  const isSubmitting = isLoading || isMutating;

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6 p-6">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {/* Name */}
        <div className="md:col-span-2">
          <label className="block text-sm font-medium text-gray-700" htmlFor="nombre">
            Nombre del Negocio *
          </label>
          <input
            type="text"
            {...register("name")}
            id="nombre"
            disabled={isSubmitting}
            className="mt-1 w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100"
            placeholder="Ej: Barbería Los Amigos"
          />
          {errors.name && (
            <p className="mt-1 text-sm text-red-600">{errors.name.message}</p>
          )}
        </div>

        {/* Trade Name */}
        <div>
          <label className="block text-sm font-medium text-gray-700" htmlFor="nombreComercial">
            Nombre Comercial *
          </label>
          <input
            type="text"
            {...register("tradeName")}
            id="nombreComercial"
            disabled={isSubmitting}
            className="mt-1 w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100"
            placeholder="Ej: LosAmigos"
          />
          {errors.tradeName && (
            <p className="mt-1 text-sm text-red-600">
              {errors.tradeName.message}
            </p>
          )}
        </div>

        {/* Time Zone */}
        <div>
          <label className="block text-sm font-medium text-gray-700" htmlFor="zonaHoraria">
            Zona Horaria *
          </label>
          <input
            type="text"
            {...register("timeZone")}
            id="zonaHoraria"
            disabled={isSubmitting}
            className="mt-1 w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100"
            placeholder="Ej: America/Bogota"
          />
          {errors.timeZone && (
            <p className="mt-1 text-sm text-red-600">
              {errors.timeZone.message}
            </p>
          )}
        </div>

        {/* Contact Email */}
        <div>
          <label className="block text-sm font-medium text-gray-700" htmlFor="correoContacto">
            Correo de Contacto *
          </label>
          <input
            type="email"
            {...register("contactEmail")}
            id="correoContacto"
            disabled={isSubmitting}
            className="mt-1 w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100"
            placeholder="Ej: contacto@negocio.com"
          />
          {errors.contactEmail && (
            <p className="mt-1 text-sm text-red-600">
              {errors.contactEmail.message}
            </p>
          )}
        </div>

        {/* Phone */}
        <div>
          <label className="block text-sm font-medium text-gray-700" htmlFor="telefono">
            Teléfono
          </label>
          <input
            type="text"
            {...register("phone")}
            id="telefono"
            disabled={isSubmitting}
            className="mt-1 w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100"
            placeholder="Ej: +57 300 123 4567"
          />
          {errors.phone && (
            <p className="mt-1 text-sm text-red-600">{errors.phone.message}</p>
          )}
        </div>

        {/* Address */}
        <div>
          <label className="block text-sm font-medium text-gray-700" htmlFor="direccion">
            Dirección
          </label>
          <input
            type="text"
            {...register("address")}
            id="direccion"
            disabled={isSubmitting}
            className="mt-1 w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100"
            placeholder="Ej: Carrera 7 #45-89, Bogotá"
          />
          {errors.address && (
            <p className="mt-1 text-sm text-red-600">
              {errors.address.message}
            </p>
          )}
        </div>

        {/* Description */}
        <div className="md:col-span-2">
          <label className="block text-sm font-medium text-gray-700" htmlFor="descripcion">
            Descripción
          </label>
          <textarea
            {...register("description")}
            id="descripcion"
            disabled={isSubmitting}
            rows={4}
            className="mt-1 w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100"
            placeholder="Describe tu negocio y sus servicios..."
          />
          {errors.description && (
            <p className="mt-1 text-sm text-red-600">
              {errors.description.message}
            </p>
          )}
        </div>
      </div>

      <div className="flex gap-3">
        <button
          type="submit"
          disabled={isSubmitting}
          className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:bg-gray-400 transition-colors"
        >
          {isMutating ? "Guardando..." : "Guardar Cambios"}
        </button>
      </div>
    </form>
  );
}
