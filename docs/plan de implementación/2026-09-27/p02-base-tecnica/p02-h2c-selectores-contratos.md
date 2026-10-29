# P02-H2C — Selectores paginados y contratos de respuesta

**Entrega:** 29 de octubre de 2026. **Base:** `f03ae30` (H2B). **Resultado:** implementación y QA aprobadas para el alcance de este bloque; G2 permanece abierta.

## Cambio entregado

Check-in busca huéspedes mediante páginas SQL de diez registros. Check-out consulta reservas en hospedaje por páginas y resuelve una reserva recibida por enlace directo aunque esté fuera de la primera página. Las autorizaciones de factura/notas y las cajas para cobros/reembolsos usan un selector compartido con búsqueda, navegación, vacío, carga, error y reintento. La selección permanece al cambiar de página o buscar; elegir una opción sigue siendo una acción explícita.

La pantalla de autorizaciones combina CAI y autorizaciones documentales en PostgreSQL, sin descargar ambos historiales completos. El enlace «Ver Facturas» usa la fuente correcta y conserva compatibilidad con enlaces anteriores `source=document`. Las fuentes de la API nueva son `cai` y `docauth`.

Se sustituyeron las 28 interfaces compartidas todavía utilizadas por aliases derivados del contrato generado y se retiró `InvoicePrintData`, que no tenía consumidores. Los resultados de estadísticas de huésped, check-in, vista previa e integridad de auditoría también derivan de los esquemas. Los valores nulos de configuración se inicializan conforme al contrato real. Los tipos propios de formularios y componentes siguen siendo locales.

## Rutas y reglas

| Ruta | Filtros y comportamiento |
|---|---|
| `GET /api/document-authorizations/options/paged` | `source=cai|docauth`, `documentType=Factura|NotaCredito|NotaDebito`, `activeOnly`, `search`, `page`, `pageSize`. Unión SQL; orden por vencimiento descendente, fuente e ID. |
| `GET /api/cash-registers/paged` | Añade `openOnly`; devuelve cajas activas cuyo último movimiento indica apertura. Conserva los filtros y agregados de H2B. |
| `GET /api/Guests/paged` | Consumido ahora por la búsqueda de check-in; no descarga todos los huéspedes. |
| `GET /api/Reservations/paged` | Consumido por check-out con estado `CheckIn`; selección por ID se resuelve aparte mediante la ruta de detalle. |

El tamaño general permite 1–100 registros y el selector solicita 10; autorizaciones administrativas solicita 15. La búsqueda compartida está limitada a 100 caracteres. Los filtros inválidos devuelven `ProblemDetails` 400. `activeOnly` exige estado activo y fecha de Honduras entre emisión y vencimiento, ambos incluidos. No reemplaza la validación transaccional de rango/agotamiento al emitir, que pertenece al núcleo fiscal P03.

Se conservan las políticas existentes de autorización. La revisión integral de mínimo privilegio y aprobaciones corresponde a P02-06; esta entrega no la declara cerrada.

## OpenAPI y compatibilidad

Se revisaron las 53 respuestas 200/201 sin esquema inventariadas en H2B. Las acciones que realmente no devuelven cuerpo documentan 204; las rutas heredadas que siempre rechazan cancelación directa de factura/eliminación de reserva documentan sus errores. Los mensajes, usuario actual, salud, clasificación, impresión, resumen fiscal y exportación de integridad tienen esquemas explícitos. Las altas documentan 201 con su DTO.

El contrato final tiene **cero respuestas 200/201 sin esquema**, **25 respuestas 204 explícitas**, **23 respuestas 201** y **ocho descargas binarias** con MIME correcto: respaldo, cuatro exportaciones XLSX, dos libros XLSX y adjunto PDF. El transformador OpenAPI aplica el esquema binario únicamente a métodos marcados como archivo. La integración impide volver a introducir éxitos sin esquema.

No se modificó el modelo EF ni se requiere migración. Las anotaciones describen el comportamiento HTTP existente; se conserva la forma JSON de las respuestas. El snapshot y `api.generated.ts` fueron regenerados y comparados con la API ejecutándose sobre PostgreSQL sintético.

## QA y evidencia

| Comprobación | Resultado |
|---|---|
| `bash scripts/qa.sh` | Build backend, 109 unitarias, 5 integraciones PostgreSQL, 5 validaciones del expediente, límites de repositorio, contrato real, lint y build frontend aprobados. |
| Auditorías npm de frontend y generador | Cero vulnerabilidades reportadas en esta ejecución. |
| Modelo EF | Sin diferencias respecto de la última migración. |
| Selectores HTTP | 65 autorizaciones sintéticas; 62 vigentes; 31 CAI; 16 notas de crédito; 31 cajas elegibles. Filtros, páginas distintas y rechazo de parámetros inválidos comprobados. |
| Navegador real | Seis flujos, ocho capturas, cero errores JavaScript y cero mutaciones de negocio durante las comprobaciones UI. |
| Vista móvil | Sin desbordamiento horizontal del documento en las cuatro vistas. Las tablas anchas conservan desplazamiento interno. |
| Impeccable 4.8.0 | Revisión visual de ocho capturas y detector sobre nueve archivos: cero hallazgos mecánicos. |
| Base QA original al cierre | 42 tablas, 345 filas, 11 migraciones; cero bases temporales residuales. |

La prueba del navegador recorrió búsqueda/vacío/restablecimiento, página dos, selección persistente de autorización/caja, enlace profundo de reserva fuera de la primera página, enlace de autorización a facturas, compatibilidad de fuente heredada, error HTTP simulado con reintento y navegación dentro de formularios. Se verificó que Enter en la búsqueda y los botones del paginador no envíen formularios. El paginador compacto evita solapamientos en columnas estrechas y los modales permiten desplazamiento vertical.

La primera QA detectó imports sin uso después de sustituir tipos. Se corrigieron; se conserva el log inicial y la ejecución completa final aprobada. El número de integraciones sigue siendo cinco: los casos de selectores/contratos se añadieron a la regresión HTTP existente.

Evidencia: [QA final](../evidencia/p02h2c-qa.log), [casos de selectores](../evidencia/p02h2c-selectores.json), [inventario completo](../evidencia/p02h2c-inventario-contratos.json), [resultado UI](../evidencia/ui-p02h2c/resultado.json), [base preservada](../evidencia/p02h2c-base-qa.json) y [resumen con hashes](../evidencia/p02h2c-qa-resumen.json).

### Capturas revisadas

| Vista | Escritorio | Móvil |
|---|---|---|
| Check-in | [Captura](../evidencia/ui-p02h2c/check-in-escritorio.png) | [Captura](../evidencia/ui-p02h2c/check-in-movil.png) |
| Check-out | [Captura](../evidencia/ui-p02h2c/check-out-escritorio.png) | [Captura](../evidencia/ui-p02h2c/check-out-movil.png) |
| Autorizaciones | [Captura](../evidencia/ui-p02h2c/autorizaciones-escritorio.png) | [Captura](../evidencia/ui-p02h2c/autorizaciones-movil.png) |
| Selector de nota de crédito | [Captura](../evidencia/ui-p02h2c/nota-credito-selector-escritorio.png) | [Captura](../evidencia/ui-p02h2c/nota-credito-selector-movil.png) |

## Guía breve para empleados: cambios de navegación

1. En check-in, escriba nombre o documento y pulse **Buscar**. Si no aparece el huésped en la página actual, avance con las flechas; si no hay coincidencias, revise el texto antes de crear otro registro.
2. En check-out, busque el huésped o habitación. En autorización y caja, escriba parte del nombre/CAI y elija la opción del desplegable. Puede recorrer las páginas sin perder lo elegido.
3. En autorizaciones, busque el CAI y use **Ver Facturas** para consultar sus documentos. En notas, seleccione la autorización correspondiente al tipo de nota.
4. Si aparece un error de carga, pulse **Reintentar**. Compruebe autorización, caja, motivo y monto antes de confirmar la operación.

Este anexo actualiza la navegación afectada; las capturas usan exclusivamente datos sintéticos y no constituyen una prueba de impresión física.

## Trabajo restante y orden siguiente

**P02-05 queda completada en código y QA.** Los tipos compartidos derivan del contrato y la comparación automática está activa. La aprobación técnica independiente sigue en P02-10.

**P02-04 permanece parcial.** El inventario de 29 lecturas completas de arrays baja a 19. Se clasificaron todas: usuarios/roles requieren paginación administrativa; habitaciones/tipos/descuentos/categorías/impresoras/acciones requieren medición y límites con cardinalidad oficial; árbol/selector/reporte contable requieren preservar jerarquía y totales; pagos y notas de una factura son auxiliares completos de un documento y no deben truncarse sin sustituir sus cálculos por agregados backend. Estos conjuntos no equivalen a historiales globales, pero tampoco se presume que su volumen sea ilimitado.

El siguiente bloque debe trabajar P02-06 (permisos y motivos/aprobaciones sensibles), incluir la paginación administrativa pendiente y registrar cada caso API01–API05, DAT01–DAT03 y SEC01–SEC11 en la matriz formal P02-09. P02-07 conserva separación de cuentas y rotación de secretos; su ejecución en la instalación Windows sigue pendiente. P02-10 exige una persona distinta del implementador.

P03 conserva el defecto ya registrado del desglose de folio en frontend (descuentos y redondeos); no se declara resuelto por esta entrega. G0/G1 y las decisiones reales del gerente/contador siguen abiertas. Windows 11, Docker Desktop, QA PowerShell e impresión física permanecen aplazados por el usuario, con el recordatorio diario existente a las 10:00 de Honduras. No se han aprobado uso productivo ni cumplimiento SAR.
