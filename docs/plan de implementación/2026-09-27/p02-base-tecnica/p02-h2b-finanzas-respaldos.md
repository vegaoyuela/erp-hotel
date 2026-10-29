# P02-H2B — Consultas financieras y respaldos

**Fecha:** 29 de octubre de 2026. **Estado:** alcance H2B implementado y verificado. P02-04/P02-05 y G2 permanecen abiertos.

## Cambios implementados

- **Caja:** selector de cajas con búsqueda y paginación, detalle independiente y movimientos paginados. La selección inicial se fija una vez y se conserva al cambiar de página del selector. La consulta de detalle conserva el saldo/estado del último movimiento según `CreatedAt` e ID, aunque se consulte una página histórica. Cambiar de caja reinicia la página de movimientos y cierra el formulario anterior. Apertura/cierre mantienen idempotencia, validaciones y transacciones; sus botones se bloquean mientras se actualiza el saldo o existe un error del detalle.
- **Liquidaciones:** historial y cobros elegibles paginados por separado, búsqueda por número/referencia y total pendiente calculado en SQL sobre todos los cobros elegibles. El saldo disponible descuenta exclusivamente reembolsos y liquidaciones confirmados; registros anulados no reducen el saldo. Los importes persistidos tienen precisión 18,2, por lo que la resta decimal conserva los centavos sin convertirlos a flotantes.
- **Selección de cobros:** el lote conserva objetos seleccionados entre páginas y búsquedas. **Seleccionar esta página** afecta solo la página visible; **Limpiar selección** retira todo el lote. **Actualizar** limpia la selección y vuelve a consultar. El backend vuelve a validar importes dentro de sus locks/transacción; las cantidades conservadas en el navegador no sustituyen esa validación.
- **Diario:** búsqueda, páginas y proyección compacta `AccountingEntryListDto`, sin enviar todas las líneas de todos los asientos. **Ver** obtiene el detalle completo. Los totales de débito/crédito son agregados globales independientes del filtro y la página; el catálogo de cuentas se carga al abrir el formulario manual.
- **Respaldos:** historial paginado y filtros de archivo/estado, sin quedarse en los veinte eventos del endpoint anterior. `BackupLogDto` excluye ruta local e identificador de Drive. El endpoint antiguo conserva su valor predeterminado de veinte y ahora valida `limit` entre 1 y 100. La generación/descarga manual conserva el flujo existente.
- **Contrato:** resultados explícitos de apertura/cierre, creación de caja/liquidación/asiento y detalle. El historial de respaldo deja de depender de una respuesta anónima. Se migraron doce tipos frontend a los esquemas generados sin añadir dependencias.

## Rutas

| Ruta | Filtros / finalidad | Orden estable |
|---|---|---|
| `GET /api/cash-registers/paged` | `search`: nombre/ubicación | Nombre, ID |
| `GET /api/cash-registers/{id}` | Detalle y saldo de la caja seleccionada | Último movimiento por creación e ID |
| `GET /api/cash-registers/{id}/movements/paged` | `search`, `movementType` | Creación e ID descendentes |
| `GET /api/card-settlements/paged` | `search`: número/referencia | Fecha e ID descendentes |
| `GET /api/card-settlements/eligible-payments/paged` | `search`: pago/referencia POS | Fecha e ID ascendentes |
| `GET /api/card-settlements/eligible-payments/summary` | Conteo y monto global pendiente | Agregado SQL |
| `GET /api/Accounts/journal-entries/paged` | `search`: concepto, tipo, código/nombre de cuenta | Fecha e ID descendentes |
| `GET /api/Accounts/journal-entries/summary` | Débito y crédito globales | Agregado SQL |
| `GET /api/Backup/logs/paged` | `search`: archivo; `status` | Inicio e ID descendentes |

Las seis colecciones nuevas usan las convenciones compartidas: página desde 1, tamaño 1–100, valor predeterminado 25, ajuste a la última página y respuesta `items/page/pageSize/totalItems/totalPages`. Las consultas usan `AsNoTracking`, cancelación y orden con ID para resolver empates. Las búsquedas tienen máximo cien caracteres. Tipos/estados inválidos y límites fuera de rango devuelven 400 normalizado; una caja inexistente devuelve 404.

El historial de liquidaciones pagina las identidades primero y carga aplicaciones y referencias solamente para esa página. La consulta de cajas carga un solo movimiento reciente por caja de la página. Los endpoints antiguos permanecen disponibles para selectores auxiliares pendientes de H2C.

## Permisos verificados

| Consulta | Recepción | Caja | Contabilidad | Administración |
|---|---|---|---|---|
| Cajas, detalle y movimientos | 200 | 200 | 403 | 200 |
| Liquidaciones, elegibles, resumen y diario | 403 | 403 | 200 | 200 |
| Logs paginados de respaldo | 403 | 403 | 403 | 200 |

Las seis colecciones rechazan accesos anónimos con 401. Esto conserva las políticas existentes; no cierra la revisión general de mínimo privilegio de P02-06.

## QA y pruebas de regresión

- Build .NET sin advertencias/errores; 109 unitarias, 5 integraciones PostgreSQL y 5 pruebas del expediente fiscal aprobadas.
- Seis colecciones con 31 coincidencias sintéticas cada una: conteo, primera/segunda/última página, ajuste de página excesiva, repetición estable y ausencia de IDs repetidos entre páginas.
- Caja con 31 movimientos: consultar la página histórica no cambia el saldo actual de L 130; los timestamps de creación y fecha del movimiento se ordenan deliberadamente de forma diferente para probar la regla del saldo.
- Cobro sintético de L 100, reembolso confirmado L 10 y liquidación confirmada L 20: disponible L 70. Reembolso y liquidación anulados se ignoran; cobros anulados, transferencias y cobros totalmente reembolsados/liquidados no aparecen como elegibles. La colección nueva y el resumen se contrastan con el cálculo anterior y sus importes por ID.
- Totales globales del diario contrastados con consultas independientes; lista sin `items`, conteo de dos líneas y detalle completo con dos líneas.
- Estado/archivo de respaldo combinados, límites inválidos, caja inexistente y resultado vacío. Los logs no exponen `localPath` ni `googleDriveFileId`.
- Contrato real comparado con snapshot y tipos generados; schema de `CashRegisterOperationDto` verificado. Lint/build y ambas auditorías npm aprobados con cero vulnerabilidades informadas.
- EF Core confirma que no requiere migración. La base QA conserva 42 tablas, 345 filas y 11 migraciones; cero bases temporales residuales al cerrar.

### Prueba de navegador

Se verificaron caja, diario, respaldos y liquidaciones en escritorio de 1440 px y móvil de 390 px: ocho capturas y cero errores JavaScript, sin desbordamiento horizontal del documento. Las tablas conservan desplazamiento interno.

El navegador eligió L 70 en la primera página y L 100 en la segunda. Conservó ambos al cambiar de búsqueda y registró una liquidación real en la base sintética: **bruto L 170, comisión L 5, retención L 2, depósito L 163, dos aplicaciones y HTTP 201**. El pendiente global pasó de L 3.070 a L 2.900 y el conteo de 31 a 29. La misma prueba comprobó un 503 simulado del historial de respaldos y su recuperación con Actualizar.

La primera pasada detectó que la selección automática de caja dependía de la primera fila de cada página. Se corrigió para fijar la selección inicial; el flujo volvió a aprobar antes del cierre. Los datos visuales y financieros fueron sintéticos en una base independiente; se retiraron servidores, base y secretos temporales.

### Tamaño de respuesta sintética

| Colección | Filas anteriores | Filas de página | Bytes anteriores | Bytes de página |
|---|---|---|---|---|
| Cajas | 35 | 10 | 5.844 | 1.666 |
| Movimientos de caja | 31 | 10 | 13.334 | 4.365 |
| Liquidaciones | 33 | 10 | 21.749 | 6.635 |
| Cobros elegibles | 31 | 10 | 7.382 | 2.448 |
| Asientos | 52 | 10 | 38.356 | 1.935 |
| Logs de respaldo | 32 | 10 | 7.834 | 2.465 |

Cada filtro coincide con 31 registros nuevos. Algunas respuestas anteriores incluyen además registros preexistentes del flujo de integración; el número de filas está indicado. Asientos también cambia a una proyección sin líneas. Son tamaños de JSON sin compresión, no medidas de latencia ni garantías de rendimiento para la base oficial.

## Evidencia

- [QA integral](../evidencia/p02h2b-qa.log), [modelo EF](../evidencia/p02h2b-modelo-ef.log), [base conservada](../evidencia/p02h2b-base-qa.json).
- [Métricas](../evidencia/p02h2b-paginacion.json), [contratos pendientes](../evidencia/p02h2b-inventario-contratos.json), [detector Impeccable](../evidencia/p02h2b-impeccable.json).
- [Resultado del navegador y liquidación](../evidencia/ui-p02h2b/resultado.json), [resumen y hashes](../evidencia/p02h2b-qa-resumen.json).

| Pantalla | Escritorio | Móvil |
|---|---|---|
| Caja | [Captura](../evidencia/ui-p02h2b/caja-escritorio.png) | [Captura](../evidencia/ui-p02h2b/caja-movil.png) |
| Diario | [Captura](../evidencia/ui-p02h2b/asientos-escritorio.png) | [Captura](../evidencia/ui-p02h2b/asientos-movil.png) |
| Respaldos | [Captura](../evidencia/ui-p02h2b/respaldos-escritorio.png) | [Captura](../evidencia/ui-p02h2b/respaldos-movil.png) |
| Liquidaciones | [Captura](../evidencia/ui-p02h2b/liquidaciones-escritorio.png) | [Captura](../evidencia/ui-p02h2b/liquidaciones-movil.png) |

## Próxima entrega H2C

- [x] Paginar las seis colecciones financieras y de respaldo previstas en H2B.
- [x] Mantener saldos, totales globales y selección de lote entre páginas.
- [x] Migrar doce tipos, comprobar permisos y registrar una liquidación entre páginas en navegador.
- [ ] Migrar selectores de huéspedes/reservas de check-in/out, autorizaciones y cajas auxiliares.
- [ ] Revisar el inventario actualizado: 29 lecturas frontend de arrays y 53 respuestas 200/201 sin esquema. Son candidatos a revisión, no 82 defectos confirmados: incluye catálogos, reportes y respuestas vacías intencionales.
- [ ] Migrar tipos manuales restantes y completar respuestas explícitas de éxito por operación.
- [ ] Vincular la matriz formal API/DAT/SEC, completar seguridad y obtener revisión independiente de G2.

Sigue abierto el cálculo del detalle de folio registrado para P03: debe usar el desglose monetario del backend con descuentos y redondeo compartido. G0/G1 necesitan la evidencia y decisiones oficiales del hotel.

Windows 11, Docker Desktop, QA PowerShell e impresión física continúan aplazados a petición del usuario. Permanece activo el recordatorio diario de las 10:00 de Honduras hasta completar Windows e impresión. Esta entrega no sustituye dichas pruebas ni la aceptación fiscal.
