# P02-I — Acceso por permisos y administración paginada

**Entrega:** 29 de octubre de 2026. **Base:** `703ce23`. **Estado:** implementación y QA del alcance descrito; P02-06 y G2 permanecen abiertas.

## Problemas corregidos

Varias lecturas de huéspedes, reservas, folios, facturas y configuración exigían únicamente sesión. Un usuario con un rol personalizado podía consultar información fuera de su función llamando directamente a la API. A la vez, menú, buscador y algunas rutas del frontend dependían del nombre del rol; el cliente trataba nombres como «Admin» o «Administrador» como un permiso general. Esto bloqueaba roles personalizados legítimos y mostraba acciones que el servidor podía rechazar.

Otro problema permitía a un operador con `manage_users` intentar restablecer la contraseña, modificar, desactivar o asignar roles a una cuenta cuyos permisos excedían los propios. Los gestores de roles podían intentar alterar un rol superior. La ruta heredada de registro también asignaba Recepción sin comprobar que el operador pudiera conceder sus permisos.

La corrección exige permisos efectivos tanto en consultas como en navegación, comprueba el destino de cambios de seguridad y restringe las notas fiscales mediante un permiso propio. Se mantiene la autenticación y revocación de sesiones existente.

## Políticas de lectura

Cada fila permite **cualquiera** de los permisos indicados. Las mutaciones conservan además su permiso específico: poder leer no concede permiso de edición.

| Datos | Permisos que habilitan lectura |
|---|---|
| Huéspedes y clientes | `manage_reservations`, `manage_accounting` |
| Reservas, habitaciones y tipos | `manage_reservations` |
| Folios | `manage_reservations`, `create_invoices`, `manage_cash`, `manage_accounting` |
| Facturas, vista previa e impresión del documento | `create_invoices`, `adjust_invoices`, `manage_cash`, `manage_accounting`, `manage_taxes` |
| Opciones fiscales CAI/documentales | `create_invoices`, `adjust_invoices`, `manage_taxes` |
| Descuentos de catálogo | `manage_settings`, `manage_reservations`, `create_invoices` |
| Tasas | `manage_taxes`, `create_invoices`, `manage_reservations` |
| Configuración comercial | `manage_settings`, `manage_taxes` |
| Pagos y aplicaciones | `manage_cash`, `manage_accounting` |

`HotelAccessPolicies` centraliza las lecturas compartidas. Las pruebas reflectivas rechazan cualquier GET de controlador sin política nombrada, salvo usuario actual y dashboard. El primero devuelve la sesión propia; el segundo filtra secciones según permisos: reservas/ocupación, facturas, caja, alertas fiscales e inventario se consultan y devuelven solo si corresponden. Un operador de usuarios sin permisos hoteleros recibe listas vacías, caja nula y ningún indicador hotelero. La integración comprueba esta minimización contra datos sintéticos presentes, además de comprobar que Admin sí los recibe.

## Notas fiscales y cambios de seguridad

- `adjust_invoices` controla notas de crédito y débito. Se incorpora al catálogo de permisos y se asigna a los roles de sistema Admin y Contador. Recepción conserva emisión ordinaria y cobros, pero no recibe este permiso automáticamente.
- El frontend muestra los botones de notas por ese permiso. Contador puede consultar el auxiliar de pagos, pero cobrar/reembolsar sigue requiriendo `manage_cash`.
- Modificar, restablecer contraseña, desactivar o asignar un rol a otro usuario exige que los permisos actuales del destino no excedan los del actor. Crear/asignar roles conserva la comprobación de que no concedan permisos superiores.
- Actualizar o eliminar un rol comprueba también los permisos actuales de ese rol antes de modificarlo.
- El registro heredado comprueba los permisos reales del rol de sistema Recepción. Su resolución usa `SystemKey`, sin convertir un nombre editable en identidad del sistema.
- Los rechazos de autorización autenticada se registran como `AccessDenied`. El evento conserva método, plantilla de ruta y políticas; excluye ID de URL, query string, cuerpo, cookies y credenciales. Los nuevos rechazos de administración superior se registran como `SecurityChangeDenied`, con actor, destino y nombre de operación.
- Las comprobaciones que registran un rechazo se ejecutan antes de preparar cambios de usuario. Registrar la auditoría no debe guardar accidentalmente valores de una solicitud rechazada.

No se incorpora todavía un circuito de doble aprobación ni umbrales monetarios. Tener `adjust_invoices` no representa aprobación contable por monto; ese trabajo sigue abierto y depende de las decisiones del hotel.

## Frontend y aislamiento de sesión

Barra lateral, buscador de módulos y rutas usan permisos. Un nombre de rol no concede acceso adicional. Los nombres se conservan como etiquetas de presentación. `access.ts` reúne los conjuntos de navegación compartidos; la API sigue siendo la autoridad para cada solicitud.

La caché de consultas se crea por identidad y conjunto de permisos y se limpia al reemplazarse. Esto evita conservar consultas de una identidad anterior en el cliente activo. La prueba cambia de una sesión administrativa a un rol denominado «Administrador» que solo tiene inventario: no aparecen usuarios, facturas ni caja, y un GET directo de facturas recibe 403. También se verifica que un rol personalizado de recepción tenga sus módulos aunque su nombre no coincida con los roles iniciales.

Se preservó el diseño existente. Los modales de usuario permiten desplazamiento vertical; el selector de roles conserva selecciones entre búsquedas/páginas y sus controles no envían el formulario.

## Consultas administrativas paginadas

| Ruta | Contrato |
|---|---|
| `GET /api/Users/paged` | `search`, `roleId`, `page`, `pageSize`; exige `manage_users`. Búsqueda por usuario, email y nombre; orden estable por usuario e ID. |
| `GET /api/Users/role-options/paged` | `search`, `assignableOnly`, `page`, `pageSize`; exige `manage_users`. Devuelve únicamente ID/nombre. Si `assignableOnly=true`, filtra en SQL los roles cuyos permisos exceden los del actor. |

Ambas usan el límite compartido de 1–100 registros y búsqueda de hasta 100 caracteres. La pantalla solicita 15 usuarios y el selector 10 roles. Roles de filtro y roles asignables tienen propósitos distintos: el filtro puede identificar roles existentes; el selector de alta/edición solo ofrece los concedibles. No exige `manage_roles` para administrar usuarios. La selección se realiza explícitamente, sin preasignar Recepción en el formulario.

Las rutas completas anteriores permanecen por compatibilidad. Ya no se usan para cargar usuarios/roles en esta pantalla. P02-04 sigue abierta por límites y medición de catálogos/reportes y revisión de clientes de rutas heredadas; las lecturas completas de arrays en las páginas bajan de 19 a 17.

## Evidencia y reproducción

1. Configure `HOTEL_ERP_TEST_ADMIN_CONNECTION` fuera del repositorio para un PostgreSQL de QA capaz de crear bases efímeras.
2. Ejecute `bash scripts/qa.sh`. Para guardar el detalle de la matriz nueva, defina `HOTEL_ERP_READ_ACCESS_METRICS` con una ruta de evidencia.
3. La prueba `SystemRoleMatrix_EnforcesSensitiveEndpointPolicies` ejecuta la matriz anónima de controladores, las políticas anteriores y `AssertReadAccessMatrixAsync`: 64 lecturas adicionales de cuatro roles, ocho intentos de notas, seis cambios administrativos rechazados, dashboard y páginas de 31 usuarios/31 roles.
4. Para verificar la interfaz use una base sintética: busque 31 usuarios, avance a la segunda página, seleccione dos roles en páginas/búsquedas distintas, cambie de sesión sin cerrar la aplicación, pruebe un rol personalizado y el alias de administrador, consulte una factura como Contador y simule un error/reintento del listado. No use credenciales ni correlativos oficiales.

| Verificación | Resultado del bloque |
|---|---|
| QA completa | 136 unitarias, 5 integraciones PostgreSQL, 5 validaciones del expediente; build, lint y contrato aprobados. |
| Dependencias | Auditorías npm de frontend y herramientas de contrato sin vulnerabilidades reportadas. |
| Esquema | Sin cambios pendientes de modelo EF; permiso nuevo sincronizado por el seeder, sin migración nueva. |
| API | 72 comprobaciones adicionales de lectura/notas, seis cambios superiores rechazados, paginación y filtros inválidos. |
| UI | Siete recorridos, doce capturas, cero errores JavaScript y cero mutaciones de negocio desde el navegador de QA. |
| Impeccable 4.8.0 | Doce capturas revisadas y detector sobre ocho archivos UI sin hallazgos mecánicos. |
| Datos originales | Base QA preservada y recursos temporales retirados; conteos en la evidencia. |

Los ensayos iniciales detectaron un ID de fixture tomado antes de que EF lo generara y una proyección de rol que EF no traducía a SQL. Se corrigieron antes de la ejecución final. Los logs iniciales conservan esos fallos; el resumen final identifica la ejecución aprobada. Las pruebas de la matriz se agregaron a una integración existente: el total sigue siendo cinco.

Consulte [matriz formal API/DAT/SEC](p02-i-matriz-api-dat-sec.md), [detalle de accesos](../evidencia/p02i-accesos.json), [QA final](../evidencia/p02i-qa.log), [resultado UI](../evidencia/ui-p02i/resultado.json), [base QA](../evidencia/p02i-base-qa.json) y [resumen con hashes](../evidencia/p02i-qa-resumen.json).

### Capturas

| Vista | Escritorio | Móvil |
|---|---|---|
| Usuarios en segunda página | [Captura](../evidencia/ui-p02i/usuarios-paginados-escritorio.png) | [Captura](../evidencia/ui-p02i/usuarios-paginados-movil.png) |
| Roles seleccionados | [Captura](../evidencia/ui-p02i/selector-roles-escritorio.png) | [Captura](../evidencia/ui-p02i/selector-roles-movil.png) |
| Alias de administrador limitado | [Captura](../evidencia/ui-p02i/alias-admin-sin-acceso-escritorio.png) | [Captura](../evidencia/ui-p02i/alias-admin-sin-acceso-movil.png) |
| Recepción personalizada | [Captura](../evidencia/ui-p02i/recepcion-personalizada-escritorio.png) | [Captura](../evidencia/ui-p02i/recepcion-personalizada-movil.png) |
| Factura como Contador | [Captura](../evidencia/ui-p02i/contador-factura-escritorio.png) | [Captura](../evidencia/ui-p02i/contador-factura-movil.png) |
| Gestión delegada | [Captura](../evidencia/ui-p02i/administrador-delegado-escritorio.png) | [Captura](../evidencia/ui-p02i/administrador-delegado-movil.png) |

## Anexo de uso para gerente y empleados

- **Después de actualizar:** vuelva a iniciar sesión para obtener el catálogo vigente de permisos. Revise los roles personalizados y conceda `adjust_invoices` solo a quienes deban emitir notas.
- **Recepción:** continúa facturando y cobrando; solicite al responsable autorizado las notas de crédito/débito si no dispone del permiso específico.
- **Contador:** puede emitir notas y consultar pagos; las acciones de caja no se habilitan por tener acceso de lectura contable.
- **Usuarios:** busque por nombre, usuario o correo; use el filtro de rol y las flechas. En alta/edición busque y seleccione cada rol autorizado; los elegidos permanecen visibles y pueden quitarse. Un gestor delegado no puede modificar cuentas superiores.
- **Acceso insuficiente:** solicite al administrador que revise los permisos asignados. Cambiar el nombre de un rol no habilita módulos.

## Pendientes que conserva el siguiente bloque

P02-06 sigue parcial: faltan aprobación segregada por monto/medio/origen, motivos/aprobaciones sensibles restantes y revisión de escrituras de administración bajo concurrencia (incluida protección del último administrador y atomicidad/auditoría de cambios de seguridad exitosos). La matriz formal registra exactamente qué cubren las pruebas y qué aún falta, sin equiparar regresión a aceptación final.

P02-04 conserva los límites/volumen y clientes heredados; P02-07, P02-09 y P02-10 conservan trabajo pendiente. P03 mantiene el defecto del desglose monetario del detalle de folio. G0/G1 requieren datos y decisiones reales. Windows 11, Docker Desktop, QA PowerShell e impresión física siguen aplazados por el usuario y mantienen el recordatorio diario existente a las 10:00 de Honduras. Este bloque no aprueba producción ni cumplimiento SAR.
