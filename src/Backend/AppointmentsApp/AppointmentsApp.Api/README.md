# AppointmentsApp.Api

## 📘 Descripción

La capa **Api** es la **puerta de entrada a la aplicación**.  
Su función principal es exponer los **endpoints HTTP** que serán consumidos por los clientes externos (frontend web, aplicaciones móviles, integraciones, etc.).

## 📁 Estructura de Carpetas

- **Controllers/**  
  Contiene los controladores que exponen los endpoints de la API.  
  Cada controlador debe estar enfocado en un módulo o entidad del negocio.

- **Middlewares/**  
  Contiene los middlewares personalizados (autenticación, validación, logging, manejo de errores, etc.).

- **appsettings.json**  
  Archivo de configuración de entorno (cadenas de conexión, claves API, parámetros globales).

- **Program.cs**  
  Punto de entrada de la aplicación donde se registran los servicios, middlewares y dependencias.

---

## 🧠 Buenas Prácticas

- Los controladores **no deben contener lógica de negocio**, solo orquestar llamadas a servicios de `Infrastructure`.
- Utiliza **ApiResponse** o **ApiResponseData\<T>** para estandarizar las respuestas.
- Configura **middleware globales** para manejo de errores, logs y validaciones.
- Mantén los endpoints limpios y autocontenidos.