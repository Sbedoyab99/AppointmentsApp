import { z } from "zod";

export const updateBusinessProfileSchema = z.object({
  name: z
    .string()
    .min(2, "El nombre debe tener al menos 2 caracteres")
    .max(200, "El nombre no puede superar 200 caracteres"),
  tradeName: z
    .string()
    .min(2, "El nombre comercial debe tener al menos 2 caracteres")
    .max(150, "El nombre comercial no puede superar 150 caracteres"),
  description: z
    .string()
    .max(1000, "La descripción no puede superar 1000 caracteres")
    .optional(),
  phone: z
    .string()
    .max(30, "El teléfono no puede superar 30 caracteres")
    .optional(),
  address: z
    .string()
    .max(300, "La dirección no puede superar 300 caracteres")
    .optional(),
  timeZone: z
    .string()
    .min(1, "La zona horaria es obligatoria")
    .max(100, "La zona horaria no puede superar 100 caracteres"),
  contactEmail: z
    .email("Ingresa un correo electrónico válido")
    .max(100, "El correo de contacto no puede superar 100 caracteres"),
});

export type UpdateBusinessProfileFormData = z.infer<
  typeof updateBusinessProfileSchema
>;
