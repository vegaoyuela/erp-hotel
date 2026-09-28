# P02 — Ejecución de base técnica, contratos y seguridad

**Inicio:** 27 de septiembre de 2026

**Estado:** en ejecución; entregas P02-A a P02-E aprobadas por pruebas automáticas

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
| P02-01 | Completada | Finanzas, factura/notas, reservas, check-in/out, folios, caja, compras, inventario y respaldo delegan sus mutaciones a servicios. |
| P02-02 | Parcial | Los flujos fiscales, operativos, compra e inventario tienen transacción exterior y pruebas de rollback; faltan fallos P02-B y retirar 56 confirmaciones implícitas en 11 archivos de repositorio. |
| P02-03 | Parcial | Reloj y ejecución de respaldo tienen contrato inyectable; faltan interfaces para cálculo, correlativo e impresión. |
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

## Entrega P02-C — emisión y ajustes fiscales

`InvoiceApplicationService` concentra la actualización de borradores, emisión de factura, liquidación completa de folio y creación de notas de crédito y débito. La transacción exterior incluye correlativo, documento, pago y caja cuando corresponden, estado del folio/reserva/habitación, asiento, auditoría e idempotencia. `InvoicesController` conserva únicamente consultas, identidad del actor y traducción HTTP.

Los tres flujos fiscales incorporan puntos de fallo de prueba después de persistencia, contabilidad, auditoría e idempotencia. La integración provoca un fallo después del asiento en factura, nota de crédito y nota de débito, y comprueba que PostgreSQL revierte documento, asiento, cuentas creadas, bitácora, idempotencia, estado de la factura original y avance del correlativo. Los flujos normales, reintentos y conflictos siguen cubiertos por la misma suite.

## Entrega P02-D — operación hotelera y caja

`ReservationApplicationService`, `FolioApplicationService` y `CashRegisterApplicationService` concentran creación, actualización, confirmación y cancelación de reservas, check-in/out, cargos de folio y creación/apertura/cierre de caja. Los controladores mantienen las consultas y la traducción HTTP. Locks, control de versión, superposición de fechas, estado físico de habitación, cálculo de estancia, idempotencia y auditoría permanecen dentro de la transacción exterior de cada caso.

La integración conserva las pruebas de concurrencia para reserva, check-in y caja. Además inyecta fallos después de persistir check-in, cargo de folio, apertura y cierre de caja, y confirma que no sobreviven folios, cargos, movimientos, cambios de habitación/reserva, auditorías ni registros idempotentes.

## Entrega P02-E — compras, inventario y respaldo

`PurchaseInvoiceApplicationService` une factura de compra, asiento, auditoría y estado dentro de una transacción exterior. Valida líneas, separa ISV 15 % e ISV 18 %, rechaza duplicados del mismo proveedor y prueba que un fallo posterior al asiento no conserva compra, contabilidad, cuentas ni auditoría.

`InventoryApplicationService` concentra catálogos, productos y movimientos. Cada movimiento toma un lock transaccional por producto antes de leer existencias. La integración provoca un fallo posterior a la persistencia y dos salidas simultáneas: el fallo revierte producto, movimiento y auditoría; de dos salidas que excederían el saldo combinado, una finaliza y la otra recibe conflicto.

`IDatabaseBackupService` e `IPostgresDumpExecutor` separan la orquestación y el proceso externo. El ejecutor usa `ArgumentList`, conserva la contraseña fuera de los argumentos, registra el intento antes de ejecutar, elimina archivos parciales y devuelve un respaldo cuyo tamaño y SHA-256 se verifican contra una descarga real.

## Criterio para la siguiente entrega

P02-F retirará las confirmaciones implícitas de los repositorios que todavía las conservan y añadirá pruebas de rollback a pagos, reembolsos y liquidaciones. Después continuará con contratos de cálculo, correlativo e impresión, OpenAPI y tipos frontend.

La evidencia de QA y compatibilidad se conserva en el [expediente general](../evidencia/README.md#p02--base-técnica-primera-entrega).
