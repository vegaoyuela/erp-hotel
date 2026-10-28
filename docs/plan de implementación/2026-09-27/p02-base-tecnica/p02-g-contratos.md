# P02-G — Contratos de cálculo, auditoría e impresión

**Implementación:** 28 de octubre de 2026. Continuación del plan del 27 de septiembre.

## Resultado

P02-03 completa sus contratos inyectables. Los servicios financieros, fiscales y hoteleros reciben la auditoría mediante `IAuditService`; facturas, notas y cargos de folio usan `IInvoiceCalculator`; check-in usa `ITaxCalculator`. La impresión recibe un generador de documento y un transporte independientes.

| Contrato | Implementación registrada | Consumidores |
|---|---|---|
| `IInvoiceCalculator` | `InvoiceCalculationService`, por solicitud | Factura, notas de crédito/débito, cargos de folio y muestra de impresión. |
| `ITaxCalculator` | `TaxService`, por solicitud | Check-in, con las tasas cargadas explícitamente desde la configuración. |
| `IAuditService` | `AuditService`, por solicitud | Finanzas, factura/notas, reservas, folios, caja, compras, inventario, autenticación y configuración fiscal. |
| `IInvoicePrintRenderer` | `EscPosService`, por solicitud | Vista previa, factura con original/copia y regla de calibración. |
| `IPrinterTransport` | `WindowsPrinterTransport`, singleton sin estado | Enumeración, conexión y envío al servicio de impresión de Windows. |

`IHotelClock`, `ICaiCorrelativeService`, `IFiscalAuthorizationService`, `IDatabaseBackupService` e `IPostgresDumpExecutor` conservan sus registros previos. La aritmética de redondeo permanece como función pura. La auditoría conserva el guardado explícito dentro del lock de cadena y participa en la transacción exterior.

## Correcciones incluidas

- Se retiró la consulta síncrona que ejecutaba `TaxService` al construirse. Check-in ya obtiene la configuración de manera asíncrona y asigna ambas tasas antes de calcular; cada solicitud mantiene su propio calculador.
- La muestra de impresión ahora usa el mismo calculador que la emisión: tres unidades de L 100 producen subtotal L 300, ISV L 45, turismo L 12 y total L 357. Las líneas, el desglose ISV y el cambio corresponden a esos importes.
- `PrintController` envía los documentos mediante el transporte inyectado. Las llamadas al componente nativo quedan en `WindowsPrinterTransport`.

## Pruebas

La integración sustituye el calculador y la auditoría por implementaciones que fallan. En ambos casos la API responde con un error interno normalizado y PostgreSQL conserva los conteos anteriores de documentos, líneas, pagos, caja, asientos, cuentas, auditoría e idempotencia, además del correlativo. Después se ejecutan los flujos normales con las implementaciones reales.

La impresión utiliza un transporte simulado que captura los bytes generados por `EscPosService`. Se recorren enumeración, conexión, vista previa, muestra con original/copia, regla y factura emitida. Las vistas previas no envían trabajos. Los fallos del generador y del transporte producen error; una plataforma no disponible responde 501. También se comprueba el aislamiento de tasas entre dos solicitudes.

Las pruebas unitarias mantienen los casos de impuestos, exoneración, descuentos, redondeo, snapshot fiscal y notas. La prueba de arquitectura comprueba los contratos de cada consumidor y rechaza dependencias concretas de cálculo, auditoría e impresión.

La impresión física, papel, márgenes y aceptación de la factura siguen pendientes de validación en Windows 11, según el [procedimiento de pruebas del hotel](../05-pruebas-factura-windows.md). La integración usa un transporte simulado.

## Evidencia y continuación

- [QA integral](../evidencia/p02g-qa.log).
- [Modelo EF](../evidencia/p02g-modelo-ef.log).
- [Conteos de QA](../evidencia/p02g-base-qa.json).
- [Resumen con hashes](../evidencia/p02g-qa-resumen.json).

P02-H continuará con convenciones y descripción OpenAPI, paginación y validación de tipos frontend. La puerta G2 conserva los pendientes de seguridad, matriz formal y revisión técnica independiente de la [lista maestra](../06-lista-maestra.md).
