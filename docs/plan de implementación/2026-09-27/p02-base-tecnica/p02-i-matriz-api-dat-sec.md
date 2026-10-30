# P02-I — Matriz formal API, DAT y SEC

**Fecha:** 29 de octubre de 2026. **Actualizada con P02-L el 30 de octubre de 2026.** **Catálogo de origen:** [pruebas y aprobación](../../../plan-sar-2026-09-07/03-pruebas-y-aprobacion.md). **Entorno:** Linux, PostgreSQL efímero y Chromium con datos sintéticos.

Esta matriz vincula los identificadores del plan con pruebas existentes y nuevas. «Ejecutado» significa que la comprobación descrita está incluida en la QA final; no sustituye la aceptación del caso completo ni la revisión independiente. P02-09 permanece parcial cuando el catálogo exige escenarios adicionales o el entorno Windows.

## Seguridad

| Caso | Cobertura ejecutada y fuente | Falta para la aceptación completa |
|---|---|---|
| SEC01 — roles y elevación | `SystemRoleMatrix_EnforcesSensitiveEndpointPolicies`, `AssertReadAccessMatrixAsync` y `EndpointAuthorizationTests`: permisos de administración, alias de administrador sin acceso implícito, intento de modificar rol superior y auditoría de rechazos. | P02-J comprueba renombrado/eliminación protegidos, permisos de Admin, actor en espera y último administrador. Resta aceptación independiente y matriz completa de variantes del catálogo. |
| SEC02 — anónimos | `AssertAnonymousControllerMatrixAsync` obtiene los endpoints reales, prueba cada verbo y exige 401 en controladores privados. Solo login y refresh de controladores permiten anónimo; salud se comprueba aparte. | Repetir la matriz sobre la distribución final, incluido gateway/adjuntos/servicios Windows y cualquier endpoint nuevo. |
| SEC03 — cuatro roles | Matriz anterior más 64 lecturas y ocho intentos de notas en `ReadAccess`; siete recorridos UI. Rutas prohibidas devuelven 403 aunque se invoquen directamente. | Ampliar cuerpos válidos y datos presentes para cada combinación de mutación/módulo y completar todos los E2E por rol. |
| SEC04 — cambios de privilegios | Seis intentos rechazados: restablecer contraseña, editar, desactivar y asignar rol a usuario superior, cambiar rol superior y crear Recepción por registro heredado. Se comprueban cuenta/contraseña/permisos preservados y seis eventos `SecurityChangeDenied`. | P02-J añade ocho carreras, seis salvaguardas, nueve clases de mutación auditadas y 39 rollbacks. Restan reaperturas y aprobaciones sensibles de operación/finanzas, más revisión independiente. |
| SEC05 — archivos privados | Matriz de permiso sobre respaldo y PDF; `AssertAuthorizationAttachmentSecurityAsync` carga y descarga PDF por ID y verifica bytes, permiso y ruta privada. `AssertManualBackupAsync` descarga un respaldo real y comprueba tamaño/hash. | Repetir con archivos y roles del despliegue; comprobar directorios/volúmenes del equipo Windows. |
| SEC06 — revocación | `BootstrapPasswordChangeAndLogout_InvalidatePreviousTokens`, regresión de persistencia de usuarios y `Program.OnTokenValidated`: cambio obligatorio, versión de seguridad y revocación. | P02-J registra baja/cambio de usuario, rol/permiso, contraseña y logout, revocación y carreras frente a login/refresh; verifica rollback conjunto. P02-K cubre 27 métodos hoteleros/financieros/inventario: 54 denegaciones directas, tres HTTP en espera y permiso vigente retirado. P02-L cubre esas 34 mutaciones con 68 rechazos directos y 68 HTTP. Restan los efectos externos enumerados, el tratamiento de lecturas, variantes adicionales/E2E y aceptación independiente en distribución final. |
| SEC07 — logout | Regresión principal comprueba rechazo de access/refresh anteriores al logout. UI alterna sesiones y reconstruye caché por identidad/permisos. | P02-K comprueba que logout espera al movimiento ya admitido y que el token anterior se rechaza. P02-L añade un PDF admitido antes de logout y cancelación con limpieza de archivo. Restan ensayo explícito de dos pestañas simultáneas y revisión de cabeceras/caché en distribución final. |
| SEC08 — refresh/CSRF/CORS | `AuthenticationAbuse_IsRateLimited_AndInternalErrorsAreSafe`, rotación de refresh en regresión principal y `AssertCorsSessionPolicyAsync`: cookies, CSRF ausente, rotación y orígenes permitidos/rechazados. | P02-J prueba dos refresh simultáneos por HTTP, replay y revocación de la sesión recién emitida. Restan ensayo explícito entre pestañas del navegador y origen HTTPS final del hotel. |
| SEC09 — adjuntos | `AssertAuthorizationAttachmentSecurityAsync`: falso PDF, más de 10 MiB, nombre con traversal, descarga por ID y archivo externo al almacén. | Revisión de permisos de disco/volumen y comportamiento en Windows. |
| SEC10 — abuso y errores | `AuthenticationAbuse_IsRateLimited_AndInternalErrorsAreSafe`: 429/Retry-After, recuperación y excepción sintética sin detalles sensibles. `AssertSafeProblemDetailsAsync` valida errores seguros. | Caída real/reinicio de PostgreSQL bajo carga, límites acordados para LAN y revisión de logs del despliegue final. |
| SEC11 — mínimo privilegio DB | `ApplicationRole_RunsOrdinaryFlow_ButCannotMigrateOrRewriteAudit`: flujo ordinario con rol restringido; rechaza DDL, escritura de historial de migraciones, UPDATE/DELETE de bitácora y lectura de `pg_authid`. | Ejecutar roles definitivos de migración/aplicación/respaldo/bootstrap y rotación de secretos en Windows (P02-07). |

## Contrato API

| Caso | Cobertura ejecutada y fuente | Falta para la aceptación completa |
|---|---|---|
| API01 — listas/detalles | Helpers de persistencia, operación, compras y paginación verifican detalle y proyección; listas de folios/diario omiten hijos y conservan conteos. | Caso dedicado con exactamente dos líneas y comparación exhaustiva de factura, folio y compra conforme al catálogo. |
| API02 — fechas de autorización | Altas/lecturas de autorización en regresión fiscal y de adjuntos; serialización DateOnly y selector por fecha de Honduras. | Consolidar aserción formal de igualdad civil en cada vía de alta, incluidos multipart y despliegue Windows. |
| API03 — enums y formularios | Validación de filtros/tipos inválidos en paginación operativa, financiera y selectores; validaciones de emisión/compra/nota. | Matriz exhaustiva de valores fuera de catálogo y comprador empresarial válido por cada formulario. |
| API04 — PDF autenticado | Helper de adjuntos y políticas: bytes correctos, 401/403 y ID inexistente. Frontend descarga por Axios autenticado y revoca URL temporal. | E2E explícito de descarga PDF y liberación de URL temporal en navegador. |
| API05 — cliente y OpenAPI | `AssertApiContractAndPaginationAsync`, `AssertSelectorContractsAsync`, comparación de snapshot real y tipos en `qa.sh`; cero éxitos 200/201 sin esquema y ocho binarios. Se conserva la prueba negativa H1. | Revisión de adaptadores y E2E del conjunto completo de botones, cuerpos y métodos, más revisión independiente. |

## Fechas

| Caso | Cobertura ejecutada y fuente | Falta para la aceptación completa |
|---|---|---|
| DAT01 — UTC/Honduras | `HotelClockTests.Clock_ConvertsUtcToHondurasAndExposesLocalDate` usa instante fijo; integración guarda fechas civiles y timestamps PostgreSQL. | Repetición del mismo caso en Windows y verificación contra instalación final. |
| DAT02 — límites de período | `AssertApiContractAndPaginationAsync` comprueba filtros de facturas incluyendo el día final completo; reloj compartido en operación/reportes. | Fixture dedicado a medianoche y cambio de mes para todos los movimientos/reportes aplicables. |
| DAT03 — fechas civiles de estadía | Regresión de reservas, check-in y paginación conserva DateOnly; UI del bloque se ejecutó con zona `America/Tegucigalpa`. | Navegadores con zonas diferentes y comparación de entrada/salida sin cambio de día. |

## Ampliación P02-J

Tres integraciones nuevas cubren administración, rollback y concurrencia. La suite suma ocho integraciones, sin omisiones. Véase [detalle P02-J](p02-j-administracion-atomica.md), [operaciones](../evidencia/p02j-operaciones.json), [ocho carreras](../evidencia/p02j-concurrencia.json) y [39 rollbacks](../evidencia/p02j-rollback.json). Los límites de cada fila permanecen explícitos; P02-09 continúa parcial.

## Archivos de evidencia

- [QA de P02-I](../evidencia/p02i-qa.log): ejecución de aquella entrega, con cinco integraciones.
- [QA de P02-J](../evidencia/p02j-qa.log): ejecución final con 138 unitarias y ocho integraciones, incluidas tres pruebas nuevas de administración/rollback/concurrencia.
- [Matriz HTTP adicional](../evidencia/p02i-accesos.json): 72 comprobaciones y seis rechazos administrativos, páginas de 31 usuarios/roles y minimización del dashboard.
- [UI](../evidencia/ui-p02i/resultado.json): siete recorridos, doce capturas, errores y desbordamientos.
- [Resumen](../evidencia/p02i-qa-resumen.json): hashes y estado de P02.

Los nombres de helpers se encuentran en `backend/tests/hotel-erp.IntegrationTests/AuthenticationFlowTests*.cs`; políticas y pruebas reflectivas están en `backend/tests/hotel-erp.UnitTests`. Ninguna fila pendiente se cierra por usar datos sintéticos o por tener una prueba similar.

## Ampliación P02-K

Dos integraciones nuevas elevan la suite a diez integraciones sin omisiones. Véase [detalle](p02-k-revocacion-operativa.md), [revocación](../evidencia/p02k-revocacion.json), [orden/cancelación](../evidencia/p02k-orden.json), [alcance residual](../evidencia/p02k-alcance.json) y [QA final](../evidencia/p02k-qa.log). Las 54 llamadas directas comprueban admisión previa al acceso a entradas mínimas; no representan 54 flujos HTTP válidos. La prueba de 30 esperas es funcional, no una medición de capacidad. P02-09 permanece parcial.

## Ampliación P02-L

Tres integraciones nuevas elevan la suite a trece integraciones sin omisiones. La matriz de catálogos contiene las 34 acciones HTTP y una rama adicional de cuenta con historial: 35 éxitos con auditoría y 71 rollbacks de base/archivos. Revocación: 34 servicios y 34 rutas para 401, repetidos para 403 con permisos retirados sin cambiar la versión del fixture. También se comprueban el orden de revocación/logout, cancelación y 200/409 en doble aprobación fiscal. No se sustituyen los casos de detalle fiscal/folio/compra ni las combinaciones por rol/entorno que faltan en las filas anteriores.

Véanse [detalle](p02-l-catalogos-configuracion.md), [QA final](../evidencia/p02l-qa.log), [revocación](../evidencia/p02l-revocacion.json), [atomicidad](../evidencia/p02l-atomicidad.json), [orden](../evidencia/p02l-orden.json) e [inventario restante](../evidencia/p02l-alcance.json). P02-09 sigue parcial; las lecturas, efectos externos y aceptación independiente no quedan cerrados.
