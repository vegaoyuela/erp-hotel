# 01 — Alcance funcional, fiscal y contable

## P00. Línea base y limpieza controlada

### Cambios

- Crear una rama de implementación y etiquetar el commit de inicio.
- Exportar el estado de PostgreSQL, adjuntos, configuración y versión antes de cualquier nueva migración.
- Eliminar del control de versiones `bin/`, `obj/`, logs, SQLite legado, bundles compilados obsoletos y el segundo lockfile del frontend. Conservar primero cualquier cambio legítimo.
- Mantener `package-lock.json` como única fuente de dependencias del frontend.
- Crear un manifiesto con SHA-256 de base, adjuntos, imágenes de contenedor, migraciones y commit.
- Ejecutar `scripts/qa.sh` con PostgreSQL real y guardar el resultado inicial.

### Archivos principales

- `.gitignore`
- `frontend/package-lock.json`
- `frontend/pnpm-lock.yaml`
- `backend/src/hotel-erp.Api/Data/hotel.db`
- `backend/src/hotel-erp.Api/bin/`, `obj/`, `logs/`, `wwwroot/assets/`
- `scripts/qa.sh` y `scripts/qa.ps1`

### Pruebas y salida G0

- Instalación de dependencias desde cero.
- Backend y frontend con cero errores y cero advertencias propias.
- 76 pruebas unitarias y 4 integraciones actuales continúan aprobadas.
- No hay secretos, datos personales, bases, adjuntos o artefactos de compilación versionados.
- Respaldo previo restaurado en una base aislada y comparado por manifiesto.

## P01. Decisiones del hotel y perfil aprobado

El desarrollo no debe inventar estos datos. Gerente y contador deben completar un expediente con:

1. RTN, razón social, nombre comercial, domicilio, teléfonos y correo;
2. establecimiento, punto de emisión y modalidad de facturación;
3. obligaciones activas, registro aplicable y autorizaciones CAI por tipo;
4. rangos, fechas de recepción, activación y vencimiento;
5. productos y servicios, clasificación 15 %, 18 %, exenta o exonerada;
6. aplicación de tasa turística y condición ante IHT;
7. anticipos, depósitos, no-show, crédito y momento de emisión;
8. descuentos comerciales y beneficios legales, soportes y acumulación;
9. retenciones practicadas y sufridas;
10. exoneraciones y documentos de respaldo;
11. moneda, tipo de cambio y restricción inicial a HNL;
12. plan de cuentas, cuentas de control y valoración de inventario;
13. anulaciones, números no utilizados y contingencia;
14. conservación documental, impresora, papel, original y copia;
15. RPO, RTO, usuarios simultáneos y obligaciones fuera del alcance.

### Implementación

- Extender `BusinessSettings`/perfil fiscal con los campos aprobados y vigencias necesarias.
- Mantener estados Borrador, Aprobado y Retirado; prohibir edición retroactiva.
- Exigir motivo, usuario y fecha para aprobar o retirar.
- Versionar reglas fiscales y contables en lugar de sustituir valores históricos.
- Bloquear cualquier emisión real si falta el perfil, una autorización o una regla aplicable.

### Pruebas y salida G1

- Comparar cada campo con la documentación aportada.
- Probar vigencia, retiro y uso de versiones históricas.
- Rechazar emisión con perfil incompleto, futuro, retirado o no aprobado.
- Contador y gerente firman el expediente; los documentos se custodian fuera del repositorio.

## P02. Contratos, servicios y seguridad

### Refactor obligatorio

- Extraer de controladores los casos de uso de emisión, notas, pagos, check-in/out, compras, inventario, cierres y respaldos.
- Mantener una transacción exterior por operación compuesta; ningún repositorio debe confirmar parcialmente.
- Inyectar reloj de Honduras, numerador, cálculo, auditoría, impresión y almacenamiento.
- Publicar contratos OpenAPI y generar o validar los tipos TypeScript.
- Normalizar errores con `ProblemDetails`, `traceId`, códigos estables y campos de validación.
- Introducir paginación, filtros y orden estables en listados con crecimiento.
- Separar fechas fiscales `DateOnly` de instantes UTC y usar intervalos semiabiertos.
- Completar DTO de detalle para líneas de factura, folio, compras, pagos y asientos.

### Seguridad

- Conservar access token en memoria y refresh en cookie `HttpOnly`, `Secure`, `SameSite=Strict`.
- Mantener CSRF de doble envío, CORS exacto, rate limiting y revocación por versión de seguridad.
- Aplicar permisos en backend a cada lectura sensible y toda mutación; el menú nunca sustituye la autorización.
- Completar motivos y aprobaciones para notas, anulaciones, descuentos extraordinarios, ajustes, reaperturas y diferencias.
- Canonicalizar bitácora, bloquear actualización/borrado con la cuenta de aplicación y crear verificador de cadena.
- Rotar todos los secretos de instalación y separar credenciales de migrador, aplicación, respaldo y administrador inicial.

### Pruebas y salida G2

- Contratos API01–API05 y fechas DAT01–DAT03.
- Matriz anónimo/Recepción/Caja/Contador/Admin sobre todos los endpoints.
- Revocación tras cambio de contraseña, baja de usuario y logout en dos pestañas.
- Rate limit, CSRF, CORS, adjuntos falsos, traversal y errores sin SQL/stack.
- Revisión técnica independiente registrada.

## P03. Núcleo fiscal, monetario y contable

### Autorizaciones y correlativos

- Consolidar `CAI` y `DocumentAuthorization` en un modelo `FiscalAuthorization` o documentar la compatibilidad definitiva.
- Estructurar establecimiento, punto, tipo, rango numérico, vigencia, estado y evidencia.
- Aplicar unicidad por identidad fiscal y tipo; impedir reiniciar o reutilizar rango desde la interfaz.
- Bloquear autorización futura, vencida, agotada, suspendida o de tipo incorrecto.
- Registrar números no utilizados y su trámite sin permitir reutilización.
- Probar 50 emisiones concurrentes, agotamiento con un solo número y reintento idempotente.

### Motor monetario

- Versionar reglas por concepto y vigencia.
- Conservar bases 15 %, 18 %, exentas, exoneradas y turismo por línea.
- Calcular cantidad, precio, descuento, base, impuesto y total solo en servidor.
- Implementar cotización versionada y volver a calcular al confirmar; mostrar conflicto si cambió.
- Diferenciar tasa cero legítima de configuración ausente.
- Validar redondeo al centavo con oráculos independientes firmados por el contador.

### Documentos y pagos

- Separar estado fiscal, estado de cobro y estado de representación/impresión.
- Completar snapshot fiscal y hash canónico versionado.
- Separar factura, proforma, recibo interno, nota y anulación.
- Extender transacción e idempotencia a toda operación financiera.
- Persistir trabajos de representación e impresión; reintentar el trabajo sin emitir otro correlativo.
- Conservar límites acumulados por línea para notas y saldos a favor/reembolsos.

### Contabilidad

- Versionar el mapeo evento–cuenta aprobado.
- Prohibir imputar a cuentas agrupadoras/inactivas y borrar asientos contabilizados.
- Implementar períodos abiertos, cierre, reapertura y control transaccional.
- Crear reversos enlazados en lugar de editar historia.
- Conciliar factura, cobro, nota, reembolso, tarjeta, anticipo, compra y pago a proveedor.
- Completar ejercicio, saldo inicial, resultado y consolidación del árbol sin doble conteo.

### Archivos principales

- `Database/Entities/InvoiceEntities.cs`
- `Database/Entities/AccountingEntities.cs`
- `Database/Entities/PaymentEntities.cs`
- `Services/FiscalAuthorizationService.cs`
- `Services/InvoiceCalculationService.cs`
- `Services/AccountingService.cs`
- `Services/IdempotencyService.cs`
- `Controllers/InvoicesController.cs`
- `Controllers/AccountingController.cs`
- nuevas migraciones bajo `Migrations/`

### Salida G3

- Factura, notas, pagos y asientos concilian al centavo.
- Inyección de fallo después de cada escritura no deja datos parciales.
- Reintentos y concurrencia no duplican documento, número, pago, crédito o asiento.
- La reimpresión posterior a cambios de catálogo produce la misma representación histórica.
- Ejemplos E01–E08 aplicables son aprobados por contador y QA.

## P04. Operación hotelera, pagos y caja

### Trabajo restante

- Implementar anticipos y depósitos con auxiliar, aplicación y saldo; no tratarlos como descuento o ingreso duplicado.
- Definir salida a crédito, no-show y cancelación con las políticas firmadas.
- Extender idempotencia y conservación de formulario a todas las mutaciones del flujo.
- Aplicar circuito segregado de aprobación por monto, medio y origen.
- Añadir conciliación bancaria y reversos/contracargos del adquirente.
- Completar jornada reserva → check-in → consumos → factura → pagos → checkout → cierre.

### Pruebas y salida G4

- Reservas adyacentes, solapadas, traslado, mantenimiento, capacidad y versión obsoleta.
- Pago parcial, mixto, cambio, saldo a favor, devolución y concurrencia del último saldo.
- Caja cerrada, apertura, egreso, retiro, diferencia, cierre concurrente y reapertura.
- Estado de habitación, folio, cartera, caja y asiento concilian al finalizar.
- Recepción, Caja y gerente ejecutan el recorrido sin ayuda del desarrollador.

## P05. Compras, retenciones e inventario

### Compras y proveedores

- Completar documento fiscal de proveedor, CAI, número normalizado, fechas y líneas.
- Crear unicidad concurrente por proveedor/documento.
- Clasificar gasto, inventario, activo e ISV acreditable/no acreditable.
- Implementar cuentas por pagar, abonos, pagos, notas del proveedor y reversos.
- Incorporar retenciones solo después de aprobar base, tasa, momento, constancia y cuentas.
- Crear pantallas completas de proveedores, compras, pagos y retenciones.

### Inventario

- Sustituir edición de existencia por kardex de entrada, salida, ajuste y reverso.
- Aplicar el método de valoración aprobado y preservar costo histórico.
- Integrar recepción de compra, consumo de folio y costo de venta.
- Bloquear stock negativo y serializar salidas concurrentes.
- Exigir motivo, permiso y asiento para ajustes físicos.

### Salida G5

- Compra, proveedor, retención, banco y contabilidad concilian.
- Kardex, existencia y cuenta de inventario concilian por producto y período.
- Dos compras iguales o dos salidas concurrentes no duplican ni dejan stock negativo.
- Casos COM01–COM07, RET01–RET05 e INV01–INV06 aprobados.

## P06. Reportes, declaraciones, auditoría y representación

### Reportes

- Reconstruir libro de ventas desde naturaleza y estado del documento, no desde cobros.
- Crear auxiliares de ISV 15 %, ISV 18 %, turismo, compras, retenciones, cartera, proveedores y caja.
- Implementar exportaciones versionadas con totales de control y período.
- Sustituir reportes declarativos provisionales por obligaciones confirmadas por el contador.
- Conservar estados preparado, revisado, presentado y pagado con evidencia real.
- Separar ventas, cobros, caja y depósitos en dashboard y reportes.

### Auditoría y documentos

- Serializar cadena de bitácora y verificarla tras respaldo/restauración.
- Crear exportación y ancla custodiada fuera de la base.
- Generar PDF/copia conservable e impresión desde un modelo común basado en snapshot.
- Crear trabajos de documento/impresión con estado, intentos, error y usuario.
- Mantener plantillas por Factura, Nota de Crédito y Nota de Débito.

### Salida G6

- Documento, libro, asiento, auxiliar y reporte aplicable concilian.
- Los reportes conservan los mismos totales al exportar PDF/Excel/CSV.
- La cadena de auditoría verifica antes y después de restaurar.
- Contador aprueba muestras y formatos; ninguna presentación externa se marca sin acuse real.
