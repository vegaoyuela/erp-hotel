# P02-K — Revocación y escrituras operativas

**Fecha:** 30 de octubre de 2026. **Base de código:** `889230c` (P02-J). **Estado:** alcance implementado y aprobado por QA automática; G2 y aceptación independiente pendientes.

## Problema y comportamiento corregido

La autenticación HTTP validaba al usuario antes de entrar al caso de uso. Una solicitud podía superar ese control y esperar después un bloqueo de caja, producto, reserva o correlativo. Si un administrador revocaba sus permisos durante la espera, la operación podía continuar con la identidad anterior.

Los nueve servicios de esta entrega adquieren bloqueos compartidos del catálogo de seguridad y del usuario, consultan su estado y permisos vigentes y conservan los bloqueos hasta finalizar la misma transacción de negocio. No se abre una conexión adicional para la protección.

- Si el cambio de seguridad obtiene primero su bloqueo, la operación espera y vuelve a consultar al usuario: una versión revocada recibe 401; una sesión vigente sin el permiso requerido recibe 403.
- Si la operación obtiene primero la admisión, termina su transacción antes de que la revocación o el cierre de su sesión puedan confirmarse. La administración puede esperar; la respuesta exitosa del cambio de seguridad establece el límite a partir del cual la operación anterior ya no continúa escribiendo.
- Cancelar la solicitud durante la admisión libera la transacción y sus bloqueos. La revocación que esperaba puede continuar.

Una operación ya admitida no se revierte parcialmente por un cambio posterior de rol. Las validaciones, bloqueos propios, asientos, correlativos, auditoría e idempotencia conservan la transacción exterior. Incluso un reintento idempotente debe superar la autorización vigente.

## Lista de implementación

- [x] Introducir `OperationalMutationGuard`, con aislamiento `ReadCommitted`, comprobación de actividad/baja, versión de seguridad, cambio obligatorio de contraseña y permiso efectivo.
- [x] Reutilizar las claves de bloqueo de administración/sesiones de P02-J.
- [x] Pasar `SecurityActor` (ID y versión) desde los nueve controladores, sin aceptar la identidad desde el cuerpo HTTP.
- [x] Admitir cada uno de los 27 métodos antes de leer o preparar cambios de negocio.
- [x] Reemplazar su apertura de transacción por la transacción protegida; conservar confirmación y rollback en el servicio.
- [x] Traducir las denegaciones a errores seguros, con `Cache-Control: no-store` y `ProblemDetails` normalizado.
- [x] Registrar las denegaciones 403 del servicio usando verbo y plantilla de ruta; excluir query, cuerpo y credenciales.
- [x] Hacer que la normalización de errores se ejecute también sobre resultados procedentes del filtro de excepciones.
- [x] Añadir comprobación de arquitectura en los nueve servicios y pruebas PostgreSQL de espera, revocación, autorización vigente y cancelación.
- [x] Aprobar QA completa y conservar el resultado final.
- [x] Registrar modelo EF, base original preservada, inventario de alcance y hashes.

## Alcance exacto

| Servicio | Métodos | Permiso requerido |
|---|---:|---|
| Facturas | 4 | `create_invoices` para borrador/emisión; `adjust_invoices` para notas |
| Pagos | 1 | `manage_cash` |
| Reembolsos | 1 | `manage_cash` |
| Liquidaciones de tarjeta | 1 | `manage_accounting` |
| Reservas, check-in y check-out | 6 | `manage_reservations` |
| Cargos de folio | 1 | `create_invoices` |
| Cajas: alta, apertura y cierre | 3 | `manage_cash` |
| Compras: alta, pago y eliminación | 3 | `manage_accounting` |
| Categorías, productos y movimientos | 7 | `manage_inventory` |
| **Total** | **27** | **9 servicios** |

El [inventario](../evidencia/p02k-alcance.json) enumera los métodos y el trabajo residual. Las políticas HTTP siguen comprobándose antes de entrar al controlador. Las lecturas y otras escrituras no reciben automáticamente esta garantía por compartir un controlador o un permiso.

Orden de adquisición: catálogo de seguridad → usuario → recursos del caso de uso → cadena de auditoría. Las operaciones ordinarias comparten catálogo/usuario entre sí; la administración toma catálogo exclusivo y las mutaciones de sesión toman usuario exclusivo. No se cambia el esquema de base de datos ni se incorporan dependencias.

## Pruebas y significado de la evidencia

`OperationalMutations_RevalidateAllMethodsAfterWaitingForRevocation` retiene una edición de rol en PostgreSQL y observa 30 esperas de bloqueo: 27 llamadas directas a los servicios y tres solicitudes HTTP (categoría, caja y confirmación de reserva). Al confirmar la revocación exige 401. Después exige 403 a los 27 métodos con una versión vigente y sin sus permisos. Compara un hash del estado de las tablas de negocio antes/después, incluidos documentos, correlativos fiscales, pagos e inventario; excluye tablas de seguridad, auditoría, historial y el registro auxiliar de locks.

Las 54 denegaciones directas usan entradas mínimas deliberadamente inválidas: comprueban que la protección sucede antes de acceder a ellas. No equivalen a 54 recorridos HTTP completos de operación válida. Una cuarta solicitud HTTP retira un permiso sin incrementar la versión en una preparación sintética: demuestra que también se consulta el permiso vigente y que su evento 403 no guarda parámetros de la URL.

`OperationalMutations_FinishBeforeRevocationAndReleaseOnCancellation` bloquea un producto y admite un movimiento de inventario. Comprueba que la edición del rol espera hasta que finaliza el movimiento; repite el orden con logout del propio usuario. Finalmente cancela una admisión mientras la revocación espera. Solo se conservan dos entradas: stock inicial 10, final 12, cero movimientos de la operación cancelada y cadena de auditoría válida.

La regresión completa conserva los flujos fiscales, financieros, hoteleros, administrativos, de mínimo privilegio, idempotencia y rollback de entregas anteriores. Los datos son sintéticos y cada integración crea y elimina su propia base temporal.

## Resultado de QA

- 147 pruebas unitarias, 10 integraciones PostgreSQL y 5 validaciones del expediente: aprobadas, sin omisiones.
- Compilación backend sin advertencias, lint y compilación frontend de producción aprobados. Lint conserva una advertencia previa en `App.tsx:92` sobre la dependencia `sessionKey` de `useMemo` (también presente en P02-J); cero errores. Revisar en P07 preservando la separación de caché por identidad/permisos.
- Dependencias frontend y herramientas de contrato: cero vulnerabilidades informadas por npm audit en esta ejecución.
- Contrato OpenAPI real, snapshot y tipos sincronizados: 185 operaciones, cero éxitos 200/201 sin esquema y ocho respuestas binarias; sin cambio público respecto de P02-J.
- EF no requiere migración. Base QA original: 42 tablas, 345 filas y 11 migraciones; cero bases temporales al cierre.

| Evidencia | Contenido |
|---|---|
| [QA completa](../evidencia/p02k-qa.log) | Comando `bash scripts/qa.sh`, integración incluida |
| [Prueba selectiva](../evidencia/p02k-selectiva.log) | Dos integraciones nuevas aprobadas antes de la regresión |
| [Revocación](../evidencia/p02k-revocacion.json) | 27 rechazos 401, 27 rechazos 403, tres HTTP en espera y un 403 HTTP adicional |
| [Orden y cancelación](../evidencia/p02k-orden.json) | Revocación/logout esperan; cancelación sin movimiento parcial |
| [Alcance](../evidencia/p02k-alcance.json) | Métodos cubiertos y acciones pendientes |
| [Modelo](../evidencia/p02k-modelo-ef.log) | Sin cambios pendientes |
| [Base original](../evidencia/p02k-base-qa.json) | Conteos y retiro de temporales |
| [Resumen y hashes](../evidencia/p02k-qa-resumen.json) | Resultado, límites y huellas de fuentes/evidencia |

Durante la preparación se corrigieron una expectativa de test (el movimiento devuelve 200 según su contrato existente) y la ausencia de `traceId`/`code` cuando el resultado provenía de un filtro de excepciones. También se corrigió un error de sintaxis del helper de hash antes de ejecutar las pruebas. La QA final corresponde al código corregido; no se modificó el contrato para acomodar el test.

No se ejecutó navegador porque no se alteró la UI. La carga sostenida, p95 de espera administrativa y Windows quedan pendientes; observar 30 bloqueos en una prueba funcional no es una medición de capacidad del equipo del hotel.

## Uso diario

El empleado al que se le revoca la sesión deberá volver a iniciar sesión. Si su nueva sesión carece del permiso, deberá solicitar al responsable del hotel que revise el acceso. Una edición administrativa puede esperar brevemente a que termine una transacción admitida; no se debe interpretar la espera como una autorización ya confirmada. Ante una respuesta incierta de cobro/emisión, consultar el documento y seguir el mecanismo de reintento existente antes de repetir la operación.

No hay cambios visuales ni cambios en cuerpos, rutas o DTO públicos. Se valida la compatibilidad de OpenAPI con una API real en la QA final.

## Trabajo restante y siguiente bloque

1. Extender la coordinación a las **34 mutaciones de once controladores** todavía directos: clientes, huéspedes, habitaciones, tipos, descuentos, CAI, autorizaciones, impuestos, configuración/perfil fiscal, cuentas/asientos manuales y proveedores. Mantienen sus políticas HTTP, pero no se declara resuelta su carrera durante una revocación. Extraer casos de uso, comprobar transacción/auditoría y probar carreras antes de cerrar SEC06.
2. Revisar aparte respaldo manual e impresión (cuatro acciones con efectos externos): la generación de archivos, el registro de intentos y la impresión física requieren un protocolo de admisión y resultado durable; no quedan protegidos por envolver solamente una llamada SQL.
3. Completar los demás casos formales API/DAT/SEC, variantes HTTP con datos válidos y E2E por rol. P02-09 permanece parcial.
4. Mantener abiertos P02-04 (límites/volumen y clientes heredados), P02-06 (aprobaciones segregadas y motivos sensibles), P02-07 (cuentas/secretos definitivos) y P02-10 (revisión independiente). Los montos/reglas de aprobación requieren decisiones del hotel.
5. Retomar Windows 11, Docker Desktop, QA PowerShell e impresión física cuando el usuario lo indique. Se conserva el recordatorio diario existente a las 10:00 de Honduras, sin duplicarlo.

G0/G1 requieren las fuentes oficiales y decisiones reales; G2 sigue abierta. Esta entrega no constituye aceptación productiva ni aprobación fiscal/SAR. El defecto previamente registrado de desglose/redondeo del detalle de folio sigue asignado a P03.
