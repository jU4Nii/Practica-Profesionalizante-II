# Uso de Agentes, Skills y/o MCPs en el proyecto

**Materia:** Práctica Profesionalizante II  
**Proyecto:** BarberManager (API ASP.NET Core 8 + Minimal APIs)  
**Integrantes:** [MarianoAressi-JuanIgnacioBonora]

---

## Idea elegida

La opción elegida fue **agilizar el proceso de desarrollo con IA**.

En este proyecto cada recurso de la API se arma siempre igual: entidad, DTO, `DbSet` en `AppDbContext`, repositorio (interfaz y clase en el mismo archivo), lógica, endpoints Minimal API y registro de DI en `Program.cs`. Eso se presta a copiar mal una capa, olvidar el `AddScoped` o meter un controller donde el resto del código no los usa.

La idea fue dejar esa receta escrita como **regla de Cursor**, para que el agente cree APIs nuevas con la misma arquitectura, sin tener que explicarle el proyecto en cada chat.

---

## Herramienta utilizada

| Qué | Detalle |
| --- | --- |
| Entorno | [Cursor](https://cursor.com) |
| Modo | Agent (el agente lee el repo, escribe archivos y ejecuta comandos) |
| Mecanismo | **Cursor Rule** en `.cursor/rules/crear-api.mdc` |
| Frontmatter de la regla | `alwaysApply: true` (se aplica en todos los chats de este proyecto) |
| Skills / MCP | En esta prueba **no** se usó un Skill ni un MCP propio. La automatización es la regla. |

---

## Cómo se implementó

1. Se eligió documentar el flujo real del proyecto 
2. El **agente de Cursor analizó código existente** de `Endpoints`, `Repositorios` y `Logica` (por ejemplo clientes, productos y caja) y redactó la regla `.cursor/rules/crear-api.mdc`.
3. La regla fija el orden de capas, namespaces, convenciones de rutas, autenticación JWT y qué **no** hacer (controllers, SQL en endpoints, interfaz en archivo aparte).
4. Para **probarla**, en un chat de Agent se pidió crear la API de **Proveedores** (nombre, teléfono y email), sin volver a pegar la arquitectura en el prompt: la regla ya estaba activa.

Resultado verificado en el repo: se crearon las capas, se registró DI y se agregó la migración EF `AgregarProveedores`.

---

## Contenido de la regla

Archivo: [`.cursor/rules/crear-api.mdc`](../.cursor/rules/crear-api.mdc)

La regla indica:

- **No usar Controllers.** El proyecto es Minimal APIs.
- **Orden de capas:** `Entidades` → `Logica/DTOs` → `Datos` (`DbSet`) → `Repositorios` → `Logica` → `Endpoints` → `Program.cs`.
- **Entidad:** `BarberManagerAPIs.Entidades`, con `Id` y propiedades de persistencia, sin lógica ni EF.
- **DTO:** `BarberManagerAPIs.Logica.DTOs`, contrato de entrada POST/PUT, en general **sin `Id`**. Si el resto de APIs devuelven la entidad, no devolver el DTO.
- **DbSet:** `public DbSet<{Nombre}> {Nombres} { get; set; }` y migración EF si cambia el esquema.
- **Repositorio:** en el **mismo archivo**, `I{Nombre}Repository` + `{Nombre}Repository`. Solo `AppDbContext`. Métodos habituales: `ObtenerTodos`, `ObtenerPorId`, `Agregar`, `Guardar`, `Eliminar` si aplica.
- **Lógica:** en el **mismo archivo**, `I{Nombre}Logica` + `{Nombre}Logica`. Valida el DTO, mapea a entidad, devuelve `null`/`false` si falla. No usa `Results.*`.
- **Endpoints:** clase `static`, extensión `Map{Nombre}Endpoints`. Inyecta **solo** la lógica (nunca el repositorio). Rutas en español, minúsculas, plural (`/clientes`, `/productos`).
- **Auth:** hay `FallbackPolicy`; todo pide JWT salvo `.AllowAnonymous()`. Administración: `.RequireAuthorization("Admin")` o `MapGroup("/recurso").RequireAuthorization("Admin")` (como Caja).
- **DELETE** solo si el dominio ya lo usa (ej. productos, turnos).

Registro esperado en `Program.cs`:

```csharp
builder.Services.AddScoped<I{Nombre}Repository, {Nombre}Repository>();
builder.Services.AddScoped<I{Nombre}Logica, {Nombre}Logica>();
// ...
app.Map{Nombre}Endpoints();
```

---

## Ejemplo de uso

### Prompt

```text
Creá una API para gestionar Proveedores con nombre, teléfono y email, basandote en @.cursor/Rules/crearapi.mdc
```

No hizo falta describir capas, JWT ni Minimal APIs: eso está en la regla.

### Endpoints generados

Grupo `/proveedores` con `.RequireAuthorization("Admin")` (JWT + rol `Administrador`). Misma política de Caja; más estricta que clientes (en clientes el GET/POST usan el JWT por `FallbackPolicy` y el PUT pide Admin).

| Método | Ruta | Respuesta |
| --- | --- | --- |
| `GET` | `/proveedores` | `200` con la lista de entidades |
| `GET` | `/proveedores/{id}` | `200` o `404` |
| `POST` | `/proveedores` | `201` `{ id, nombre, mensaje }` o `400` `{ mensaje: "Datos inválidos" }` |
| `PUT` | `/proveedores/{id}` | `200` con mensaje, `404` si no existe, `400` si el DTO es inválido |

**Sin `DELETE`**, igual que clientes. La regla dice que DELETE solo va si el dominio ya lo usa (productos, turnos). El agente lo respetó.

Validación en lógica: `Nombre`, `Telefono` y `Email` no pueden ser nulos ni solo espacios. No hay validación de formato de email.

### Archivos creados

| Archivo | Rol |
| --- | --- |
| `Entidades/Proveedor.cs` | Entidad (`Id`, `Nombre`, `Telefono`, `Email`) |
| `Logica/DTOs/ProveedorDTO.cs` | DTO de entrada, sin `Id` |
| `Repositorios/ProveedorRepository.cs` | `IProveedorRepository` + `ProveedorRepository` |
| `Logica/ProveedorLogica.cs` | `IProveedorLogica` + `ProveedorLogica` |
| `Endpoints/ProveedorEndpoints.cs` | Minimal APIs del grupo `/proveedores` |
| `Migrations/20261030205231_AgregarProveedores.cs` | Migración EF (tabla `Proveedores`) |
| `Migrations/20261030205231_AgregarProveedores.Designer.cs` | Designer de la migración |

### Archivos modificados

| Archivo | Cambio |
| --- | --- |
| `Datos/AppDbContext.cs` | `public DbSet<Proveedor> Proveedores { get; set; }` |
| `Program.cs` | `AddScoped` de repositorio y lógica + `app.MapProveedorEndpoints()` |
| `Migrations/AppDbContextModelSnapshot.cs` | Snapshot actualizado por EF |

### Fragmento de los endpoints

```csharp
var group = app.MapGroup("/proveedores").RequireAuthorization("Admin");

group.MapGet("", async (IProveedorLogica logica) =>
{
    var proveedores = await logica.ObtenerTodos();
    return Results.Ok(proveedores);
});
```


---

## Conclusiones

### Qué funcionó

- El agente **siguió la arquitectura** del proyecto: no inventó controllers ni metió EF en los endpoints.
- **Dedujo que Proveedores no lleva DELETE**, igual que clientes, porque la regla lo limita a dominios que ya lo usan.
- Un prompt corto alcanzó para generar entidad, DTO, repositorio, lógica, endpoints, `DbSet`, DI y migración.
- La autorización de administración se agrupó con `MapGroup`, como Caja.

### Limitaciones

- La migración se **generó** (`AgregarProveedores`) pero **no se aplicó**: `dotnet ef database update` falló porque Azure SQL (`BarberManagerDB` en `barber-manager-server.database.windows.net`) no estaba disponible. Hay que correr `dotnet ef database update` cuando el servidor esté activo.
- El código generado **siempre hay que revisarlo** (nombres de propiedades, si todo el CRUD debe ser Admin, si hace falta DELETE, formato de email, relaciones con productos, etc.).
- La regla no cubre la UI MVC en `BarberManager.Web` ni tests.

### Mejoras posibles

- Una **regla para tests** (mismo estilo, a partir de tests que ya existan).
- Una **regla para pantallas** en `BarberManager.Web` (controller, view model, vista, menú).
- Un **MCP de base de datos** para aplicar migraciones, listar tablas o verificar el esquema sin copiar connection strings al chat.

---

## Cómo reproducirlo

Para que otra persona use la misma regla en este repo:

1. Clonar el repositorio y abrir la carpeta del proyecto en **Cursor**.
2. Comprobar que existe `.cursor/rules/crear-api.mdc` (con `alwaysApply: true` se carga sola).
3. Abrir un chat en modo **Agent**.
4. Pedir, por ejemplo:

   ```text
   Creá una API para X
   ```

   o, más concreto:

   ```text
   Creá una API para gestionar Proveedores con nombre, teléfono y email
   ```

5. Revisar los archivos generados (capas, `Program.cs`, migración).
6. Aplicar el esquema cuando la base esté disponible:

   ```bash
   dotnet ef database update
   ```

7. Probar en Swagger (`/swagger` en Development) con un JWT de usuario con rol `Administrador`.
