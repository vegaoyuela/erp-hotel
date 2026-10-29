# P02-J — Administración atómica de usuarios, roles y sesiones

**Entrega:** 29 de octubre de 2026. **Base:** `2281700`. **Alcance:** cerrar las carreras y guardados parciales de administración detectados en P02-I. P02-06 y G2 conservan sus pendientes restantes.

## Qué cambia

Las mutaciones de usuarios y roles se trasladan a `SecurityAdministrationService`. Una transacción PostgreSQL exterior incluye el cambio, sus asignaciones de roles/permisos, incremento de versión de seguridad, revocación de refresh y auditoría. Los controladores traducen el resultado HTTP. La ruta heredada de registro conserva roles/permisos efectivos en su respuesta y no emite una sesión; usa el mismo servicio; se retira la implementación interna alternativa que podía crear Recepción fuera de esa comprobación.

La versión anterior podía validar una sesión antes de esperar y usar después permisos ya revocados. También comprobaba el número de administradores sin excluir cambios simultáneos y tenía rutas de edición capaces de retirar el último rol Admin. El nuevo servicio vuelve a leer estado, versión de sesión y permisos del actor después de adquirir el bloqueo de administración.

## Orden de bloqueos y revalidación

| Operación | Protección |
|---|---|
| Alta/edición/baja de usuario; restablecimiento; asignación de rol; alta/edición/baja de rol; registro heredado | Bloqueo exclusivo transaccional del catálogo de seguridad. |
| Login, refresh, cambio de contraseña y logout | Bloqueo compartido del catálogo y bloqueo exclusivo por usuario. |
| Auditoría dentro de esas operaciones | Se adquiere después de los bloqueos anteriores, mediante la cadena existente. |
| Sincronización del catálogo inicial | Usa el bloqueo exclusivo del catálogo. |

El orden es **catálogo → usuario, cuando corresponde → auditoría**. Las sesiones de usuarios diferentes pueden avanzar en paralelo; las escrituras del mismo usuario se serializan. Una mutación administrativa espera a las sesiones ya iniciadas y bloquea nuevas escrituras de sesión mientras cambia permisos o credenciales. No se depende de locks de memoria: las pruebas observan solicitudes en espera en `pg_locks`.

El servicio administrativo exige que el actor siga activo, no eliminado, con la misma versión del token y sin cambio obligatorio de contraseña. Consulta los permisos actuales en base de datos y vuelve a validar los del usuario/rol de destino. Una solicitud administrativa cuyo token fue revocado mientras esperaba recibe 401 y no escribe. Login vuelve a leer la contraseña después de esperar; refresh consume y rota dentro de la misma transacción.

Los cambios de login/contraseña usan el seguimiento de la entidad de usuario y no marcan su grafo de roles como modificado. Una prueba compara el catálogo antes/después de login, cambio de contraseña y logout; roles, permisos y asignaciones permanecen idénticos.

`JwtService` exige una transacción para generar, consumir o revocar sesiones. Los llamadores activos coordinan los bloqueos; la prueba rechaza las tres llamadas JWT hechas directamente sin transacción y comprueba que no alteran datos.

## Último administrador y roles protegidos

Se considera administrador a un usuario activo, no eliminado, con el rol no eliminado cuyo `SystemKey` es `administrator`. Un nombre de rol o un rol personalizado con muchos permisos no sustituye esa identidad.

- Antes de desactivar o retirar Admin mediante PUT/DELETE se exige otro administrador activo. La respuesta es 409 si se perdería el último.
- Se mantiene el rechazo de desactivar la propia cuenta.
- Dos administradores que se retiran su propio rol simultáneamente no pueden dejar el sistema sin Admin: la segunda operación ve el resultado confirmado de la primera.
- Si dos administradores intentan desactivarse entre sí, la solicitud del actor que quedó inactivo se rechaza al revalidarse.
- Admin conserva todos los permisos del catálogo del sistema. No puede vaciarse su lista para conservar el nombre y perder la capacidad administrativa.
- Los roles de sistema mantienen las restricciones de renombrado y eliminación.

La edición de asignaciones aplica diferencias: conserva vínculos existentes, elimina los retirados y añade los nuevos. Seleccionar un rol ya asignado o repetirlo en el cuerpo no produce un conflicto de seguimiento EF ni duplica claves.

## Auditoría y sesiones

Se registran altas, cambios, bajas, restablecimientos y asignaciones: `CreateUser`, `UpdateUser`, `ResetUserPassword`, `DeactivateUser`, `AssignRole`, `CreateRole`, `UpdateRole`, `DeleteRole` y `Register`. Los rechazos por permiso conservan `SecurityChangeDenied`; los rechazos de reglas/validación se registran como `SecurityChangeRejected` antes de confirmar únicamente el evento.

La metadata usa IDs de rol, permisos, estados y campos cambiados; no copia contraseña, hash de contraseña, access token, refresh token ni el cuerpo completo de la solicitud. Un restablecimiento indica el cambio obligatorio, sin guardar su valor secreto en auditoría. Las validaciones preceden a la preparación de entidades modificadas para que registrar un rechazo no guarde cambios parciales.

Cambio de contraseña, logout y emisión/rotación de sesión también confirman su estado con auditoría dentro de la transacción. La cookie nueva se emite después del resultado del servicio. El replay concurrente de un refresh invalida la familia y el access recién emitido por la solicitud competidora; se conserva ese efecto aun cuando el refresh rechazado devuelve 401. Las solicitudes que necesitan volver a iniciar sesión usan un error específico; fallos internos no se convierten en mensajes de validación de contraseña.

Se retiraron helpers internos de recuperación por email sin rutas ni mecanismo de entrega activo. La recuperación disponible sigue siendo el restablecimiento administrativo protegido. No se retiró ningún endpoint público de recuperación, porque esos helpers no tenían rutas HTTP.

## Pruebas nuevas

### Operaciones y salvaguardas

`SecurityAdministration_PreservesAdministratorAndAuditsSuccessfulChanges` recorre nueve clases de mutación, cambios de usuario y rol con revocación, selección de rol existente/duplicado, registro heredado, seis rechazos para conservar Admin y roles de sistema, exclusión de credenciales en metadata y verificación de la cadena. También comprueba las tres llamadas JWT sin transacción.

### Concurrencia

`SecurityAdministration_SerializesRacesAndRevalidatesWaitingSessions` comprueba:

| Carrera | Resultado verificado |
|---|---|
| Dos retiros del propio rol Admin | Primero 200, segundo 409; queda un administrador. |
| Dos administradores que se desactivan entre sí | Primero 200, actor de la segunda solicitud 401; queda uno. |
| Retirada de permiso frente a una escritura ya autenticada | El rol se actualiza; la escritura en espera recibe 401 y no crea el objeto. |
| Asignación de rol frente a eliminación del mismo rol | Ambas operaciones se ordenan; el rol eliminado no concede permisos y se revocan sesiones afectadas. |
| Restablecimiento frente a login con contraseña anterior | Login espera y recibe 401; no recupera el hash anterior. |
| Restablecimiento frente a refresh anterior | Refresh espera y recibe 401; no sobrevive una sesión activa. |
| Dos usos simultáneos de un refresh | 200/401; el access emitido por el ganador queda revocado por detección de replay. |
| Cinco contraseñas incorrectas simultáneas | Cinco rechazos; se conservan cinco intentos y el bloqueo temporal. |

En seis carreras se observó expresamente la segunda solicitud esperando un lock PostgreSQL antes de liberar la primera. Las otras dos comprueban el resultado de solicitudes simultáneas serializadas por usuario. La prueba verifica la cadena de auditoría al terminar.

### Rollback

`SecurityAdministration_RollsBackChangesTokensAndAuditAtEveryCheckpoint` provoca **39 fallos**: trece operaciones en tres puntos (después de persistir, revocar y auditar). Las operaciones son las nueve administrativas anteriores más cambio de contraseña, logout, login y refresh.

Antes y después se compara un digest del contenido completo de `Users`, `Roles`, `Permissions`, `UserRoles`, `RolePermissions`, `RefreshTokens`, `AuditLogs` y `CorrelativeLocks`. Los valores secretos permanecen en memoria de la prueba; el expediente conserva nombres de operación, punto de fallo y confirmación de restauración. Cada intento devuelve un 500 seguro y conserva exactamente el estado anterior.

## QA y compatibilidad

Las tres nuevas integraciones amplían la suite de cinco a ocho. La QA final ejecuta **138 unitarias, 8 integraciones PostgreSQL y 5 validaciones del expediente**, además de build, límites de repositorio, contrato real, auditorías npm, lint y build frontend. El modelo EF no cambia y no requiere migración.

Se mantienen las rutas y formas JSON. Se documenta 409 en las acciones que protegen el último administrador y permisos de Admin; los errores mantienen `ProblemDetails`. Snapshot y tipos generados se regeneran desde el servidor de integración y se comparan con una API real. La interfaz no se modificó en este bloque; conserva el manejo central de errores y estados de P02-I. La ejecución previa completa pasó; después se retiraron las marcas de actualización del grafo de usuario y se añadió la comprobación del catálogo. La revisión final también restituyó explícitamente el 200 de actualización de usuario al añadir 409 y amplió la prueba de contrato: toda operación debe conservar una respuesta 2xx, salvo las dos rutas heredadas que siempre rechazan. La QA final cubre ambos ajustes.

Evidencia: [QA final](../evidencia/p02j-qa.log), [integración](../evidencia/p02j-integracion.log), [operaciones](../evidencia/p02j-operaciones.json), [concurrencia](../evidencia/p02j-concurrencia.json), [39 rollbacks](../evidencia/p02j-rollback.json), [modelo EF](../evidencia/p02j-modelo-ef.log), [base QA](../evidencia/p02j-base-qa.json) y [resumen con hashes](../evidencia/p02j-qa-resumen.json).

## Anexo de uso

- Un 409 al retirar/desactivar Admin exige conservar otro administrador activo antes de continuar.
- Si una operación esperó mientras cambiaban sus permisos y recibe 401, vuelva a iniciar sesión; su solicitud anterior no fue aplicada.
- Restablecer contraseña obliga al usuario a cambiarla e invalida sus sesiones anteriores.
- Cambiar o eliminar un rol invalida las sesiones de sus miembros. Deben iniciar sesión de nuevo para usar sus permisos vigentes.
- Ante un error o una respuesta perdida, consulte el estado del usuario/rol y la auditoría antes de reintentar. Las pruebas de fallos anteriores a la confirmación verifican que no quede un cambio administrativo parcial.

## Pendientes y siguiente bloque

La concurrencia, permanencia del último administrador y atomicidad/auditoría de estas mutaciones quedan cubiertas en código y QA. P02-06 permanece parcial por aprobaciones segregadas por monto/medio/origen y motivos/aprobaciones sensibles restantes; no se asignan umbrales financieros supuestos. La revisión independiente continúa en P02-10.

El siguiente trabajo ejecutable debe extender la comprobación de revocación a solicitudes operativas/financieras que esperan sus propios bloqueos y cerrar las demás brechas formales API/DAT/SEC independientes de Windows y decisiones del hotel. La coordinación descrita en este bloque cubre las escrituras de seguridad y sesión de la tabla; no declara cubiertas todas las combinaciones de autorización durante operaciones hoteleras/monetarias. Se mantienen P02-04 (límites de catálogos/reportes y clientes heredados), P02-07, el desglose monetario de folio de P03 y los datos/decisiones oficiales G0/G1. El rendimiento de estos bloqueos debe medirse en el equipo objetivo con el resto de P07; esta entrega verifica consistencia y regresión, sin afirmar p95 de producción.

Windows 11, Docker Desktop, QA PowerShell e impresión física continúan aplazados por el usuario, con el recordatorio diario existente a las 10:00 de Honduras. G2 y la liberación productiva/fiscal permanecen abiertas.
