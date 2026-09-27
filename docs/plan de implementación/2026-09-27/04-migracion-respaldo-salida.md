# 04 — Migración, respaldo, aceptación y salida

## P09. Respaldo y recuperación

## Conjunto mínimo de respaldo

- PostgreSQL en formato restaurable y con versión de servidor registrada.
- Adjuntos de autorizaciones y soportes.
- PDF y representaciones fiscales conservables.
- Configuración no secreta y manifiesto de versión.
- Identificadores de imagen/contenedor y migraciones aplicadas.
- Huellas SHA-256 del conjunto.
- Copia cifrada de secretos custodiada por separado y solo cuando la política lo permita.

## Reglas

- Nunca considerar un respaldo válido hasta restaurarlo.
- Conservar al menos una copia fuera del equipo Windows.
- Cifrar antes de copiar a nube, USB o NAS.
- Registrar RPO/RTO acordados, tamaño, duración y resultado.
- Probar recuperación con el mismo Compose y con un equipo alterno.
- Verificar después de restaurar: conteos, correlativos, documentos, asientos, adjuntos, bitácora y usuarios.
- No borrar la última copia aprobada por una política automática de retención.

## Pruebas

1. pérdida de contenedor con volumen intacto;
2. pérdida de volumen con respaldo disponible;
3. Docker Desktop reinstalado;
4. equipo alterno con ruta diferente;
5. adjunto faltante o hash incorrecto;
6. restauración de versión previa con migración posterior;
7. recuperación después de una emisión confirmada.

## Migración de datos reales

### Descubrimiento

- Identificar origen de cada tabla/archivo y distinguir real, histórico, sintético y semilla.
- Inventariar conteos, rangos fiscales, duplicados, huérfanos y estados imposibles sin copiar PII a `docs`.
- Preservar el original como solo lectura y registrar su hash.
- Mapear IDs, clientes, huéspedes, habitaciones, reservas, folios, documentos, líneas, pagos, caja, compras, inventario, cuentas y adjuntos.

### Transformación

- Crear una herramienta repetible con staging, validaciones y tabla de rechazos.
- No recalcular impuestos o totales históricos ni inventar aplicaciones de pago.
- Mantener los números y CAI históricos como snapshots.
- Separar documentos pagados sin evidencia de cobro en una cola de conciliación.
- Añadir constraints únicamente después de prevalidar y aislar datos inválidos.
- Ejecutar dry-run cuantas veces sea necesario; una ejecución repetida no debe duplicar.

### Conciliación

- Conteos por entidad y estado.
- Rango inicial/final, último emitido y números no utilizados.
- Bases 15/18/exentas/exoneradas, impuestos, turismo y totales.
- Debe/Haber y saldos de cuentas de control.
- Facturas, pagos, notas, reembolsos y cartera.
- Compras, proveedores, retenciones y banco.
- Kardex, existencia y valoración.
- Adjuntos, PDF y cadena de auditoría.

## Corte y recuperación

1. anunciar ventana y responsables;
2. congelar escrituras en el sistema anterior;
3. crear respaldo final y manifiesto;
4. ejecutar migración final;
5. conciliar los controles acordados;
6. ejecutar smoke y casos críticos;
7. autorizar apertura o volver al sistema anterior;
8. registrar toda emisión/operación realizada después del corte.

Antes de la primera emisión nueva se permite rollback completo. Después de emitir, la recuperación debe preservar esos documentos y reconciliarlos; no se puede restaurar una copia antigua destruyendo historia.

## Salida G9

- Respaldo completo restaurado en otro equipo dentro del RTO.
- Migración repetible, rechazos resueltos o formalmente aceptados.
- Conciliación firmada por gerente y contador.
- Procedimiento de contingencia probado por otra persona.

## P11. Aceptación integral

### Ejecución

- Marcar cada uno de los 186 casos como Aprobado, Fallido, Bloqueado o No aplica con justificación.
- Ejecutar una jornada completa que incluya reserva, anticipo, cargos, factura, pagos, nota, compra, caja, kardex, reportes y cierre.
- Comparar importes con un cálculo independiente del contador.
- Probar cuatro roles sin usar cuentas administrativas para tareas ordinarias.
- Probar impresora, restauración, baja de usuario, período cerrado y correlativos.
- Repetir sobre la distribución exacta que será instalada.
- Actualizar manual de usuario, videos, instalación, contingencia y soporte.

### Defectos

- Crítico: corrupción, documento fiscal incorrecto, pérdida de historia, fuga de secretos, privilegio indebido o imposibilidad de recuperar. Bloquea salida.
- Alto: operación monetaria incorrecta, duplicado, permiso roto, fallo de impresión sin recuperación o flujo esencial inutilizable. Bloquea salida.
- Medio: alternativa segura disponible. Requiere responsable, mitigación y fecha.
- Bajo: cosmético o documental sin impacto operativo. Puede pasar con registro.

## Actas requeridas

- Acta técnica: versión, pruebas, seguridad, arquitectura y limitaciones.
- Acta de datos: origen, migración, rechazos y conciliación.
- Acta contable/fiscal interna: perfil, ejemplos, reportes y documentos.
- Acta operativa: recorrido por roles y capacitación.
- Acta de infraestructura: Windows, Docker, red, certificados, impresora y recuperación.
- Autorización de salida: fecha/hora, responsables y plan de reversión.

## Liberación

- Etiquetar release y fijar digests de imágenes.
- Generar changelog, SBOM y hashes.
- Conservar Compose, scripts, manuales y matriz de compatibilidad.
- Establecer canal de incidentes y responsable de guardia.
- Restringir cambios directos en producción.
- Monitorear salud, errores, espacio, respaldos, correlativos y trabajos de impresión.

## Seguimiento inicial

- Conciliar cada jornada durante los primeros cinco días.
- Revisar correlativos, anulaciones, notas y caja a diario.
- Verificar respaldo y copia externa diariamente.
- Revisar la primera semana con gerente y contador.
- Cerrar el primer mes con libros, auxiliares y mayor conciliados.
- No retirar el sistema anterior ni su evidencia hasta completar el período de conservación acordado.

## Salida G11

- Cero defectos críticos o altos abiertos.
- G0–G10 aprobadas con evidencia.
- Release reproducible instalada y recuperable.
- Usuarios capacitados y suplentes designados.
- Actas firmadas y expediente fiscal completo.
- Primera jornada autorizada con plan de contingencia disponible.
