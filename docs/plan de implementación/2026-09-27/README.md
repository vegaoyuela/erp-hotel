# Plan final de implementación del ERP Hotel Maya Central

**Fecha de preparación:** 27 de septiembre de 2026
**Alcance:** completar el ERP, adecuarlo al perfil fiscal aprobado del hotel, validarlo con PostgreSQL y desplegarlo en una computadora Windows 11 mediante Docker Desktop.
**Punto de partida:** versión funcional de QA, estimada en 66 % del alcance total; 76 pruebas unitarias, 4 integraciones PostgreSQL, compilación backend y frontend, lint y auditoría de dependencias aprobados el 26 de septiembre de 2026.

## Estado de ejecución

La implementación comenzó el 27 de septiembre de 2026. El trabajo ejecutable de P00 está completado y documentado en [la evidencia de línea base](evidencia/README.md): respaldo y restauración de la base sintética, inventario, manifiesto, limpieza del repositorio y QA completa aprobada. En P01 ya están implementados el expediente externo validable, N01–N06 automatizadas y el historial inmutable de aprobaciones fiscales. G0 permanece abierta hasta recibir la base y los adjuntos oficiales; G1 permanece abierta hasta que gerente y contador resuelvan DEC-01–DEC-15 y firmen el expediente real.

El 29 de octubre de 2026 se completó el alcance [P02-H2A](p02-base-tecnica/p02-h2a-operacion-inventario.md): paginación de operación/inventario, contratos tipados y QA con capturas. P02 sigue abierta por los pendientes que se detallan en la lista maestra. A petición del usuario, Windows 11 e impresión física se retoman próximamente; tienen recordatorio diario a las 10:00 de Honduras hasta completar ambas.

La entrega [P02-H2B](p02-base-tecnica/p02-h2b-finanzas-respaldos.md), del 29 de octubre, completa las seis consultas financieras/de respaldo previstas y verifica una liquidación sintética con cobros seleccionados entre páginas. Al cierre de H2B, H2C quedaba previsto para selectores y contratos; su resultado se registra a continuación.

La entrega [P02-H2C](p02-base-tecnica/p02-h2c-selectores-contratos.md) completa los selectores principales y los tipos compartidos derivados del contrato; todos los éxitos 200/201 tienen esquema. QA completa y ocho capturas aprobadas. P02-05 queda completada en código/QA; siguen pendientes seguridad, paginación administrativa/límites restantes, matriz formal y revisión independiente. G2 permanece abierta.

La entrega [P02-I](p02-base-tecnica/p02-i-accesos-seguridad.md) alinea consultas y navegación por permisos, separa notas fiscales, protege cuentas/roles superiores y pagina usuarios/opciones de roles. La [matriz formal API/DAT/SEC](p02-base-tecnica/p02-i-matriz-api-dat-sec.md) registra cobertura y brechas. G2 sigue abierta; al cierre de P02-I, la siguiente prioridad era administración de seguridad bajo concurrencia, atomicidad/auditoría y aprobaciones sensibles.

La entrega [P02-J](p02-base-tecnica/p02-j-administracion-atomica.md) protege la administración y sesiones con transacciones/bloqueos, conserva el último administrador y confirma auditoría con revocación. Añade 39 rollbacks y ocho carreras; la QA suma 138 unitarias, 8 integraciones y 5 validaciones del expediente. Quedan las aprobaciones sensibles y brechas formales documentadas; G2 permanece abierta.

La entrega [P02-K](p02-base-tecnica/p02-k-revocacion-operativa.md), del 30 de octubre, coordina 27 mutaciones operativas con revocación y logout, y prueba cancelación sin cambios parciales. QA: 147 unitarias, 10 integraciones y 5 validaciones del expediente. El inventario deja 34 mutaciones directas y efectos externos pendientes; G2 sigue abierta.

La entrega [P02-L](p02-base-tecnica/p02-l-catalogos-configuracion.md) cubre las 34 mutaciones de catálogos/configuración que quedaban directas. Añade auditoría atómica, compensación del PDF y pruebas de 71 fallos; son 61 métodos coordinados entre P02-K/P02-L. QA: 170 unitarias, 13 integraciones y 5 validaciones del expediente. Siguen pendientes efectos externos, lecturas y el alcance de seguridad/aceptación documentado; G2 permanece abierta.

## Resultado esperado

Al cerrar este plan debe existir una versión que:

1. ejecute la operación diaria de habitaciones, reservaciones, huéspedes, folios, check-in, check-out, facturación, pagos y caja sin duplicar datos o movimientos;
2. emita facturas y notas únicamente con un perfil fiscal y una autorización vigentes, conservando correlativo, snapshot, asiento, bitácora y representación histórica;
3. produzca libros, auxiliares y reportes conciliables con documentos, pagos y contabilidad;
4. gestione compras, proveedores, inventario, kardex y costo mediante eventos trazables;
5. funcione con roles de mínimo privilegio y sesiones seguras dentro de la red privada del hotel;
6. se instale y recupere de manera reproducible en Windows 11;
7. conserve PostgreSQL, adjuntos, configuraciones y documentos mediante respaldos verificados;
8. tenga pruebas automáticas, evidencia de aceptación y aprobación técnica, operativa y contable;
9. permita probar gradualmente la impresión de factura en Windows sin utilizar correlativos reales antes de la autorización de salida.

## Límites de la aprobación

- Completar código y pruebas no equivale a una aprobación del SAR.
- El gerente y el contador deben resolver las decisiones fiscales y contables identificadas en este plan.
- Antes del primer comprobante real se debe volver a revisar el perfil, las autorizaciones, los formatos vigentes y la documentación oficial del SAR.
- Las pruebas iniciales de factura en Windows usarán exclusivamente datos y rangos sintéticos. El cambio a datos reales será una puerta explícita.

## Arquitectura objetivo en Windows 11

```text
Equipos de la red del hotel
          │ HTTPS 443
          ▼
Windows 11 + Firewall restringido a la LAN
          │
          ├── Docker Desktop / WSL 2 / Linux containers
          │     ├── gateway HTTPS + frontend
          │     ├── API ASP.NET Core
          │     ├── migrador EF Core de un solo uso
          │     └── PostgreSQL 16, sin puerto público
          │
          ├── volúmenes persistentes
          │     ├── base PostgreSQL
          │     ├── adjuntos fiscales
          │     ├── documentos generados
          │     ├── registros
          │     └── respaldos
          │
          └── HotelERP Print Agent, servicio nativo de Windows
                └── spooler e impresora ESC/POS autorizada
```

El agente de impresión es obligatorio porque el backend actual usa APIs de Windows y responde `501` cuando se ejecuta dentro de un contenedor Linux. La API no volverá a imprimir directamente: creará un trabajo idempotente y el agente de Windows lo enviará a una impresora incluida en una lista autorizada.

## Orden obligatorio

| Fase | Objetivo | Puerta de salida |
|---|---|---|
| P00 | Congelar base, limpiar repositorio y registrar evidencia | G0: línea base reproducible |
| P01 | Resolver datos y políticas del hotel | G1: perfil fiscal/contable firmado |
| P02 | Completar contratos, seguridad y arquitectura de servicios | G2: base técnica aprobada |
| P03 | Cerrar núcleo fiscal, monetario y contable | G3: documentos y asientos conciliados |
| P04 | Cerrar operación hotelera, pagos y caja | G4: jornada hotelera conciliada |
| P05 | Implementar compras, retenciones e inventario | G5: auxiliares y kardex conciliados |
| P06 | Implementar períodos, reportes, auditoría y documentos | G6: expediente contable/fiscal consistente |
| P07 | Completar frontend, accesibilidad, rendimiento y E2E | G7: operación sin asistencia de desarrollo |
| P08 | Construir la distribución Docker para Windows 11 | G8: instalación limpia y reinicio aprobados |
| P09 | Implementar respaldo, restauración y migración real | G9: recuperación y corte ensayados |
| P10 | Ejecutar factura e impresión gradual en Windows | G10: prueba del propietario y contador |
| P11 | Aceptación integral, salida y seguimiento inicial | G11: liberación autorizada |

No se debe adelantar una fase que pueda consumir correlativos, modificar historia o habilitar operación real si su puerta anterior continúa abierta. El frontend, las pruebas y los contenedores pueden avanzar en paralelo cuando no dependan de decisiones fiscales.

## Documentos del plan

1. [01 — Alcance funcional, fiscal y contable](01-alcance-funcional-fiscal-contable.md)
2. [02 — Calidad, frontend y pruebas](02-calidad-frontend-pruebas.md)
3. [03 — Contenedores y operación en Windows 11](03-contenedores-windows-11.md)
4. [04 — Migración, respaldo, aceptación y salida](04-migracion-respaldo-salida.md)
5. [05 — Pruebas graduales de factura en Windows](05-pruebas-factura-windows.md)
6. [06 — Lista maestra de implementación](06-lista-maestra.md)
7. [07 — Fuentes y decisiones que deben revalidarse](07-fuentes-y-revalidacion.md)
8. [Registro de pruebas de factura](registro-pruebas-factura-windows.md)
9. [Registro de pendientes y bloqueos](08-registro-pendientes.md)
10. [Expediente P01 y validador N01–N06](p01-expediente/README.md)
11. [P02 — Ejecución de base técnica](p02-base-tecnica/README.md)

Este plan complementa las listas existentes de [implementación SAR](../../implementacion-sar/README.md), el [catálogo de 186 pruebas](../../plan-sar-2026-09-07/03-pruebas-y-aprobacion.md) y la [trazabilidad de 64 hallazgos](../../plan-sar-2026-09-07/04-trazabilidad.md). Si existe una diferencia, prevalece la condición más estricta hasta que se documente la decisión.

## Definición de terminado para cada tarea

Una casilla solo puede cerrarse cuando cumple todos los puntos aplicables:

- código y migración revisados;
- prueba unitaria para reglas puras;
- integración PostgreSQL para persistencia, transacción, permisos o concurrencia;
- prueba de contrato y E2E cuando participa el navegador;
- reintento, concurrencia y fallo parcial comprobados en operaciones monetarias;
- interfaz con carga, vacío, error, éxito y bloqueo de doble envío;
- bitácora y permisos verificados;
- evidencia sin datos personales ni secretos guardada en `docs`;
- manual afectado actualizado;
- aprobación del responsable indicado en la puerta.

## Responsables de aprobación

| Responsable | Decisiones y evidencia |
|---|---|
| Desarrollo | Código, migraciones, pruebas, contenedores y documentación técnica |
| QA | Catálogo de pruebas, regresión, concurrencia, fallos y evidencia |
| Gerente | Operación, usuarios, montos de aprobación, continuidad y salida |
| Contador | Perfil fiscal, impuestos, plan de cuentas, libros, cierres y muestras |
| Propietario del entorno Windows | Instalación, red, impresión gradual y evidencia física |
| Responsable de TI | Secretos, certificados, firewall, respaldo, recuperación y actualizaciones |

## Criterio de liberación

La versión se considera lista únicamente cuando G0–G11 estén aprobadas, no existan defectos críticos o altos abiertos, las pruebas aplicables del catálogo tengan evidencia, la restauración haya sido ejecutada en un equipo alterno y el propietario haya aprobado las pruebas graduales de factura en Windows. Hasta entonces debe operar como QA o en paralelo con el sistema oficial.
