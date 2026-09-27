# P02 — Ejecución de base técnica, contratos y seguridad

**Inicio:** 27 de septiembre de 2026

**Estado:** en ejecución; primera entrega transversal aprobada por pruebas automáticas

Este registro divide P02 en entregas comprobables. No se declara G2 mientras queden controladores con lógica de negocio, repositorios que confirman cambios por su cuenta o contratos sin prueba.

## Entrega P02-A — tiempo, errores y cadena de auditoría

### Implementado

- `IHotelClock` obtiene UTC mediante `TimeProvider` y convierte la fecha operativa con la zona `America/Tegucigalpa` o su identificador equivalente de Windows.
- El reloj se inyecta en autenticación, JWT, perfil y autorización fiscal, facturas, pagos, reembolsos, liquidaciones de tarjeta, reservas, caja, inventario, reportes, configuración, impresión de prueba, respaldos, auditoría y timestamps de EF Core.
- Los resolutores de AutoMapper calculan el vencimiento fiscal contra la fecha de Honduras.
- Las respuestas HTTP 4xx producidas por controladores se normalizan como `application/problem+json` con `status`, `detail`, `code`, `traceId` e `instance`; los errores de campo se conservan.
- Una sola implementación calcula y verifica la cadena SHA-256, sin depender del orden de timestamps.
- La verificación detecta hash alterado, raíz duplicada, enlace inexistente, bifurcación, ciclo o registros desconectados y devuelve un código estable.
- `GET /api/audit-logs/export-integrity` entrega un JSON ordenado con todos los campos necesarios para una verificación independiente.
- La pantalla de auditoría permite verificar y descargar el expediente; ambas acciones requieren `view_audit`.
- La cuenta de aplicación conserva únicamente `SELECT` e `INSERT` sobre `AuditLogs`; la integración comprueba que no puede actualizar ni borrar la bitácora.

### Compatibilidad corregida

La versión anterior calculaba el hash con ticks de 100 ns antes de guardar un `timestamp` PostgreSQL, que conserva microsegundos. La base sintética tiene 73 registros enlazados; 67 perdieron el último dígito temporal al persistirse. El verificador recupera de forma determinista una de las diez variantes posibles y valida los 73 sin reescribirlos. Los eventos nuevos se normalizan a microsegundos antes de calcular el hash.

## Estado de la lista P02

| Tarea | Estado verificable | Trabajo restante |
|---|---|---|
| P02-01 | Parcial | Pagos, reembolsos y liquidaciones de tarjeta ya están en servicios de aplicación; faltan emisión/notas, check-in/out, compras, inventario, cierres y respaldos. |
| P02-02 | Parcial | Los tres servicios financieros son dueños de una transacción exterior; faltan los demás casos de uso y retirar confirmaciones implícitas de repositorios. |
| P02-03 | Parcial | Reloj completado; faltan interfaces para cálculo, correlativo, impresión y respaldo, y mover su orquestación a servicios de aplicación. |
| P02-04 | Parcial | `ProblemDetails`, código y `traceId` completados; faltan paginación general, convenciones de fechas/enums y descripción OpenAPI por respuesta. |
| P02-05 | Pendiente | Generar o validar tipos TypeScript desde OpenAPI en QA. |
| P02-06 | Parcial previo | Existe matriz de permisos y prueba reflectiva; faltan motivos/aprobaciones por monto y revalidar todas las lecturas sensibles. |
| P02-07 | Parcial previo | Existen roles PostgreSQL y migración separada; la rotación de secretos se ejecutará durante instalación Windows. |
| P02-08 | Completada | Verificador, exportador, UI, compatibilidad histórica y pruebas. |
| P02-09 | Parcial | Pasan unitarias e integración PostgreSQL; falta registrar cada caso API, DAT y SEC por su identificador formal. |
| P02-10 | Pendiente | Revisión técnica por una persona distinta del implementador. |

## Entrega P02-B — movimientos financieros

Se crearon `PaymentApplicationService`, `RefundApplicationService` y `CardSettlementApplicationService`. Cada servicio valida y normaliza la solicitud, adquiere locks en orden estable, resuelve el reintento idempotente, ejecuta todos los cambios dentro de una transacción exterior y confirma únicamente después de persistir el asiento, la bitácora y el registro idempotente. Los controladores solo obtienen el actor, delegan el caso de uso y construyen la respuesta HTTP.

La prueba de arquitectura impide que estos tres controladores vuelvan a depender directamente de contabilidad, auditoría, idempotencia o reloj. La integración PostgreSQL existente recorre creación, reintento, conflicto, saldos, caja, notas de crédito, asiento y liquidación.

## Criterio para la siguiente entrega

P02-C aplicará el mismo patrón a factura y notas de crédito/débito, seguido por reservas, check-in/out y cierres de caja. También agregará inyección de fallo explícita entre persistencia, asiento, auditoría e idempotencia para demostrar rollback sin residuos.

La evidencia de QA y compatibilidad se conserva en el [expediente general](../evidencia/README.md#p02--base-técnica-primera-entrega).
