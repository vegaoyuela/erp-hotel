# P02-F2 — Persistencia explícita

**Fecha de implementación:** 28 de octubre de 2026. Continuación del plan iniciado el 27 de septiembre.

## Resultado

Los 11 archivos de repositorio preparan altas, cambios y bajas sin confirmar la base. Se retiraron 56 llamadas internas a `SaveChangesAsync`: 54 se sustituyeron por persistencia explícita en sus consumidores y las dos del correlativo se trasladaron a un servicio de aplicación. No se modificó el esquema.

`IUnitOfWork` y `EfUnitOfWork` utilizan el mismo `ApplicationDbContext` de la solicitud. `SaveChangesAsync` persiste los cambios preparados; cuando existe una transacción exterior, no la confirma. El caso de uso mantiene la decisión de `CommitAsync` o rollback.

## Consumidores migrados

| Consumidor | Punto de persistencia |
|---|---|
| Clientes, huéspedes, proveedores, descuentos, habitaciones y tipos | El controlador guarda después de preparar la mutación y antes de responder. |
| Cuentas y asientos manuales | El guardado explícito ocurre antes de consultar o devolver el resultado. |
| Usuarios y autenticación | Intentos fallidos, último acceso, restablecimiento y desactivación se guardan explícitamente. El registro guarda usuario y rol juntos. |
| Roles | Alta y permisos se guardan juntos. Actualización y baja incluyen permisos, versión de seguridad y revocación de sesiones en una transacción exterior. |
| Configuración fiscal | La unidad de trabajo se ejecuta dentro de la transacción existente. La aprobación conserva su captura de conflictos de persistencia. |
| Autorizaciones y adjuntos | El guardado se mantiene dentro del bloque que elimina el archivo cuando falla la persistencia. |
| Auditoría | `AuditService` guarda la bitácora mientras mantiene el lock de cadena; participa en la transacción exterior cuando existe. |
| Correlativo CAI | `ICaiCorrelativeService` conserva validaciones y lock, guarda dentro de ese lock y participa en la transacción de emisión. El repositorio ya no asigna números. |
| Reservas, folios, caja, compras y documentos fiscales | Sus servicios de aplicación ya tenían persistencia y transacción explícitas. Los métodos de repositorio no utilizados dejan de guardar por su cuenta. |

## Regresiones verificadas

- Un proveedor y un huésped preparados por sus repositorios no son visibles desde otra conexión hasta el guardado explícito.
- Un guardado dentro de una transacción exterior no hace visibles los cambios antes del commit. El rollback conserva los valores originales y revierte la baja del huésped.
- Una restricción de unicidad fallida revierte todo el lote: tampoco sobrevive el proveedor válido preparado en el mismo guardado.
- Alta, edición, lectura posterior y baja funcionan para clientes, huéspedes, proveedores, descuentos, tipos, habitaciones y cuentas.
- El alta de rol conserva sus permisos; su actualización reemplaza permisos, conserva los seleccionados y persiste descripción y baja.
- El intento de acceso fallido incrementa el contador; el restablecimiento persiste contraseña, cambio obligatorio y reinicio de intentos; la desactivación persiste el estado del usuario.
- La suite existente vuelve a probar perfil fiscal, adjuntos, cadena de auditoría, emisión, correlativos, concurrencia, finanzas, inventario y respaldo real.

`scripts/check-repository-boundaries.mjs` se ejecuta desde `scripts/qa.sh`: rechaza guardados, escrituras SQL directas y apertura/confirmación de transacciones en repositorios. Esta regla evita reintroducir los puntos de persistencia implícitos.

## Dependencias corregidas durante QA

La primera auditoría npm del 28 de octubre detectó cinco alertas altas. `npm audit fix` actualizó el lockfile dentro de los rangos existentes de `package.json`, sin `--force`: `brace-expansion` 5.0.12, `follow-redirects` 1.16.2, `source-map-js` 1.2.2, Vite 8.3.5 y Zod 4.6.7, con sus dependencias compatibles. La ejecución inicial se conserva en [p02f2-qa-inicial.log](../evidencia/p02f2-qa-inicial.log); el resultado final corresponde al lockfile corregido.

La evidencia contiene el [inventario antes/después](../evidencia/p02f2-repositorios.json), el [resultado integral](../evidencia/p02f2-qa.log), la [comprobación EF](../evidencia/p02f2-modelo-ef.log) y el [resumen con hashes](../evidencia/p02f2-qa-resumen.json).

## Continuación

P02-02 queda completada para el alcance registrado. P02 permanece abierta por los contratos restantes de cálculo, auditoría e impresión; convenciones y descripción OpenAPI; paginación y tipos frontend; pendientes de seguridad y matriz formal de pruebas; y revisión técnica independiente. La validación de Windows 11 y la aceptación fiscal del hotel conservan sus tareas y evidencias propias en el plan general.
