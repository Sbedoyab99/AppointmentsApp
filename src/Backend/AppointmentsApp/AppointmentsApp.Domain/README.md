# AppointmentsApp.Domain

## 📘 Descripción

La capa **Domain** es el **fundamento de la arquitectura**, el **nivel más bajo y estable** del sistema.  
Define los elementos esenciales del dominio: modelos, entidades, DTOs, enumeraciones, excepciones y respuestas personalizadas.

## 📁 Estructura de Carpetas

- **DTOs/**  
  Objetos de transferencia de datos entre capas o servicios.

- **Entities/**  
  Entidades que representan las tablas o modelos del dominio del negocio.

- **Enums/**  
  Enumeraciones que definen valores fijos y estados del sistema.

- **Exceptions/**  
  Excepciones personalizadas para el manejo controlado de errores.

- **Responses/**  
  Clases de respuesta estándar, como `ActionResponse<T>` o `ApiResponseData<T>`.

---

## 🧠 Buenas Prácticas

- Esta capa no debe tener dependencias hacia ninguna otra.
- Las entidades deben reflejar fielmente el modelo de negocio.
- No incluir lógica de infraestructura ni dependencias a frameworks.
- Mantener las clases limpias, simples y enfocadas en la semántica del dominio.
