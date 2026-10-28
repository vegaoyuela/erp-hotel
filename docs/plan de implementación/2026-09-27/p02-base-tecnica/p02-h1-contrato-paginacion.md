# P02-H1 — Contrato API y listados paginados

**Fecha:** 28 de octubre de 2026. **Estado:** alcance H1 implementado y verificado; P02 continúa abierta.

## Cambios para el operador

- Huéspedes: búsqueda por nombre completo, documento, teléfono o empresa; páginas, tamaño y total calculados en PostgreSQL. Al cambiar el filtro se vuelve a la primera página.
- Facturas: DNI/RTN, fechas, autorización, documento original y tipo se combinan. El botón **Filtrar** aplica los campos; el último día incluye sus horas completas. Los cambios posteriores a una operación actualizan la consulta.
- Auditoría: búsqueda, filtro de acción, total y páginas consultan todos los eventos autorizados. La lista de acciones se obtiene de toda la bitácora. La ruta nueva supera el límite histórico de 200 eventos.
- Las tablas conservan desplazamiento horizontal en móvil; el paginador identifica la página activa, anuncia los conteos y bloquea cambios mientras carga.

## Contrato implementado

| Ruta | Parámetros adicionales | Orden estable |
|---|---|---|
| `GET /api/Guests/paged` | `search` | Apellido, nombre e ID |
| `GET /api/Invoices/paged` | `dni`, `start`, `end`, `caiId`, `documentAuthorizationId`, `originalInvoiceId`, `documentType` | Fecha e ID descendentes |
| `GET /api/audit-logs/paged` | `search`, `action` | Timestamp e ID descendentes |
| `GET /api/audit-logs/actions` | Sin paginación; catálogo de acciones distintas | Acción |

Todas las rutas mantienen sus permisos. Los listados antiguos permanecen disponibles para los consumidores pendientes de migración. El OpenAPI conserva las mayúsculas del nombre de controlador; el enrutamiento HTTP de ASP.NET acepta también minúsculas.

`page` empieza en 1; `pageSize` admite 1–100 y su valor predeterminado es 25. La respuesta contiene `items`, `page`, `pageSize`, `totalItems` y `totalPages`. Una página superior a la última se ajusta a la última; sin resultados se devuelve página 1 y cero elementos. Valores fuera de rango devuelven 400 normalizado. Las consultas usan cancelación y `AsNoTracking`; facturas pagina primero y carga las relaciones únicamente de los documentos de esa página.

Las fechas de filtro usan `yyyy-MM-dd`, inicio incluido y final incluido mediante el límite exclusivo del día siguiente. Se rechaza un final anterior al inicio. Los timestamps siguen las convenciones UTC/Honduras existentes. Los enums JSON se emiten como texto y rechazan valores numéricos; el tipo documental admite `Factura`, `NotaCredito` y `NotaDebito`.

El transformador OpenAPI declara identificadores de operación, JWT bearer, 401/403 según los atributos reales y errores `application/problem+json` con código y `traceId`. Normaliza los errores declarados y documenta 400/500. La descripción de todas las respuestas de éxito aún no está completada: H2 debe reemplazar respuestas anónimas/sin DTO explícito.

## Tipos y herramientas

El [snapshot OpenAPI](../contratos/openapi-v1.json) genera `frontend/src/types/api.generated.ts`. `types/api.ts` deriva paginación, huéspedes, facturas y auditoría del contrato; `types/index.ts` reutiliza los esquemas generados de huéspedes/facturas/líneas. El adaptador `ApiResponse` representa los números emitidos en JSON y las propiedades presentes, incluidas las nulas; ASP.NET admite además cadenas numéricas al recibir solicitudes. Los IDs de líneas en borradores siguen siendo opcionales.

El generador tiene paquete y lock propios en `scripts/api-contract`: `openapi-typescript` 7.13.0 y TypeScript 5.9.3. Esto respeta su dependencia par sin cambiar TypeScript 6 del frontend ni forzar instalaciones incompatibles. El formato canónico de claves hace reproducible el snapshot.

Desde la raíz:

```bash
npm --prefix scripts/api-contract ci
npm --prefix frontend run api:check
# Solo después de obtener/revisar un nuevo contrato:
node scripts/api-contract/generate.mjs --live-schema /ruta/temporal/openapi.json
```

La [guía del contrato](../contratos/README.md) explica cómo obtenerlo mediante integración. `scripts/qa.sh` y `scripts/qa.ps1` instalan y auditan ambas dependencias, obtienen el OpenAPI de integración y rechazan diferencias de snapshot/tipos. Sin conexión de integración validan únicamente snapshot contra tipos; esto no prueba correspondencia con una API ejecutándose.

## QA y evidencia

| Verificación | Resultado |
|---|---|
| Build .NET y formato | Aprobados; 0 advertencias y 0 errores |
| Unitarias / integración PostgreSQL / expediente fiscal | 109 / 5 / 5 aprobadas |
| Contrato y tipos | Snapshot igual al OpenAPI real; tipos sincronizados |
| Prueba negativa | Una alteración deliberada del contrato fue rechazada |
| Dependencias | 0 vulnerabilidades informadas en frontend y generador |
| Frontend | Lint y build aprobados, incluidos los últimos ajustes de tablas |
| Navegador real | Paginación, filtro de empresa desde otra página, vacío/restablecimiento, bitácora y filtro fiscal vacío; 0 errores JavaScript |
| Escritorio / móvil | 6 capturas; sin desbordamiento horizontal del documento en móvil; las tablas desplazan su contenido |
| EF Core | No requiere migración |
| Base QA al cerrar | 42 tablas, 345 filas, 11 migraciones, 0 bases temporales residuales |

Con 37 huéspedes sintéticos, la respuesta anterior ocupó 22.682 bytes y una página de 10 ocupó 6.195 bytes: una reducción aproximada del 73 %. No representa una medición de latencia ni una estimación para la base oficial. La integración comprobó además el conteo completo de 255 eventos de auditoría y una página de un evento.

Las pruebas de facturas comparan el total de una consulta combinada con PostgreSQL y rechazan fechas invertidas. Las de huéspedes comprueban orden estable con nombres repetidos, ausencia de solapamiento entre páginas, ajuste de página final, vacío y validación de límites. La base de UI fue independiente y sintética; se retiraron sus servidores, base y secretos temporales.

- [QA integral](../evidencia/p02h1-qa.log), [QA final frontend](../evidencia/p02h1-ui-qa.log), [modelo EF](../evidencia/p02h1-modelo-ef.log).
- [Métricas de paginación](../evidencia/p02h1-paginacion.json), [rechazo de divergencia](../evidencia/p02h1-contrato-negativo.json), [base conservada](../evidencia/p02h1-base-qa.json).
- [Resultado de navegador](../evidencia/ui-p02h1/resultado.json), [resumen y hashes](../evidencia/p02h1-qa-resumen.json).

### Capturas reales con datos sintéticos

| Módulo | Escritorio | Móvil |
|---|---|---|
| Huéspedes | [Captura](../evidencia/ui-p02h1/huespedes-escritorio.png) | [Captura](../evidencia/ui-p02h1/huespedes-movil.png) |
| Auditoría | [Captura](../evidencia/ui-p02h1/auditoria-escritorio.png) | [Captura](../evidencia/ui-p02h1/auditoria-movil.png) |
| Facturas | [Captura](../evidencia/ui-p02h1/facturas-escritorio.png) | [Captura](../evidencia/ui-p02h1/facturas-movil.png) |

## Próximo bloque P02-H2

- [ ] Inventariar las colecciones que aún cargan todos los registros y priorizar reservas, folios, clientes, inventario y movimientos financieros.
- [ ] Migrar sus listados a consultas paginadas con filtros SQL, orden estable y permisos existentes, sin romper consumidores auxiliares.
- [ ] Declarar DTOs y respuestas de éxito de acciones aún anónimas/sin esquema; documentar sus errores específicos.
- [ ] Sustituir progresivamente los tipos manuales restantes y regenerar/revisar el contrato.
- [ ] Ampliar pruebas de filtros, páginas, permisos y estados de carga/error; ejecutar QA integral y verificar pantallas afectadas.
- [ ] Registrar los casos de la matriz formal y conservar evidencia antes del cierre independiente de G2.

PowerShell no está instalado en este entorno Linux: el script actualizado debe ejecutarse en Windows 11. Continúan pendientes la impresión física, aceptación fiscal del hotel, decisiones G0/G1, seguridad restante y revisión independiente. Esta entrega no aprueba la instalación productiva.
