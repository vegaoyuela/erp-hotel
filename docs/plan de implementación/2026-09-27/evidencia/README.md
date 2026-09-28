# Evidencia P00 — línea base reproducible

**Ejecución:** 27 de septiembre de 2026
**Rama de trabajo:** `codex/implementacion-final-2026-09-27`
**Referencia anterior a la implementación:** etiqueta `implementation-baseline-2026-09-27`, commit `3403dad5f5c9f6d3ba70b48e3b59b9c7f940c5f0`

## Resultado

La parte ejecutable de P00 quedó aprobada sobre información sintética de QA:

- el respaldo PostgreSQL 16 se restauró en una base aislada y coincidieron los conteos de sus 42 tablas y 345 filas;
- el manifiesto registra hashes SHA-256 de aplicación, pruebas, frontend, migraciones, despliegue, scripts y documentación;
- no había adjuntos operativos en las rutas inventariadas y no se copió PII a esta evidencia;
- se retiraron del control de versiones 261 artefactos generados u obsoletos: `bin`, `obj`, logs, SQLite, bundles, caché del editor y el segundo lockfile;
- Docker Compose rechaza el arranque sin los secretos requeridos y acepta una configuración con valores efímeros de validación;
- la compilación terminó con cero advertencias y cero errores; pasaron 76 pruebas unitarias y 4 pruebas de integración PostgreSQL;
- `npm audit` informó cero vulnerabilidades, y el lint y la compilación de producción del frontend fueron aprobados.

La primera ejecución limpia descubrió que las pruebas dependían de la existencia física de `wwwroot`. Se corrigió agregando un objetivo de MSBuild que crea el directorio antes de compilar; los bundles permanecen ignorados y se regeneran. El log inicial fallido se conserva junto al log exitoso para demostrar la corrección.

## Archivos de evidencia

| Archivo | Contenido |
|---|---|
| [p00-respaldo-restauracion-sintetica.json](p00-respaldo-restauracion-sintetica.json) | Origen sintético, hash del dump, versión de PostgreSQL y comparación exacta de tablas. |
| [p00-inventario.json](p00-inventario.json) | Inventario de código, configuración, semillas, adjuntos y limpieza, sin valores secretos. |
| [p00-manifiesto.json](p00-manifiesto.json) | Hashes y agregados SHA-256 de los componentes disponibles. |
| [p00-qa-resumen.json](p00-qa-resumen.json) | Resultado estructurado de backend, pruebas, frontend y validación Compose. |
| [p00-qa.log](p00-qa.log) | Salida completa de la ejecución aprobada de `scripts/qa.sh`. |
| [p00-qa-initial-failure.log](p00-qa-initial-failure.log) | Fallo reproducible detectado antes de agregar el marcador de `wwwroot`. |

El dump se conserva en `data/backups/p00-2026-09-27/`, ruta excluida de Git. Su integridad puede comprobarse con el SHA-256 registrado en la evidencia JSON.

## Pendiente externo para G0

La base, los adjuntos y los documentos oficiales del hotel no estaban disponibles durante esta ejecución. G0 seguirá abierta hasta que el propietario entregue esas fuentes y se completen P00-02B, P00-03B y P00-04B. La ausencia de esos datos no invalida la QA técnica realizada, pero impide afirmar que la línea base oficial es recuperable.

## P01 — perfil fiscal y expediente

La parte técnica independiente de P01 agrega:

- plantilla externa para DEC-01–DEC-15 y controles N01–N06;
- validador Node.js que produce únicamente hash, estados y errores por ruta, sin copiar valores del expediente;
- enlace obligatorio entre una aprobación fiscal y el SHA-256/referencia del expediente validado;
- tabla `FiscalProfileRevisions` con versión, vigencia, actor, nota, estado, snapshot y hash;
- rechazo de una segunda aprobación sobre la misma versión;
- retiro enlazado a la revisión aprobada sin alterar su snapshot;
- interfaz para ingresar la referencia y el hash antes de habilitar la aprobación.

Al actualizar desde el esquema anterior, cualquier perfil marcado como aprobado o retirado pasa a una nueva versión en borrador. Esto es intencional: una aprobación antigua no contiene el manifiesto ni el snapshot exigidos por P01 y debe revalidarse antes de volver a emitir. La prueba de actualización movió el perfil sintético P00 de aprobado v2 a borrador v3, sin alterar filas de operación.

| Archivo | Resultado |
|---|---|
| [p01-validacion-sintetica.json](p01-validacion-sintetica.json) | N01–N06 aprobadas con un expediente exclusivamente sintético. |
| [p01-validacion-oficial-pendiente.json](p01-validacion-oficial-pendiente.json) | El modo oficial rechaza correctamente el expediente sintético. |
| [p01-migracion-actualizacion.json](p01-migracion-actualizacion.json) | Migración aplicada sobre una restauración P00 sin alterar conteos de las tablas de datos existentes. |
| [p01-migracion-actualizacion.log](p01-migracion-actualizacion.log) | Salida de EF Core para la actualización controlada. |
| [p01-qa-resumen.json](p01-qa-resumen.json) | Resultado consolidado, hashes y estado del bloqueo oficial. |
| [p01-qa.log](p01-qa.log) | Regresión completa con flujo PostgreSQL de aprobación, historial, inmutabilidad y retiro. |
| [p01-modelo-ef.log](p01-modelo-ef.log) | Confirmación de que el modelo no tiene cambios pendientes de migración. |

La base sintética se restauró desde el dump P00 después de que una invocación de mantenimiento de EF apuntara a su última migración aplicada; la restauración volvió a coincidir exactamente en 42 tablas y 345 filas antes de continuar. No existió intervención sobre datos oficiales.

P01 no está aprobada para producción. Faltan el expediente oficial externo, DEC-01–DEC-15, las firmas de gerente y contador y la ejecución de N01–N06 con `--require-official`.

## P02 — base técnica, primera entrega

La primera entrega de P02 reemplaza la fecha global estática por un reloj inyectable en los flujos operativos, normaliza los errores 4xx de controladores y consolida la generación, verificación y exportación de la cadena de auditoría. El frontend incorpora la descarga del expediente de integridad.

La comprobación de compatibilidad encontró que el formato histórico calculaba el hash antes de que PostgreSQL redujera el timestamp de ticks de 100 ns a microsegundos. La cadena sintética conserva una raíz y 73 enlaces válidos. Seis hashes coinciden directamente y 67 se validan recuperando una de las diez variantes posibles del tick perdido; ningún registro fue reescrito. Los eventos nuevos se firman con precisión normalizada a microsegundos.

| Archivo | Resultado |
|---|---|
| [p02-auditoria-compatibilidad.json](p02-auditoria-compatibilidad.json) | Los 73 registros sintéticos existentes validan; una raíz, sin bifurcaciones ni enlaces ausentes. |
| [p02-modelo-ef.log](p02-modelo-ef.log) | EF Core confirma que el modelo no requiere una migración adicional. |
| [p02-qa-resumen.json](p02-qa-resumen.json) | Resumen estructurado, estado de G2 y hashes SHA-256 de esta entrega. |
| [p02-qa.log](p02-qa.log) | Build sin advertencias, 83 unitarias, 4 integraciones PostgreSQL, 5 pruebas N01–N06, auditoría npm, lint y frontend aprobados. |

P02 continúa abierta. Faltan la extracción de casos de uso, la unidad de trabajo, las interfaces restantes, la descripción OpenAPI completa, la validación automática de tipos TypeScript, la matriz formal API/DAT/SEC y la revisión técnica independiente. El estado preciso se mantiene en [P02 — ejecución de base técnica](../p02-base-tecnica/README.md).

### P02-B — servicios de movimientos financieros

La segunda entrega mueve la creación de pagos, reembolsos y liquidaciones de tarjeta desde los controladores hacia servicios de aplicación. Cada servicio controla una transacción PostgreSQL exterior que incluye locks, idempotencia, saldos, caja cuando corresponde, estado de facturas o pagos, asiento contable y bitácora. Una prueba de arquitectura protege esta separación.

| Archivo | Resultado |
|---|---|
| [p02b-modelo-ef.log](p02b-modelo-ef.log) | EF Core confirma que la extracción no requiere migración. |
| [p02b-qa-resumen.json](p02b-qa-resumen.json) | Resumen estructurado y hashes SHA-256 de código y evidencia. |
| [p02b-qa.log](p02b-qa.log) | Build sin advertencias, 86 unitarias, 4 integraciones PostgreSQL, 5 pruebas N01–N06, auditoría npm, lint y frontend aprobados. |

P02-01 y P02-02 continúan parciales: aún deben extraerse reservas, folios, caja, compras, inventario y respaldo; también faltan pruebas explícitas de rollback para los movimientos financieros de P02-B.

### P02-C — emisión y ajustes fiscales

La tercera entrega traslada actualización de borradores, emisión, liquidación de folio y notas de crédito/débito a `InvoiceApplicationService`. Cada flujo fiscal conserva una transacción PostgreSQL exterior. La integración inyecta un fallo posterior al asiento de factura, nota de crédito y nota de débito, y confirma que no persisten documentos, asientos, cuentas, auditorías, idempotencia, cambios de estado ni correlativos consumidos.

| Archivo | Resultado |
|---|---|
| [p02c-modelo-ef.log](p02c-modelo-ef.log) | EF Core confirma que la extracción no requiere migración. |
| [p02c-qa-resumen.json](p02c-qa-resumen.json) | Resumen estructurado y hashes SHA-256 de código, pruebas y evidencia. |
| [p02c-qa.log](p02c-qa.log) | Build sin advertencias, 87 unitarias, 4 integraciones PostgreSQL con tres escenarios de rollback fiscal, 5 pruebas N01–N06, auditoría npm, lint y frontend aprobados. |

P02 continúa abierta. El siguiente bloque cubre compras, inventario, respaldos, persistencia implícita de repositorios y fallos inyectados de P02-B.

### P02-D — operación hotelera y caja

La cuarta entrega traslada reservas, check-in/out, cargos de folio y ciclo de caja a servicios de aplicación. Las pruebas conservan los escenarios de concurrencia e idempotencia existentes e incorporan fallos intencionales después de persistir check-in, cargo de folio, apertura y cierre de caja. PostgreSQL revierte los estados, folios, cargos, movimientos, auditorías e idempotencia de cada intento fallido.

| Archivo | Resultado |
|---|---|
| [p02d-modelo-ef.log](p02d-modelo-ef.log) | EF Core confirma que la extracción no requiere migración. |
| [p02d-qa-resumen.json](p02d-qa-resumen.json) | Resumen estructurado y hashes SHA-256 de código, pruebas y evidencia. |
| [p02d-qa.log](p02d-qa.log) | Build sin advertencias, 90 unitarias, 4 integraciones PostgreSQL con cuatro rollbacks operativos nuevos, 5 pruebas N01–N06, auditoría npm, lint y frontend aprobados. |

Al cierre de P02-D, P02-01 y P02-02 seguían parciales por compras, inventario, respaldos, repositorios con `SaveChanges` propio y fallos pendientes de P02-B.

### P02-E — compras, inventario y respaldo

La quinta entrega cierra P02-01. La compra y su asiento contable comparten transacción; los movimientos de inventario bloquean por producto; el respaldo manual se ejecuta mediante un contrato y un proceso `pg_dump` con argumentos separados. La prueba de integración valida rollback de compra e inventario, concurrencia de existencias y un archivo SQL real mediante tamaño y SHA-256.

| Archivo | Resultado |
|---|---|
| [p02e-modelo-ef.log](p02e-modelo-ef.log) | EF Core confirma que la extracción no requiere migración. |
| [p02e-qa-resumen.json](p02e-qa-resumen.json) | Resumen estructurado y hashes SHA-256 de código, pruebas y evidencia. |
| [p02e-qa.log](p02e-qa.log) | Build sin advertencias, 93 unitarias, 4 integraciones PostgreSQL, 5 pruebas N01–N06, auditoría npm, lint y frontend aprobados. |

P02 continúa abierta. P02-F cubrirá 56 guardados implícitos en 11 archivos de repositorio y los fallos inyectados pendientes de pagos, reembolsos y liquidaciones.
