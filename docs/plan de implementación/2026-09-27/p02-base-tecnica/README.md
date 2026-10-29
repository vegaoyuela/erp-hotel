# P02 — Ejecución de base técnica, contratos y seguridad

**Inicio:** 27 de septiembre de 2026

**Estado:** en ejecución; entregas P02-A a P02-H2C aprobadas por pruebas automáticas. Última ejecución: 29 de octubre de 2026.

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
| P02-02 | Completada | Persistencia explícita con `IUnitOfWork`, cero guardados/transacciones en repositorios y pruebas de aislamiento, rollback y regresión HTTP. |
| P02-03 | Completada en código | Contratos inyectables de reloj, cálculo, correlativo, auditoría, impresión y respaldo; sustitución/rollback y transporte simulado probados. Prueba física pendiente en Windows. |
| P02-04 | Parcial (P02-H2C) | Selectores paginados y cero éxitos 200/201 sin esquema; quedan usuarios/roles y validar límites de catálogos/reportes del inventario. |
| P02-05 | Completada en código y QA | Tipos compartidos derivados de OpenAPI; comparación automática contra API real, snapshot y tipos generados. Revisión independiente en P02-10. |
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

## Entrega P02-F1 — rollback financiero

Pagos, reembolsos y liquidaciones incorporan un punto de fallo inmediatamente después de generar el asiento. La integración ejecuta cada operación válida contra PostgreSQL, provoca la excepción y comprueba que la transacción no conserva pagos, aplicaciones, reembolsos, liquidaciones, movimientos de caja, cambios de estado, asientos, auditorías ni claves idempotentes. Los mismos flujos normales y concurrentes se ejecutan después para demostrar que la instrumentación no alteró el comportamiento.

## Entrega P02-F2 — unidad de trabajo explícita

El 28 de octubre se retiraron los 56 guardados internos de los repositorios. `IUnitOfWork` permite persistir los cambios preparados desde el consumidor, sin confirmar una transacción exterior. `CaiCorrelativeService` conserva la asignación del correlativo dentro del lock y `AuditService` guarda la bitácora dentro del lock de cadena.

La nueva integración prueba visibilidad antes/después del guardado, rollback exterior y reversión del lote ante fallo de unicidad. La matriz HTTP vuelve a probar altas, ediciones y bajas de catálogos, roles/permisos y usuarios; verifica que los intentos de acceso fallidos se guardan. El comprobador de límites de repositorio queda incorporado a QA. La auditoría npm de esta fecha detectó cinco alertas altas; se actualizaron dependencias dentro de los rangos existentes, incluidas Vite y Zod. El [detalle de la entrega](p02-f2-unidad-de-trabajo.md) contiene consumidores, pruebas y evidencia.

## Entrega P02-G — contratos de cálculo, auditoría e impresión

`IInvoiceCalculator`, `ITaxCalculator`, `IAuditService`, `IInvoicePrintRenderer` e `IPrinterTransport` completan las dependencias pendientes. Los consumidores reciben interfaces; `WindowsPrinterTransport` concentra el acceso al componente nativo. Se retiró la consulta síncrona al construir el calculador de impuestos y la muestra de impresión usa el cálculo compartido para conservar líneas y totales congruentes.

La integración prueba que sustituir el calculador o la auditoría por una implementación que falla revierte la factura y el correlativo. La impresión captura los bytes del generador real con un transporte simulado y recorre vista previa, original/copia, regla, fallos y plataforma no disponible. El [detalle de P02-G](p02-g-contratos.md) conserva el alcance y la prueba física pendiente de Windows.

## Entrega P02-H1 — contrato API y tres listados paginados

El contrato OpenAPI queda versionado y genera tipos TypeScript. QA compara el documento obtenido de una API real en una base sintética contra el snapshot y los tipos del repositorio. Los listados de huéspedes, facturas y auditoría filtran y paginan en PostgreSQL; las facturas combinan los filtros y contemplan el día final completo. La bitácora permite consultar más de 200 eventos.

El [detalle de P02-H1](p02-h1-contrato-paginacion.md) contiene rutas, convenciones, comandos, capturas, límites y próximos pasos. Se aprobaron 109 unitarias, 5 integraciones PostgreSQL, 5 validaciones del expediente, compilación, lint y comparación del contrato; además se comprobó el rechazo de una divergencia deliberada y seis capturas en navegador.

## Entrega P02-H2A — operación e inventario

El 29 de octubre se incorporaron páginas SQL para reservas, folios, clientes, productos y movimientos de inventario. Los indicadores usan agregados globales; el selector del Kardex busca y pagina sin cargar todos los productos. El listado de folios usa un DTO compacto y conserva referencias históricas. Los resultados de reservas y cargos de folio tienen esquemas explícitos y nueve tipos frontend adicionales derivan del contrato. El [detalle de H2A](p02-h2a-operacion-inventario.md) conserva QA, diez capturas y pendientes.

## Entrega P02-H2B — finanzas y respaldo

Se paginaron cajas, movimientos de caja, liquidaciones, cobros elegibles, asientos y logs de respaldo. Los saldos y agregados globales no dependen de la página; el lote conserva cobros elegidos en páginas distintas. Doce tipos frontend derivan de OpenAPI. El navegador registró una liquidación sintética de L 170 con dos aplicaciones y depósito L 163. El [detalle H2B](p02-h2b-finanzas-respaldos.md) contiene QA, ocho capturas y pendientes.

## Criterio para la siguiente entrega

P02-H2C queda documentada en la entrega siguiente. El siguiente bloque cubre permisos/motivos/aprobaciones (P02-06), paginación administrativa y límites restantes (P02-04), y matriz formal API/DAT/SEC (P02-09). G2 sigue abierta hasta cubrir los pendientes de la tabla y obtener la revisión independiente.

La evidencia de QA y compatibilidad se conserva en el [expediente general](../evidencia/README.md#p02--base-técnica-primera-entrega).

## Entrega P02-H2C — selectores y contratos (29 de octubre de 2026)

Se paginaron los selectores de check-in/out, autorizaciones y cajas auxiliares. La selección persiste entre búsquedas/páginas y navegar no envía formularios. OpenAPI tiene cero respuestas 200/201 sin esquema y ocho descargas binarias. Las 28 interfaces compartidas utilizadas que quedaban pasan a tipos derivados; se retira una sin consumidores. Pasan 109 unitarias, 5 integraciones, 5 validaciones del expediente y ocho capturas del navegador. El [detalle H2C](p02-h2c-selectores-contratos.md) contiene el anexo de uso, los 19 arrays restantes clasificados, QA y pendientes. P02-05 queda completada en código/QA; P02-04 sigue parcial.
