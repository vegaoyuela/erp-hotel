# 02 — Calidad, frontend y pruebas

## P07. Capa de cliente y contratos

### Implementación

- Generar un cliente TypeScript desde OpenAPI o añadir pruebas automáticas que comparen rutas, métodos, cuerpos y respuestas.
- Centralizar tipos de factura, nota, folio, compra, huésped, proveedor, pago, respaldo y error.
- Crear adaptadores API por módulo; las páginas no deben construir rutas o interpretar errores de forma distinta.
- Implementar cancelación, timeout y descarte de respuestas obsoletas para búsquedas.
- Aplicar reintentos solo a lecturas seguras o mutaciones con clave idempotente estable.
- Descargar adjuntos mediante Axios autenticado y revocar toda URL temporal.

### Criterio de aceptación

- Un cambio incompatible en OpenAPI falla en CI.
- Ningún control visible llama una ruta inexistente o envía un cuerpo incompleto.
- Las fechas civiles no cambian por la zona horaria del navegador.
- Los formularios conservan la intención y sus datos ante un error recuperable.

## Estados, prevención de errores y accesibilidad

Cada pantalla debe distinguir:

- carga inicial;
- lista vacía;
- búsqueda sin resultados;
- falta de permiso;
- error recuperable;
- error que requiere soporte;
- éxito confirmado por el servidor.

### Trabajo

- Asociar `label`, descripción y error a cada campo.
- Enviar el foco al primer error y anunciar cambios importantes con regiones accesibles.
- Encerrar el foco en diálogos, permitir Escape cuando no cause pérdida y devolverlo al control de origen.
- Completar navegación por teclado, orden lógico y tamaño mínimo de objetivos táctiles.
- Probar 390 × 844, escritorio y zoom 200 % en todas las pantallas críticas.
- Evitar depender solo del color; corregir contraste de texto, estados y acciones.
- Respetar `prefers-reduced-motion` y alojar recursos esenciales localmente.
- Añadir `ErrorBoundary` y una acción clara de recuperación.
- Revisar copias de confirmación para acciones monetarias o destructivas.

## Rendimiento y base de datos

- Paginar, filtrar y ordenar en servidor los listados de crecimiento ilimitado.
- Proyectar las listas sin materializar colecciones hijas completas.
- Separar lista y detalle por ID.
- Revisar consultas con `EXPLAIN (ANALYZE, BUFFERS)` sobre un volumen representativo.
- Eliminar N+1 y productos cartesianos; usar consultas divididas solo con justificación.
- Añadir índices derivados de filtros medidos, no por intuición.
- Medir p50/p95, tamaño de respuesta, consultas, memoria y locks en el equipo Windows objetivo.
- Definir umbrales antes de medir:
  - operaciones normales: p95 acordado por hotel;
  - búsquedas y reportes: tiempo máximo y cancelación visibles;
  - arranque: readiness únicamente después de validar esquema y dependencias.

## Estrategia de pruebas

### Pirámide mínima

| Nivel | Cobertura mínima |
|---|---|
| Unitarias backend | cálculo, redondeo, estados, permisos, correlativos, períodos y mapeo contable |
| Integración PostgreSQL | migraciones, constraints, transacciones, idempotencia, concurrencia, permisos y restauración |
| Componentes frontend | formularios, errores, permisos, totales, doble envío y accesibilidad básica |
| E2E Playwright | reserva, check-in, factura, pagos, checkout, notas, caja, compras, inventario, reportes y usuarios |
| Contrato | OpenAPI ↔ TypeScript, estados HTTP, `ProblemDetails`, fechas y enums |
| Seguridad | autorización, sesión, CSRF, CORS, rate limit, adjuntos, secretos y privilegios DB |
| Rendimiento | datos voluminosos, reportes, concurrencia fiscal y bloqueo de stock/caja |
| Recuperación | respaldo completo, restauración, integridad y reanudación |

### Automatización requerida

- Ampliar `scripts/qa.sh` y `scripts/qa.ps1` para ejecutar backend, frontend unitario, E2E seleccionados y validación de Compose.
- Crear configuración de Vitest + Testing Library para frontend.
- Crear proyecto Playwright con datos efímeros y roles Admin, Recepción, Caja y Contador.
- Añadir semillas sintéticas deterministas; nunca copiar información de huéspedes reales.
- Generar reportes JUnit, cobertura y capturas únicamente ante fallo.
- Prohibir pruebas omitidas sin incidencia y fecha de resolución.

### Casos de regresión obligatorios

1. primera y última factura de rango;
2. dos emisiones simultáneas con un correlativo restante;
3. reintento después de respuesta perdida;
4. fallo inyectado después de cada escritura financiera;
5. crédito antes y después del cobro;
6. reembolso y cierre de caja concurrentes;
7. reserva solapada y cambio de habitación;
8. anticipo aplicado una sola vez;
9. compra repetida y salida concurrente de inventario;
10. período cerrado mientras otra operación intenta contabilizar;
11. sesión revocada y acceso con rol insuficiente;
12. respaldo/restauración con adjuntos y verificación de bitácora.

## Pruebas manuales por rol

### Recepción

- búsqueda/alta de huésped;
- reservación, confirmación, check-in y traslado;
- cargos, folio, factura, pago y checkout;
- error de red sin perder datos ni duplicar operaciones.

### Caja

- apertura, cobro parcial/mixto, cambio, egreso y retiro;
- saldo a favor, reembolso, tarjeta y liquidación;
- arqueo con diferencia, motivo y cierre.

### Contador

- autorizaciones, perfil, notas, asientos y períodos;
- libros, auxiliares, exportaciones y conciliación;
- compras, retenciones, inventario y cadena de auditoría.

### Administrador

- usuarios, roles, cambio obligatorio, baja y revocación;
- configuración, impresoras, respaldo y restauración;
- revisión de logs, salud, versión y soporte.

## Puerta G7

- Frontend cuenta con pruebas unitarias y E2E en los flujos críticos.
- Los cuatro roles completan su recorrido sin controles inertes ni permisos inconsistentes.
- No se pierde información ante fallos simulados de red/servidor.
- Accesibilidad de teclado, móvil y zoom 200 % aprobada.
- Objetivos de rendimiento medidos en el hardware Windows objetivo.
- `scripts/qa.ps1` termina en código cero desde una copia limpia.
