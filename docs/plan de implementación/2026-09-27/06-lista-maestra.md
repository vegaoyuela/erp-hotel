# 06 — Lista maestra de implementación

La lista se ejecuta en orden de puertas. Una tarea marcada requiere código, prueba y evidencia según la definición del [plan principal](README.md#definición-de-terminado-para-cada-tarea).

## P00 — Línea base reproducible

- [x] P00-01 Crear rama, etiqueta, inventario de cambios y commit base.
- [x] P00-02A Respaldar y restaurar la base sintética de QA en una base aislada.
- [ ] P00-02B Respaldar y restaurar la base oficial del hotel. **Bloqueada:** la fuente oficial aún no se ha recibido.
- [x] P00-03A Inventariar adjuntos, documentos, configuración y semillas disponibles sin copiar PII.
- [ ] P00-03B Contrastar el inventario con adjuntos y documentos oficiales. **Bloqueada:** los archivos oficiales aún no se han recibido.
- [x] P00-04A Generar manifiesto y hashes de aplicación y datos sintéticos.
- [ ] P00-04B Incorporar al manifiesto los hashes de los datos oficiales cuando se reciban.
- [x] P00-05 Retirar SQLite, logs, `bin/obj`, bundles obsoletos y segundo lockfile del control de versiones.
- [x] P00-06 Ejecutar QA completa sobre PostgreSQL y registrar línea base.
- [x] P00-07 Resolver todos los defectos críticos descubiertos en la línea base disponible.
- [ ] **G0** Aprobar línea base recuperable. **Pendiente:** P00-02B, P00-03B y P00-04B.

Evidencia de esta ejecución: [P00 — estado y resultados](evidencia/README.md).

## P01 — Perfil y políticas del hotel

- [ ] P01-01 Completar DEC-01–DEC-15 con gerente y contador.
- [x] P01-02A Crear plantilla, procedimiento de custodia y referencias por hash sin PII.
- [ ] P01-02B Digitalizar y custodiar la evidencia fiscal real fuera del repositorio.
- [x] P01-03 Versionar perfil fiscal y reglas por vigencia mediante snapshot y SHA-256 inmutables.
- [ ] P01-04 Completar plan de cuentas y valoración de inventario.
- [ ] P01-05 Definir anticipos, crédito, descuentos, anulaciones y no utilizados.
- [ ] P01-06 Definir retenciones, exoneraciones y conservación.
- [ ] P01-07 Definir RPO/RTO, hardware, usuarios y aprobación por montos.
- [x] P01-08A Automatizar validaciones estructurales N01–N06 y cubrirlas con pruebas sintéticas.
- [ ] P01-08B Ejecutar y aprobar N01–N06 sobre el expediente oficial externo.
- [ ] **G1** Firmar perfil fiscal/contable y políticas.

Pendientes externos: [registro único de bloqueos](08-registro-pendientes.md). Evidencia técnica: [ejecución P01](evidencia/README.md#p01--perfil-fiscal-y-expediente).

## P02 — Base técnica, contratos y seguridad

- [x] P02-01 Extraer casos de uso críticos de controladores. Finanzas, documentos fiscales, reservas, check-in/out, folios, caja, compras, inventario y respaldo delegan su mutación a servicios.
- [x] P02-02 Unificar unidad de trabajo y retirar guardados parciales. **Completada el 28 de octubre:** 56 guardados implícitos retirados, consumidores migrados a `IUnitOfWork`, rollback y persistencia explícita verificados sobre PostgreSQL.
- [x] P02-03 Inyectar reloj, cálculo, correlativo, auditoría, impresión y respaldo. **Completada en código el 28 de octubre:** contratos registrados y consumidores migrados; sustitución de cálculo/auditoría con rollback y transporte de impresión simulado verificados. La prueba física permanece en Windows.
- [ ] P02-04 Completar OpenAPI, `ProblemDetails`, paginación, fechas y enums. **Parcial:** errores 4xx normalizados con código y `traceId`.
- [ ] P02-05 Generar/validar tipos frontend desde el contrato.
- [ ] P02-06 Completar permisos y motivos/aprobaciones sensibles.
- [ ] P02-07 Rotar secretos y separar migrador/aplicación/respaldo/bootstrap.
- [x] P02-08 Crear verificador/exportador de cadena de auditoría.
- [ ] P02-09 Ejecutar API01–API05, DAT01–DAT03 y SEC01–SEC11. **Parcial:** regresión base aprobada; falta matriz formal completa.
- [ ] P02-10 Registrar revisión técnica independiente.
- [ ] **G2** Aprobar base técnica y seguridad.

Seguimiento detallado: [ejecución P02](p02-base-tecnica/README.md).

## P03 — Núcleo fiscal, monetario y contable

- [ ] P03-01 Consolidar autorizaciones fiscales y migración puente.
- [ ] P03-02 Completar unicidad, vigencia, suspensión, agotamiento y no utilizados.
- [ ] P03-03 Ejecutar concurrencia de 50 emisiones y frontera de rango.
- [ ] P03-04 Versionar reglas monetarias por concepto/vigencia.
- [ ] P03-05 Implementar cotización versionada y conflicto al confirmar.
- [ ] P03-06 Completar snapshot/hash y separación fiscal/cobro/impresión.
- [ ] P03-07 Completar idempotencia e inyección de fallos en operaciones monetarias.
- [ ] P03-08 Crear períodos, cierre, reapertura y reversos.
- [ ] P03-09 Versionar mapeo evento–cuenta y consolidación contable.
- [ ] P03-10 Ensayar base nueva y actualización con prevalidación.
- [ ] P03-11 Ejecutar FIS, MON, TX, NC, CTA y CON aplicables.
- [ ] **G3** Aprobar conciliación de documentos y contabilidad.

## P04 — Operación hotelera, pagos y caja

- [ ] P04-01 Implementar anticipos/depósitos y su aplicación.
- [ ] P04-02 Implementar salida a crédito y no-show según política.
- [ ] P04-03 Extender idempotencia y conservación de formulario.
- [ ] P04-04 Implementar aprobación segregada por monto/medio/origen.
- [ ] P04-05 Implementar conciliación bancaria.
- [ ] P04-06 Implementar reversos/contracargos del adquirente.
- [ ] P04-07 Completar recorrido E2E de jornada hotelera.
- [ ] P04-08 Ejecutar HOT01–HOT10 y PAG01–PAG08.
- [ ] **G4** Aprobar jornada, cartera y caja conciliadas.

## P05 — Compras, retenciones e inventario

- [ ] P05-01 Completar documento fiscal de proveedor y unicidad.
- [ ] P05-02 Clasificar gasto/inventario/activo e ISV.
- [ ] P05-03 Implementar cuentas por pagar, pagos, notas y reversos.
- [ ] P05-04 Implementar retenciones confirmadas y constancias.
- [ ] P05-05 Crear UI completa de proveedores/compras/pagos.
- [ ] P05-06 Sustituir stock editable por kardex.
- [ ] P05-07 Implementar valoración, costo histórico y costo de venta.
- [ ] P05-08 Proteger stock con locks y aprobaciones de ajuste.
- [ ] P05-09 Ejecutar COM01–COM07, RET01–RET05 e INV01–INV06.
- [ ] **G5** Aprobar compras e inventario conciliados.

## P06 — Reportes, auditoría y documentos

- [ ] P06-01 Completar períodos y cuentas de control.
- [ ] P06-02 Reconstruir libro de ventas por documento/estado.
- [ ] P06-03 Crear auxiliares fiscales, cartera, proveedores y caja.
- [ ] P06-04 Implementar exportaciones versionadas y totales de control.
- [ ] P06-05 Validar obligaciones/reportes con contador.
- [ ] P06-06 Completar cadena, ancla y verificación de auditoría.
- [ ] P06-07 Crear modelo común preview/PDF/ESC-POS desde snapshot.
- [ ] P06-08 Persistir cola y estado de trabajos de impresión.
- [ ] P06-09 Ejecutar REP01–REP08, AUD01–AUD05 e IMP01–IMP07.
- [ ] **G6** Aprobar expediente fiscal/contable consistente.

## P07 — Frontend, rendimiento y QA

- [ ] P07-01 Centralizar cliente API y tipos.
- [ ] P07-02 Completar estados carga/vacío/error/permiso/éxito.
- [ ] P07-03 Completar teclado, foco, labels, contraste y reducción de movimiento.
- [ ] P07-04 Validar todas las pantallas en móvil, escritorio y zoom 200 %.
- [ ] P07-05 Implementar paginación, debounce, cancelación y timeout.
- [ ] P07-06 Optimizar consultas e índices con planes medidos.
- [ ] P07-07 Añadir Vitest/Testing Library.
- [ ] P07-08 Añadir Playwright E2E por rol.
- [ ] P07-09 Integrar pruebas frontend/E2E a QA Bash y PowerShell.
- [ ] P07-10 Medir p50/p95, memoria, consultas y locks en Windows objetivo.
- [ ] P07-11 Ejecutar UI01–UI16, PERF01–PERF05 y E2E01–E2E06.
- [ ] **G7** Aprobar operación sin asistencia de desarrollo.

## P08 — Docker y Windows 11

- [ ] P08-01 Definir Compose base/producción sin secretos fijos.
- [ ] P08-02 Endurecer imágenes, usuarios, healthchecks y versiones/digests.
- [ ] P08-03 Crear servicios db, migrator, api, gateway y backup.
- [ ] P08-04 Implementar lectura de Compose secrets en API.
- [ ] P08-05 Configurar volúmenes, rotación de logs y límites.
- [ ] P08-06 Configurar HTTPS, certificado, mismo origen y headers.
- [ ] P08-07 Configurar firewall Windows/Hyper-V solo para 443/LAN.
- [ ] P08-08 Crear instalador, actualizador, smoke y desinstalador PowerShell.
- [ ] P08-09 Crear Print Agent nativo e `IPrinterTransport`.
- [ ] P08-10 Probar instalación limpia, actualización y reinicio Windows.
- [ ] P08-11 Probar rotación de secretos/certificado y rollback previo a emisión.
- [ ] **G8** Aprobar distribución Windows reproducible.

## P09 — Respaldo, recuperación y migración

- [ ] P09-01 Crear respaldo integral cifrado y manifiesto.
- [ ] P09-02 Implementar copia fuera del equipo y retención segura.
- [ ] P09-03 Restaurar en equipo alterno y medir RPO/RTO.
- [ ] P09-04 Inventariar y clasificar datos reales/sintéticos.
- [ ] P09-05 Crear staging, mapeo y herramienta idempotente de migración.
- [ ] P09-06 Resolver rechazos y conciliar documentos/asientos/stock.
- [ ] P09-07 Ensayar congelamiento, corte, rollback y recuperación posterior.
- [ ] P09-08 Ejecutar M01–M06 y OPS01–OPS08.
- [ ] **G9** Aprobar migración y recuperación.

## P10 — Factura en Windows

- [ ] P10-01 Aprobar W0 vista previa.
- [ ] P10-02 Aprobar W1 agente de impresión.
- [ ] P10-03 Aprobar W2 calibración de papel.
- [ ] P10-04 Aprobar W3 facturas sintéticas.
- [ ] P10-05 Aprobar W4 notas, reimpresión y fallos.
- [ ] P10-06 Autorizar W5 con perfil/rango real, si procede.
- [ ] P10-07 Cerrar incidencias y conservar evidencia.
- [ ] **G10** Propietario y contador aprueban factura/impresión.

## P11 — Aceptación y salida

- [ ] P11-01 Ejecutar y clasificar los 186 casos.
- [ ] P11-02 Ejecutar jornada integral y oráculo independiente.
- [ ] P11-03 Realizar UAT por los cuatro roles.
- [ ] P11-04 Cerrar todos los defectos críticos y altos.
- [ ] P11-05 Actualizar manuales, videos, instalación y contingencia.
- [ ] P11-06 Firmar actas técnica, datos, contable, operativa e infraestructura.
- [ ] P11-07 Crear release, SBOM, hashes, digests y changelog.
- [ ] P11-08 Ejecutar corte y apertura autorizada.
- [ ] P11-09 Conciliar primeras cinco jornadas y primer mes.
- [ ] **G11** Autorizar liberación definitiva.
