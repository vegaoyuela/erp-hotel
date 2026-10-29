# 08 — Registro de pendientes y bloqueos

**Última actualización:** 29 de octubre de 2026

Este registro separa lo que depende del código de aquello que requiere información o aprobación del hotel. Una tarea externa no se cierra con datos supuestos ni con el expediente sintético de QA.

## Bloqueos para G0

| ID | Pendiente | Responsable | Condición para cerrarlo | Evidencia |
|---|---|---|---|---|
| P00-02B | Entregar y restaurar una copia de la base oficial | Propietario / TI | Dump oficial identificado, restaurado en una base aislada y comparado | Resultado de restauración y SHA-256 sin PII |
| P00-03B | Entregar inventario de adjuntos y documentos oficiales | Propietario / TI | Conteo, tamaños y clasificación conciliados con el sistema actual | Manifiesto de archivos; contenido fuera de Git |
| P00-04B | Incorporar hashes de fuentes oficiales | Propietario / TI | Base, adjuntos, configuración y versión quedan enlazados al manifiesto | Manifiesto firmado o aprobado |

## Decisiones requeridas para G1

| ID | Decisión pendiente | Responsable principal | Evidencia mínima de cierre |
|---|---|---|---|
| DEC-01 | Identidad fiscal, establecimiento, punto y obligaciones activas | Gerente + contador | Documentos vigentes y acta de contraste |
| DEC-02 | Modalidad, registro del sistema, tipos, CAI y rangos | Contador | Autorizaciones y referencias de trámite |
| DEC-03 | Catálogo real y clasificación 15 %, 18 %, exenta o exonerada | Contador | Catálogo firmado con vigencia |
| DEC-04 | Tasa turística y condición ante IHT | Contador | Sustento de aplicación o no aplicación |
| DEC-05 | Anticipos, depósitos, no-show, crédito y momento de emisión | Gerente + contador | Política operativa y asientos modelo |
| DEC-06 | Descuentos y beneficios legales | Gerente + contador | Reglas, soportes, acumulación y límites |
| DEC-07 | Retenciones practicadas y sufridas | Contador | Matriz de tasa, base, momento, constancia y cuenta |
| DEC-08 | Exoneraciones | Contador | Requisitos, alcance y custodia del soporte |
| DEC-09 | Plan de cuentas y valoración de inventario | Contador | Plan aprobado, cuentas de control y método |
| DEC-10 | Moneda y tipo de cambio | Gerente + contador | Moneda inicial y fuente/política de conversión |
| DEC-11 | Anulaciones, no utilizados y contingencia | Contador | Procedimiento, autorizaciones y responsables |
| DEC-12 | Conservación documental | Gerente + contador | Tabla de retención y ubicación de originales/copias |
| DEC-13 | Impresora, papel, original/copia y expediente térmico | Propietario Windows + contador | Modelo, papel y prueba física aprobada |
| DEC-14 | Equipo Windows, usuarios, RPO y RTO | Gerente + TI | Inventario del equipo y objetivos firmados |
| DEC-15 | ISR, nómina, activos y obligaciones fuera del ERP | Gerente + contador | Matriz de alcance con responsable externo |

## Condiciones técnicas pendientes de P01

| ID | Estado | Condición de cierre |
|---|---|---|
| P01-01 | Bloqueada por DEC-01–DEC-15 | Las quince decisiones deben estar aprobadas o sustentadas como no aplicables. |
| P01-02 | Preparada | Custodiar el expediente real fuera del repositorio y conservar en Git solo el reporte sin valores. |
| P01-03 | Completada en código | Cada aprobación conserva snapshot, hash, vigencia, actor, nota, manifiesto y referencia externa; el retiro actualiza su estado sin reescribir el snapshot. |
| P01-04 | Bloqueada por DEC-09 | Aprobar plan de cuentas y valoración. |
| P01-05 | Bloqueada por DEC-05, DEC-06 y DEC-11 | Aprobar políticas de operación y excepción. |
| P01-06 | Bloqueada por DEC-07, DEC-08 y DEC-12 | Aprobar retenciones, exoneraciones y conservación. |
| P01-07 | Bloqueada por DEC-13 y DEC-14 | Definir infraestructura, impresión, RPO/RTO y usuarios. |
| P01-08 | P01-08A completada; P01-08B pendiente | Ejecutar N01–N06 con `--require-official` sobre el expediente real. |

## Regla de custodia

La plantilla puede copiarse desde [expediente P01](p01-expediente/README.md), pero el archivo completado, las autorizaciones, RTN, CAI, firmas y documentos deben permanecer fuera de Git. El validador produce un reporte con hash, estados y rutas de campos; no replica los valores del expediente.

## Pendientes técnicos abiertos de P02

Las entregas P02-A a P02-G completaron la extracción de casos críticos, el rollback financiero/fiscal/operativo, la eliminación de los 56 guardados implícitos y los contratos pendientes de cálculo, auditoría e impresión. Los consumidores usan interfaces y persistencia explícita; la auditoría y los correlativos conservan sus locks y transacciones. Las pruebas sustituyen cálculo/auditoría con fallos y recorren la impresión con un transporte simulado. Permanecen abiertos P02-04 a P02-07; la matriz completa P02-09; la revisión independiente P02-10; y las tareas de impresión física, instalación Windows y aceptación fiscal del hotel. El detalle está en [P02 — base técnica](p02-base-tecnica/README.md) y en la [entrega P02-G](p02-base-tecnica/p02-g-contratos.md).


### Avance P02-H1 — contrato y paginación (28 de octubre de 2026)

OpenAPI y tipos TypeScript están versionados y se validan contra el contrato de una API real en QA. Huéspedes, facturas y auditoría consumen paginación SQL; los filtros fiscales se combinan y la bitácora ya no queda limitada a los 200 primeros eventos. Se verificaron escritorio y móvil con datos sintéticos.

P02-H2 debe completar las demás colecciones paginadas, declarar los esquemas de éxito de las acciones aún sin DTO explícito y sustituir los tipos frontend manuales restantes. P02-04 y P02-05 siguen parciales. No cambia el estado de G0/G1, P02-06/P02-07/P02-09/P02-10 ni la aceptación fiscal, impresión física y ejecución de QA PowerShell en Windows. Véase [entrega P02-H1](p02-base-tecnica/p02-h1-contrato-paginacion.md).


### Avance P02-H2A — operación e inventario (29 de octubre de 2026)

Se paginaron reservas, folios, clientes, productos y movimientos; los indicadores globales y el selector paginado de Kardex evitan depender de una sola página. Se migraron nueve tipos a OpenAPI y se documentaron los resultados de reservas y cargos de folio. El inventario identifica 35 lecturas frontend de arrays y 56 respuestas 200/201 sin esquema para revisión individual; no todas son defectos.

H2B debe cubrir caja, liquidaciones, asientos, respaldos, selectores y contratos restantes. Se registra para P03 el recálculo de totales de detalle de folio en frontend, que debe reemplazarse por desglose backend y probar descuentos/redondeos. El [detalle H2A](p02-base-tecnica/p02-h2a-operacion-inventario.md) contiene rutas, límites y evidencia.

**Windows e impresión aplazados por el usuario:** retomar QA PowerShell, instalación Docker Desktop/Windows 11 e impresión física y aceptación de factura. Recordatorio diario activo a las 10:00 de Honduras hasta completar ambas; ID `pendientes-windows-11-e-impresi-n-del-hotel`. No se cierran con las pruebas Linux ni con transporte simulado.


### Avance P02-H2B — finanzas y respaldos (29 de octubre de 2026)

Completadas las seis colecciones financieras/de respaldo previstas: cajas, movimientos, liquidaciones, cobros elegibles, asientos y logs. Los saldos y totales globales son independientes de la página y el navegador conservó dos cobros entre páginas para registrar un lote sintético real. Se migraron doce tipos y se comprobaron las políticas de los cuatro roles. El [detalle H2B](p02-base-tecnica/p02-h2b-finanzas-respaldos.md) contiene contrato, pruebas y ocho capturas.

H2C debe revisar selectores de check-in/out, autorizaciones/cajas auxiliares, 29 lecturas de arrays y 53 respuestas 200/201 sin esquema; cada caso requiere clasificación. Persisten los contratos manuales restantes, seguridad, matriz formal e independencia de G2, además del cálculo de folio ya registrado en P03. Windows e impresión mantienen su aplazamiento y recordatorio diario.

### Avance P02-H2C — selectores y contratos (29 de octubre de 2026)

Se completaron los selectores principales de check-in/out, autorizaciones y cajas; la selección persiste entre páginas y búsquedas. Se clasificaron las 53 respuestas sin esquema de H2B: el contrato final tiene cero éxitos 200/201 sin esquema y ocho descargas binarias. Se migraron 28 interfaces compartidas utilizadas y se retiró una sin consumidores. P02-05 queda completada en código y QA. Véase [detalle H2C](p02-base-tecnica/p02-h2c-selectores-contratos.md).

P02-04 permanece parcial: las 19 lecturas de arrays restantes están [clasificadas](evidencia/p02h2c-inventario-contratos.json); usuarios/roles requieren paginación administrativa y los catálogos/reportes requieren validar volumen/límites. Los auxiliares de una factura deben conservar su completitud hasta disponer de agregados equivalentes. El siguiente bloque cubre P02-06 (permisos, motivos y aprobaciones), esas tareas residuales y la matriz API/DAT/SEC de P02-09. P02-07/P02-10 siguen pendientes en su alcance documentado.

No cambia el defecto P03 de desglose de folio, G0/G1, decisiones reales del gerente/contador ni aprobación productiva/fiscal. Windows 11, Docker Desktop, QA PowerShell e impresión física siguen aplazados con el recordatorio diario existente a las 10:00 de Honduras.

### Avance P02-I — permisos y administración (29 de octubre de 2026)

Comprobadas lecturas sensibles y navegación por permisos; el dashboard minimiza información por sección. `adjust_invoices` separa notas de la emisión ordinaria y se asigna a Admin/Contador. La gestión delegada rechaza seis formas de alterar cuentas/roles superiores o conceder Recepción por la ruta heredada. Usuarios y opciones de roles ya se paginan, sin exigir `manage_roles` para el selector del gestor de usuarios. El [detalle](p02-base-tecnica/p02-i-accesos-seguridad.md) incluye QA, doce capturas y anexo de uso.

Se registró la [matriz formal API/DAT/SEC](p02-base-tecnica/p02-i-matriz-api-dat-sec.md), con brechas por caso; P02-09 no queda cerrada. El siguiente bloque debe proteger cambios de seguridad bajo concurrencia (incluido último administrador), unificar atomicidad/auditoría de sus mutaciones exitosas y avanzar las aprobaciones sensibles; los umbrales por monto/medio/origen requieren decisiones del hotel. P02-04 conserva catálogos/reportes, límites de volumen y clientes heredados. P02-07/P02-10, P03-folio y G0/G1 siguen abiertos.

Windows 11, Docker Desktop, QA PowerShell e impresión física permanecen aplazados con el recordatorio diario existente a las 10:00 de Honduras.
