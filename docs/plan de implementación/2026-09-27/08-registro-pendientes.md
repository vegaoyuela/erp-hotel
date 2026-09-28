# 08 — Registro de pendientes y bloqueos

**Última actualización:** 27 de septiembre de 2026

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

Las entregas P02-A a P02-D completaron el reloj determinista, la normalización de errores, el verificador/exportador de auditoría y la extracción transaccional de pagos, reembolsos, liquidaciones, facturas, notas fiscales, reservas, check-in/out, folios y caja. Factura, notas, check-in, cargo de folio y apertura/cierre de caja demuestran rollback sin residuos mediante fallos inyectados. Permanecen abiertas compras, inventario y respaldo; las pruebas de fallo de los movimientos financieros P02-B; la eliminación de persistencia implícita en repositorios; P02-03 a P02-07; la matriz completa P02-09 y la revisión independiente P02-10. El detalle y el orden de la siguiente entrega están en [P02 — base técnica](p02-base-tecnica/README.md).
