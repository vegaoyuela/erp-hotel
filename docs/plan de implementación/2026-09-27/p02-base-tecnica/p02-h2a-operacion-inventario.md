# P02-H2A — Reservas, folios, clientes e inventario

**Fecha:** 29 de octubre de 2026. **Estado:** alcance H2A implementado y verificado. P02-04/P02-05 y G2 permanecen abiertos.

## Implementado

- Reservas: consulta paginada, búsqueda por huésped/documento/habitación y filtro de estado combinados en SQL. Conserva confirmación, cancelación con motivo, check-in/out, control de versión y exportación completa. Los botones de mutación respetan el permiso del operador.
- Folios: búsqueda por huésped/habitación, estado y paginación. La lista proyecta solo los datos visibles y `itemCount`; los cargos completos se consultan al abrir el detalle. El conteo excluye cargos eliminados. Conserva nombres históricos aunque el huésped haya sido dado de baja; al registrar un cargo se actualizan detalle y lista.
- Clientes: búsqueda por nombre, RTN, teléfono o correo en PostgreSQL, con páginas y total real. La búsqueda se demora 250 ms para reducir solicitudes; se reinicia la página al cambiar el filtro.
- Inventario: productos y movimientos paginados. Los filtros de producto combinan nombre/SKU/descripción/categoría, categoría e indicador activo. Kardex admite filtros API por producto y tipo. El selector de producto tiene su propia búsqueda y páginas de diez opciones; conserva la selección al cambiar la búsqueda.
- Los indicadores de inventario se calculan en SQL sobre el inventario completo. No cambian al filtrar o cambiar la página de productos. Solo se solicita el listado de la pestaña activa; categorías e indicadores mantienen consultas independientes.
- Los listados muestran carga, vacío, errores y recuperación mediante **Actualizar**. Las tablas desplazan su contenido horizontalmente en móvil sin ensanchar el documento. Se preservó la identidad visual existente con Impeccable 4.8.0.

## Rutas y contrato

Todas mantienen las políticas existentes de sus controladores. Inventario exige `manage_inventory`; las lecturas de reservas/folios/clientes conservan la autenticación anterior. La revisión de mínimo privilegio completa sigue en P02-06.

| Ruta nueva | Filtros | Orden estable |
|---|---|---|
| `GET /api/Reservations/paged` | `search`, `status` | Check-in e ID descendentes |
| `GET /api/Folios/paged` | `search`, `status` | Apertura e ID descendentes |
| `GET /api/Customers/paged` | `search` | Nombre e ID ascendentes |
| `GET /api/Inventory/products/paged` | `search`, `categoryId`, `isActive` | Nombre e ID ascendentes |
| `GET /api/Inventory/movements/paged` | `search`, `productId`, `movementType` | Fecha e ID descendentes |
| `GET /api/Inventory/summary` | Ninguno | Totales agregados |

Las cinco colecciones usan `page` desde 1, `pageSize` 1–100, valor predeterminado 25, ajuste de página superior a la última y respuesta `items/page/pageSize/totalItems/totalPages`. Los textos tienen máximo 100 caracteres; estados y tipos inválidos se rechazan con 400 normalizado. Los listados usan `AsNoTracking` y cancelación. `FolioListDto` evita enviar los cargos de todos los folios en cada página. Los endpoints anteriores siguen disponibles para consumidores pendientes de migración.

Se declararon respuestas de éxito explícitas de reservas y cargos de folio usando los resultados reales del servicio: versión, cambio de estado, check-in, check-out y cargo. La creación de reserva documenta 201; los endpoints correspondientes documentan 200/404/409. Una prueba contrasta los esquemas reales de check-in/out y cargo con OpenAPI. Declarar solo errores suprime el éxito inferido en ApiExplorer; las anotaciones incluyen ambos deliberadamente.

`frontend/src/types/index.ts` deriva ahora del contrato `Reservation`, `Customer`, `Category`, `Product`, `InventoryMovement`, `Folio`, `FolioItem`, `FolioList` e `InventorySummary`. El hook paginado admite habilitación por pestaña. No se añadieron dependencias.

## QA

- Build .NET: cero advertencias y cero errores; 109 unitarias aprobadas.
- Cinco integraciones PostgreSQL aprobadas. La matriz de roles comprueba los tres endpoints nuevos de inventario: recepción, caja y contabilidad reciben 403; administración recibe 200. Las cinco colecciones nuevas rechazan accesos anónimos.
- Cada colección se prueba con 31 registros coincidentes: página inicial, página siguiente sin IDs repetidos, repetición estable, última página ajustada y conteos. Se comprueban filtros combinados, correo, estados inválidos, GUID inválido, límite de tamaño y vacío.
- Folios históricos conservan su huésped eliminado; el listado omite `items` y cuenta solo el cargo visible. Los indicadores de inventario se contrastan con consultas independientes de PostgreSQL.
- Cinco pruebas del expediente fiscal, auditorías npm de frontend/generador, contrato real frente a snapshot, tipos generados, lint y build aprobados.
- EF Core confirma que no hace falta migración.
- Navegador real: páginas y filtros, apertura de detalle de folio, indicadores independientes de filtro, producto fuera de primera página, selección preservada, error 503 simulado y recuperación. Cero errores JavaScript; diez capturas a 1440 px y 390 px, sin desbordamiento del documento.

La integración inicial detectó una proyección de folios no traducible por EF Core; se cambió por inicializadores traducibles antes de aprobar. La revisión visual detectó una columna automática de grid que ensanchaba inventario en móvil; se corrigió con columnas limitadas y desplazamiento interno. Los datos visuales fueron sintéticos en una base separada; sus servidores, base y secretos se retiraron.

### Medición sintética de respuesta

| Colección | Filas de respuesta anterior | Filas de página nueva | Bytes anteriores | Bytes de página |
|---|---|---|---|---|
| Reservas | 33 | 10 | 12.190 | 3.760 |
| Folios | 33 | 10 | 18.611 | 2.195 |
| Clientes | 31 | 10 | 11.595 | 3.805 |
| Productos | 31 | 10 | 8.421 | 2.788 |
| Kardex | 31 | 10 | 8.935 | 2.946 |

Los cinco filtros coinciden con 31 registros nuevos. Reservas y folios anteriores incluyen además dos registros preexistentes del flujo integrado: la tabla muestra sus tamaños completos, sin atribuir toda la diferencia exclusivamente al tamaño de página. La medición es de JSON sin compresión; no mide latencia ni representa la base oficial.

### Evidencia y capturas

- [QA completa](../evidencia/p02h2a-qa.log), [modelo EF](../evidencia/p02h2a-modelo-ef.log), [base QA preservada](../evidencia/p02h2a-base-qa.json).
- [Métricas](../evidencia/p02h2a-paginacion.json), [inventario de contratos](../evidencia/p02h2a-inventario-contratos.json), [detector de diseño](../evidencia/p02h2a-impeccable.json).
- [Flujos de navegador](../evidencia/ui-p02h2a/resultado.json), [resumen y hashes](../evidencia/p02h2a-qa-resumen.json).

| Pantalla | Escritorio | Móvil |
|---|---|---|
| Reservas | [Captura](../evidencia/ui-p02h2a/reservaciones-escritorio.png) | [Captura](../evidencia/ui-p02h2a/reservaciones-movil.png) |
| Clientes | [Captura](../evidencia/ui-p02h2a/clientes-escritorio.png) | [Captura](../evidencia/ui-p02h2a/clientes-movil.png) |
| Folios | [Captura](../evidencia/ui-p02h2a/folios-escritorio.png) | [Captura](../evidencia/ui-p02h2a/folios-movil.png) |
| Productos | [Captura](../evidencia/ui-p02h2a/inventario-productos-escritorio.png) | [Captura](../evidencia/ui-p02h2a/inventario-productos-movil.png) |
| Kardex | [Captura](../evidencia/ui-p02h2a/inventario-kardex-escritorio.png) | [Captura](../evidencia/ui-p02h2a/inventario-kardex-movil.png) |

## Siguiente bloque H2B y pendientes

- [x] Paginar reservas, folios, clientes, productos y Kardex sin cargar sus colecciones completas en estas pantallas.
- [x] Sustituir sus contratos manuales y registrar las respuestas de éxito operativas indicadas.
- [x] Probar consultas, permisos, contrato y experiencia de escritorio/móvil.
- [ ] Paginar caja y sus movimientos, liquidaciones y pagos elegibles, asientos y logs de respaldo.
- [ ] Revisar los selectores de check-in/out, autorizaciones, catálogos y consumidores auxiliares que aún usan arrays. El inventario contiene 35 lecturas frontend para revisar; no todas requieren paginación, pues también incluye catálogos pequeños y reportes.
- [ ] Revisar 56 respuestas 200/201 sin esquema y los tipos manuales restantes. Una respuesta sin esquema puede ser intencionalmente vacía; el inventario requiere revisión por operación.
- [ ] Vincular casos con API01–API05, DAT01–DAT03 y SEC01–SEC11, completar seguridad y obtener revisión independiente de G2.
- [ ] En P03, retirar el recálculo monetario del detalle de folio en el navegador: su presentación previa suma el bruto al impuesto sin descontar el descuento del total. Debe usar un desglose calculado por backend, con prueba de descuento/redondeo y comparación contra `TotalAmount`. Esta entrega cambió la consulta de lista; no aprobó ese cálculo de detalle.

## Windows e impresión — aplazados por el usuario

El 29 de octubre el usuario pidió continuar el código y retomar Windows e impresión en breve. Ambos siguen pendientes: ejecución de QA PowerShell y despliegue Docker Desktop en Windows 11; prueba física de factura, original/copia y aceptación del hotel. Se creó un recordatorio diario a las 10:00, **America/Tegucigalpa**, hasta completar y documentar ambas pruebas. Identificador: `pendientes-windows-11-e-impresi-n-del-hotel`.

La aceptación oficial G0/G1, el perfil del contador y la aprobación fiscal siguen sujetos a la evidencia real del hotel. Esta entrega no cierra esas puertas.
