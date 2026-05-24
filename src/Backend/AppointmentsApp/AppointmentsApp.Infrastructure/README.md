# AppointmentsApp.Infrastructure

## 📘 Descripción

La capa **Infrastructure** representa el **segundo nivel en la jerarquía**.  
Aquí se encuentran las implementaciones de los **servicios principales** que se consumen desde los endpoints de la capa `Api`, así como la **lógica de negocio**, el **acceso a bases de datos**, y las **configuraciones de persistencia**.

## 📁 Estructura de Carpetas

- **Data/**  
  Contiene los `DbContext` y las configuraciones de Entity Framework Core (mapeos, relaciones, constraints, etc.).

- **Interfaces/**  
  Define las interfaces de los servicios y repositorios utilizados dentro de la capa de infraestructura.

- **Migrations/**  
  Archivos generados por EF Core para la gestión del esquema de base de datos.

- **Services/**  
  Implementaciones de los servicios principales, incluyendo la lógica de negocio que interactúa directamente con los repositorios.

---

## 🧠 Buenas Prácticas

- La lógica de negocio debe residir aquí, no en la capa `Api`.
- Todas las operaciones de base de datos deben realizarse mediante contextos o repositorios definidos aquí.
- No incluir dependencias hacia capas superiores (`Api` o externas a la arquitectura).
- Utiliza **ActionResponse\<T>** para estandarizar las respuestas de los servicios.
- Mantener interfaces claras y desacopladas.
