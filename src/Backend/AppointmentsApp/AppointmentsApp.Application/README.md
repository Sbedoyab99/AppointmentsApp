# AppointmentsApp.Application

## 📘 Descripción

La capa **Application** es el **tercer nivel en la jerarquía**.  
Aquí se definen los servicios encargados de **interactuar con sistemas externos** (APIs REST, servicios SOAP, colas, almacenamiento en la nube, etc.) y las clases de **apoyo funcional** al dominio.

## 📁 Estructura de Carpetas

- **Helpers/**  
  Funciones o clases auxiliares reutilizables para operaciones genéricas (formato, fechas, cálculos, etc.).

- **Interfaces/**  
  Contratos para los servicios de integración (por ejemplo, `IApiExternalService`, `IEmailSender`, `IFileStorageService`).

- **Mappers/**  
  Configuraciones para transformar entidades, DTOs y respuestas.

- **Services/**  
  Implementaciones de servicios de conexión con terceros o utilidades internas.

- **Validators/**  
  Validadores de entrada.

---

## 🧠 Buenas Prácticas

- No incluir lógica de negocio propia, solo la necesaria para integrar servicios externos.
- Mantener los mapeos y validaciones centralizados aquí.
- Utiliza **ActionResponse\<T>** para estandarizar las respuestas de los servicios.
- Evitar dependencias hacia `Infrastructure` o `Api`.
