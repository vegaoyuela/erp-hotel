# P02-L — Catálogos y configuración protegidos

**Fecha:** 30 de octubre de 2026. **Base de código:** `a513f1a` (P02-K). **Estado:** alcance implementado y aprobado por QA automática; G2 y revisión independiente pendientes.

## Resultado buscado

Las 34 mutaciones que seguían en once controladores pasan a servicios de aplicación. Todas admiten al actor con ID y versión, vuelven a consultar sesión/permisos vigentes y conservan los bloqueos de seguridad durante su transacción. Un cambio de rol o logout que llega después de la admisión espera; si la revocación se confirma primero, la operación pendiente se rechaza con 401. Una versión vigente sin el permiso requerido recibe 403.

Además, el guardado de negocio y su auditoría se confirman juntos. Los controladores conservan las lecturas, las políticas HTTP y la traducción del resultado, incluidos los códigos, DTO y `Location` existentes. No se agregan rutas ni cambios de interfaz.

## Lista de implementación

- [x] Extraer las 34 acciones de los once controladores a servicios.
- [x] Usar `OperationalMutationGuard` antes de leer o preparar cambios de negocio, incluido el PDF.
- [x] Serializar ediciones dentro de cada catálogo con un bloqueo transaccional propio, después de los bloqueos de catálogo de seguridad/usuario.
- [x] Unir persistencia y auditoría; comprobar que el confirmador rechaza usos sin transacción exterior.
- [x] Conservar los eventos y snapshots de aprobación/retiro fiscal.
- [x] Compensar el PDF nuevo ante fallos anteriores al intento de commit y ante cancelación durante la admisión preparada para persistir.
- [x] Probar las 34 acciones HTTP, la desactivación de cuenta con historial, auditoría y 71 rollbacks.
- [x] Probar 34 métodos y 34 rutas frente a revocación y permiso vigente retirado.
- [x] Probar el orden frente a logout/revocación y dos aprobaciones de la misma versión fiscal.
- [x] Aprobar y conservar QA completa, contrato real, modelo EF y estado de la base original.
- [x] Actualizar matriz API/DAT/SEC, lista maestra, pendientes y hashes de la entrega.

## Cobertura exacta

| Servicio | Acciones | Permiso |
|---|---:|---|
| `CustomerApplicationService` | 3: alta, actualización, baja | `manage_reservations` |
| `GuestApplicationService` | 4: alta, actualización, clasificación, baja | `manage_reservations` |
| `RoomApplicationService` | 3: alta, actualización, baja | `manage_reservations` |
| `RoomTypeApplicationService` | 3: alta, actualización, baja | `manage_reservations` |
| `DiscountApplicationService` | 3: alta, actualización, baja | `manage_settings` |
| `SupplierApplicationService` | 3: alta, actualización, baja | `manage_accounting` |
| `CaiApplicationService` | 2: alta, baja | `manage_taxes` |
| `DocumentAuthorizationApplicationService` | 3: alta, baja, alta con PDF | `manage_taxes` |
| `TaxConfigurationApplicationService` | 3: alta, actualización, baja | `manage_taxes` |
| `BusinessSettingsApplicationService` | 3: edición, aprobación, retiro | `manage_settings` para edición; `manage_taxes` para aprobación/retiro |
| `AccountingCatalogApplicationService` | 4: alta/edición/desactivación de cuenta y asiento manual | `manage_accounting` |
| **Total** | **34 métodos de once servicios** | |

Sumados a P02-K, son 61 métodos operativos/de catálogos en veinte servicios con esta admisión. Administración y sesiones mantienen su coordinación de P02-J. El [inventario](../evidencia/p02l-alcance.json) identifica cada acción, permiso, servicio y bloqueo; las 34 mutaciones directas de aquel inventario quedan cubiertas por esta entrega.

Orden de bloqueos: catálogo de seguridad compartido → usuario compartido → catálogo de negocio exclusivo → auditoría. La administración obtiene catálogo de seguridad exclusivo y las mutaciones de sesión, usuario exclusivo. El bloqueo de cada catálogo evita lecturas previas a una espera entre dos ediciones del mismo grupo; no representa una solución general a todas las carreras entre catálogos y transacciones financieras de P03/P04.

## Persistencia y auditoría

`CatalogMutationCommitter` requiere una transacción exterior y guarda antes de registrar el evento, sin hacer commit por su cuenta. El servicio confirma después de ambos pasos. El callback de identidad se evalúa después del guardado, cuando el GUID generado ya existe.

Las 31 acciones ajenas a configuración usan eventos que identifican actor, operación, entidad e ID, con los nombres de propiedades marcadas por EF. No se copia el cuerpo del formulario ni valores privados de clientes, huéspedes o campos fiscales. Los tres eventos existentes de configuración conservan su metadato específico de aprobación/retiro y sus snapshots; no se sustituyen por un registro genérico.

Se preserva la respuesta anterior de borrados sin registro: un 204 que no cambió entidades no crea un evento de cambio ficticio. La autorización y el bloqueo sí se ejecutan. Las validaciones y políticas funcionales previas no se declaran completas por este refactor; siguen los casos pendientes del plan.

## Autorización con PDF

La admisión y el bloqueo del catálogo ocurren antes de crear un archivo en el almacén. El PDF se valida con el mismo almacén privado. Si fallan el guardado, la auditoría o el punto de prueba posterior al staging, se revierte la transacción y se elimina únicamente el archivo nuevo de esa operación. La cancelación de la solicitud mientras el PDF espera para persistirse también libera los bloqueos y limpia ese archivo.

Antes de iniciar el commit se prepara el DTO y se comprueba la cancelación. Una vez intentado el commit, una caída de conexión puede dejar un resultado incierto: se conserva el archivo para evitar borrar un PDF que podría estar referenciado por una transacción confirmada. Esa situación requiere reconciliación y recuperación de huérfanos en P09; SQL y el sistema de archivos no forman una transacción distribuida. No se elimina el PDF de una autorización histórica por su baja lógica.

## Evidencia y significado de las pruebas

`CatalogMutations_RevalidateAll34MethodsAndHttpActionsAfterRevocation` retiene una edición de rol y observa 68 esperas PostgreSQL: 34 servicios y 34 solicitudes HTTP. Al confirmar la revocación, exige 401 en todas. Luego prepara un token con los cuatro permisos y retira los permisos sin incrementar versión en el fixture: exige 403 a los 34 servicios y 34 rutas, con 34 denegaciones auditadas sin query/cuerpo privado. El hash de las tablas de negocio y de los archivos se conserva. Las llamadas directas usan entradas mínimas inválidas para exigir admisión antes de consumirlas; los cuerpos HTTP cumplen validación, aunque sus estados de negocio no se ejercen en esta prueba de denegación.

`CatalogMutations_All34ActionsPersistAuditAndRollbackAtEveryCheckpoint` recorre datos de negocio presentes y solicitudes válidas de las 34 rutas, más la rama de desactivación de cuenta con movimientos: **35 casos exitosos**. Cada uno falla después de persistir y después de auditar; el multipart también falla tras guardar el PDF: **71 rollbacks**. Compara estado de todas las tablas públicas de negocio, auditoría y registro auxiliar de locks, además de nombres y SHA-256 de archivos. Excluye tablas de seguridad/sesiones y el historial de migraciones. En cada éxito comprueba una sola auditoría, su actor e ID de recurso; comprueba partida doble y las dos líneas del asiento válido y prueba que una cuenta con historial se desactiva sin perderlo. Solo queda el PDF de la operación exitosa.

`CatalogMutations_OrderRevocationLogoutCancellationAndConcurrentProfileApproval` comprueba edición de configuración admitida antes de revocación, PDF admitido antes de logout y cancelación de otro PDF mientras la revocación espera. Dos aprobaciones simultáneas terminan en 200/409, con un snapshot y un evento de aprobación. La cadena de auditoría queda válida.

Las pruebas trabajan con bases y carpetas temporales sintéticas. La observación de 68 esperas es funcional: no mide capacidad sostenida, p95 ni rendimiento del equipo objetivo.

## Resultado de QA

- **170 unitarias, 13 integraciones PostgreSQL y 5 validaciones del expediente** aprobadas, sin omisiones.
- Backend compilado sin advertencias ni errores. Lint sin errores; conserva la advertencia previa de `App.tsx:92` sobre `sessionKey`, registrada para P07. Frontend de producción compilado.
- Auditorías npm de frontend y herramientas del contrato: cero vulnerabilidades informadas en esta ejecución.
- API real, OpenAPI y tipos TypeScript sincronizados: 185 operaciones, cero éxitos 200/201 sin esquema y ocho respuestas binarias; sin cambio del contrato respecto de P02-K.
- EF sin cambios pendientes: cero migraciones nuevas. Base original de QA conservada con 42 tablas, 345 filas y 11 migraciones; cero bases temporales al cierre.
- Sin cambios frontend ni recorridos de navegador nuevos. Los ensayos por rol/navegador, carga sostenida y plataforma Windows mantienen su alcance pendiente.

| Evidencia | Contenido |
|---|---|
| [QA final](../evidencia/p02l-qa.log) | Regresión completa del código final |
| [QA inicial](../evidencia/p02l-qa-inicial.log) | Prueba anterior de arquitectura esperaba auditoría en el controlador; ajustada al servicio real, sin quitar el contrato |
| [Selectiva inicial](../evidencia/p02l-selectiva-inicial.log) | Dos integraciones aprobadas y una aserción de test incorrecta sobre ID público de configuración |
| [Atomicidad selectiva](../evidencia/p02l-atomicidad-selectiva.log) | La prueba corregida valida el ID interno sin cambiar el DTO público |
| [Revocación](../evidencia/p02l-revocacion.json) | 68 rechazos directos y 68 HTTP; estado y archivos preservados |
| [Atomicidad](../evidencia/p02l-atomicidad.json) | 35 casos exitosos auditados y 71 rollbacks de base/archivos |
| [Orden](../evidencia/p02l-orden.json) | Revocación/logout, cancelación del PDF y aprobación fiscal concurrente |
| [Alcance](../evidencia/p02l-alcance.json) | 34 métodos nuevos, 61 acumulados y cuatro acciones externas restantes |
| [Modelo](../evidencia/p02l-modelo-ef.log) | Sin migración adicional |
| [Base QA](../evidencia/p02l-base-qa.json) | Conteos de la base original y ausencia de temporales |
| [Resumen y hashes](../evidencia/p02l-qa-resumen.json) | Resultado, límites y SHA-256 de las fuentes y evidencia |

Durante la preparación se corrigió la selección del evento nuevo del test usando su ID, se aisló el almacén temporal de PDF sustituyendo sus opciones por DI y se usó el ID interno del perfil en las comprobaciones de auditoría: el DTO de configuración no publica ID. Se retiraron únicamente dos PDF sintéticos del primer ensayo, después de comparar sus bytes con el contenido de prueba. La primera QA completa también identificó la expectativa de arquitectura anterior al refactor; la prueba ahora exige interfaces de auditoría en los consumidores correctos. La QA final incluye todas estas correcciones.

## Uso diario

Una edición administrativa puede esperar a que termine una operación admitida. La revocación se considera confirmada cuando responde exitosamente. El empleado con sesión revocada debe volver a ingresar; sin el permiso requerido verá acceso denegado. Dos responsables intentando aprobar la misma versión fiscal deben recargar el perfil cuando uno reciba conflicto.

Los menús y formularios no cambian. Ante una respuesta incierta al subir una autorización, consultar el registro antes de repetir la carga. Una aprobación sintética de QA no sustituye la aceptación del expediente real del hotel.

## Pendientes y siguiente bloque

1. Diseñar la admisión y resultado durable de las cuatro acciones de efectos externos: respaldo manual y tres acciones de impresión. La impresión física continúa aplazada por el usuario.
2. Completar aprobaciones segregadas por monto/medio/origen y motivos sensibles con las decisiones del hotel. P02-06 permanece parcial.
3. Completar la matriz API/DAT/SEC, sus variantes de entrada válidas e inválidas y E2E por rol. Las lecturas no adquieren estos bloqueos de revocación; su tratamiento sigue pendiente. P02-09 permanece parcial.
4. Mantener los límites/volumen y clientes heredados de P02-04, cuentas/secretos definitivos de P02-07 y revisión independiente de P02-10. P07 conserva la advertencia previa de `App.tsx:92` sin retirar el aislamiento de caché.
5. Retomar Windows/Docker Desktop/QA PowerShell e impresión física cuando corresponda. No se ejecutan instalaciones ni pruebas físicas por esta entrega.

G0/G1 requieren fuentes oficiales y decisiones del gerente/contador. G2 sigue abierta por su alcance restante y revisión independiente. P03 conserva sus validaciones fiscales/monetarias, idempotencia de asientos manuales, coordinación entre recursos y desglose de folio. No se declara liberación productiva ni aprobación SAR.
