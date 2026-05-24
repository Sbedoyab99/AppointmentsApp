# 🧩 Backend Template (.NET 8)

Esta plantilla base define el estándar de desarrollo backend en la empresa para proyectos construidos sobre **.NET 8**, siguiendo una **arquitectura por capas de tipo hexagonal**.

El objetivo es proporcionar una estructura clara, escalable y desacoplada que facilite la mantenibilidad, la estandarización de procesos y la integración continua entre proyectos.

Asegurate de revisar el README.md dentro de cada uno de los proyectos para comprender mejor su funcionamiento y utilidad.

---

## 🏗️ Arquitectura General

La solución se compone de cuatro proyectos principales, organizados jerárquicamente:

AppointmentsApp.Api
│
├── AppointmentsApp.Infrastructure
│
├── AppointmentsApp.Application
│
└── AppointmentsApp.Domain


### Jerarquía de Dependencias

Cada capa puede depender de las capas inferiores, pero **nunca de las superiores**.  
Por ejemplo:
- `Api` puede utilizar `Infrastructure`, `Application` y `Domain`.
- `Infrastructure` puede utilizar `Application` y `Domain`.
- `Application` puede utilizar `Domain`.
- `Domain` no depende de ninguna otra capa.

Este flujo garantiza el principio fundamental de la **arquitectura hexagonal**:  
➡️ *Las capas externas dependen de las internas, pero las internas nunca dependen de las externas.*

| Capa | Rol | Puede depender de |
|------|------|------------------|
| `Api` | Presentación / Endpoints | Infrastructure, Application, Domain |
| `Infrastructure` | Servicios principales y acceso a datos | Application, Domain |
| `Application` | Integraciones externas y utilidades | Domain |
| `Domain` | Lógica pura del negocio | — |

---

## 🚀 Propósito de Cada Capa

### 1️⃣ `AppointmentsApp.Api` — *Capa de Presentación*
Es la **puerta de entrada a la aplicación**.  
Expone los endpoints que serán consumidos por el cliente (web, móvil, etc.) y es responsable de manejar las peticiones HTTP, respuestas, autenticación, y middleware global.

### 2️⃣ `AppointmentsApp.Infrastructure` — *Capa de Servicios y Acceso a Datos*
Es el **segundo nivel de la jerarquía**.  
Contiene la implementación de los **servicios principales** que son consumidos por los endpoints de la capa `Api`.  
Además, gestiona todo lo relacionado con **acceso y configuración de bases de datos**, así como la **lógica de negocio** central que orquesta los flujos del sistema.

### 3️⃣ `AppointmentsApp.Application` — *Capa de Integraciones y Apoyo*
Es el **tercer nivel**.  
Aquí se ubican los **servicios que interactúan con terceros** (por ejemplo, APIs REST, servicios SOAP, colas, almacenamiento externo, etc.).  
También contiene los **Mappers**, **Helpers**, y **Validators**, que actúan como herramientas de apoyo a la lógica de negocio.

### 4️⃣ `AppointmentsApp.Domain` — *Capa de Dominio Puro*
Es la **base del sistema**.  
Define los **modelos de datos**, **entidades**, **DTOs**, **enumeraciones**, **excepciones personalizadas** y **respuestas estándar**.  
No debe contener dependencias hacia ningún framework ni hacia otras capas.

---

## 🧱 Principios Clave

- **Desacoplamiento:** Cada capa tiene una responsabilidad única y puede ser testeada de forma independiente.  
- **Sustituibilidad:** Cambiar una base de datos, un proveedor externo o un servicio no impacta en las demás capas.  
- **Reutilización:** Los componentes comunes pueden emplearse en diferentes proyectos sin modificaciones.  
- **Mantenibilidad:** La estructura estandarizada facilita el trabajo en equipo y la incorporación de nuevos desarrolladores.  
- **Escalabilidad:** Permite crecer en complejidad funcional sin perder coherencia estructural.

---

## ⚙️ Cómo Utilizar la Plantilla

Esta plantilla se encuentra configurada como una plantilla de .NET mediante la carpeta `.template.config` y el archivo `template.json`.

### 🧩 1. Clonar el repositorio base

```bash
git clone https://ServiciosNimbutech@dev.azure.com/ServiciosNimbutech/Plantillas_Base_Proyectos/_git/BackEnd_Estc_Hexagonal
cd BackEnd_Estc_Hexagonal
```

### ⚙️ 2. Instalar la plantilla localmente
Puedes hacerlo de dos maneras:
##### 🅰️ Instalar desde la carpeta actual (recomendada)
```bash
dotnet new --install .
```

##### 🅱️ Instalar desde una ruta específica
```bash
dotnet new --install "C:\ruta\al\repo\BackEnd_Estc_Hexagonal"
# o en macOS/Linux:
dotnet new --install /ruta/al/repo/BackEnd_Estc_Hexagonal
```

### 🔍 3. Verificar que la plantilla esté disponible
```bash
dotnet new list
```
Deberías ver algo como:
```pgsql
Template Name     Short Name     Language     Tags
---------------    ------------   ----------   --------------------------
Backend Template   backend-api    [C#]         WebAPI, CleanArchitecture, Backend
```

### 🏗️ 4. Crear un nuevo proyecto desde la plantilla

###### ⚠️ Importante: Antes de crear el nuevo proyecto, sal de la carpeta donde clonaste el repositorio de la plantilla.
###### De lo contrario, el nuevo proyecto se generará dentro de la plantilla original.
###### Ejecuta este comando dentro de la carpeta donde quieres que se genere el proyecto.

Ejecuta el siguiente comando para generar tu propio proyecto:
```bash
dotnet new backend-api -n {{NombreDelProyecto}}
```
Esto creará una carpeta llamada {{NombreDelProyecto}} con toda la estructura base ya personalizada.

Gracias a las propiedades del archivo template.json, todas las ocurrencias de AppointmentsApp en namespaces, archivos y carpetas serán reemplazadas automáticamente por {{NombreDelProyecto}}.

### 🧪 5. Compilar y ejecutar el proyecto
```bash
cd {{NombreDelProyecto}}
dotnet build
dotnet run --project {{NombreDelProyecto}}
```

Abre el navegador en https://localhost:\<puerto> para acceder al proyecto o a su documentación Swagger (si está configurada).

### 🔧 6. Personalización posterior
- Ajusta appsettings.json con las cadenas de conexión y configuraciones del entorno.

- Registra tus servicios, contextos e inyecciones en Program.cs.

- Agrega módulos o controladores nuevos siguiendo las convenciones establecidas.

- Verifica que los namespaces correspondan a tu nuevo nombre de proyecto.

### 🔧 7. Desinstalar la plantilla (si es necesario)

Si se necesita actualizar la plantilla a una version nueva debemos primero desinstalarla, hacer pull al repo con los cambios y volver a instalarla.
```bash
dotnet new --uninstall .
# o
dotnet new --uninstall "C:\ruta\al\repo\AppointmentsApp"
```

---

## 🧩 Ejecución de migraciones (Entity Framework Core)

El proyecto utiliza **Entity Framework Core 8** para el manejo del modelo de datos y las migraciones.
Todas las operaciones deben ejecutarse desde la **consola del Package Manager** de Visual Studio o desde la **CLI de .NET** (PowerShell o CMD), asegurándose de que el Startup Project sea `{{NombreDelProyecto}}.Api`.

### 📘 Comandos principales

#### ➕ Crear una nueva migración
```bash
Add-Migration {{NombreMigracion}} -Context DataContext -Project "{{NombreDelProyecto}}.Infrastructure" -StartupProject "{{NombreDelProyecto}}.Api"
```
Crea una nueva migracion en la carpeta Migrations con el nombre `{{NombreMigracion}}_{{Timestamp}}`.

#### 🔄 Actualizar la base de datos
```bash
Update-Database -Context DataContext -Project "{{NombreDelProyecto}}.Infrastructure" -StartupProject "{{NombreDelProyecto}}.Api"
```
Aplica todas las migraciones pendientes a la base de datos configurada.

#### ❌ Eliminar la última migración
```bash
Remove-Migration -Context DataContext -Project "{{NombreDelProyecto}}.Infrastructure" -StartupProject "{{NombreDelProyecto}}.Api"
```
Revertir la última migración creada (solo si no ha sido aplicada a la base de datos).

#### 📜 Listar las migraciones existentes
```bash
Get-Migration -Context DataContext -Project "{{NombreDelProyecto}}.Infrastructure" -StartupProject "{{NombreDelProyecto}}.Api"
```
Muestra todas las migraciones registradas en el proyecto.

### ⚠️ Recomendaciones
- Antes de ejecutar cualquier comando, asegúrate de estar en el **directorio raíz de la solución** (no dentro de la carpeta de la plantilla).

- Verifica que el **connection string** correcto esté configurado en `appsettings.json` o en el entorno actual.

- Si trabajas con múltiples `DbContext`, recuerda especificar el contexto correspondiente con el parámetro `-Context`.