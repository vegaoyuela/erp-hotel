# 07 — Fuentes y decisiones que deben revalidarse

**Fecha de consulta:** 27 de septiembre de 2026.

Este archivo identifica las fuentes empleadas para preparar el plan. No sustituye la revisión del contador ni la confirmación de obligaciones específicas del hotel.

## Fuentes oficiales del SAR

- [Reglamento del Régimen de Facturación y textos consolidados](https://www.sar.gob.hn/download-category/leyes-de-facturacion/). El SAR publica en esta categoría el Acuerdo 481-2017, sus reformas 609-2017, 725-2018, 817-2018 y el texto consolidado.
- [Ayuda de la Declaración Mensual de Compras (DMC)](https://www.sar.gob.hn/wp-content/uploads/2024/06/Ayuda-Declaracion-Mensual-de-Compras-DMC.pdf). La guía identifica, entre otros datos, clase de documento, CAI y número fiscal estructurado.
- [Requisitos y trámites del SAR](https://www.sar.gob.hn/requisitos/). Debe revisarse para trámites o autorizaciones que correspondan al perfil concreto del hotel.

### Revalidación fiscal obligatoria

- Al cerrar P01: perfil, obligaciones, modalidad, tipos de documento y soportes.
- Al cerrar P03: formatos, reglas, autorizaciones y casos monetarios.
- Al cerrar P06: libros, auxiliares, declaraciones y conservación.
- Antes de W5: autorización, rango, vigencia y representación de la factura.
- Antes de producción y al inicio de cada período fiscal: cambios normativos o administrativos.

Toda fuente normativa debe guardarse en el expediente externo con URL, título, fecha de consulta y hash del archivo descargado. No almacenar en Git documentos que contengan información fiscal privada del hotel.

## Fuentes oficiales de Docker

- [Instalar Docker Desktop en Windows](https://docs.docker.com/desktop/setup/install/windows-install/). Al consultar la fuente, Docker recomendaba instalación por usuario y WSL 2 para la mayoría de usuarios; indicaba WSL 2.1.5 o posterior, Windows 11 23H2 o posterior, 8 GB de RAM y virtualización habilitada. Estos requisitos deben revisarse de nuevo el día de la instalación.
- [Docker Desktop con WSL 2](https://docs.docker.com/desktop/features/wsl/). Explica el backend WSL 2 y su integración.
- [Buenas prácticas de WSL 2](https://docs.docker.com/desktop/features/wsl/best-practices/). Recomienda mantener WSL actualizado y considerar rendimiento/recursos.
- [Secrets en Docker Compose](https://docs.docker.com/compose/how-tos/use-secrets/). Los secretos se montan como archivos y se asignan únicamente a los servicios consumidores.
- [Orden de arranque y healthchecks](https://docs.docker.com/compose/how-tos/startup-order/). `service_healthy` permite esperar readiness de una dependencia; `service_completed_successfully` sirve para trabajos de un solo uso.
- [Uso de Compose en producción](https://docs.docker.com/compose/how-tos/production/). Recomienda un override de producción, restart policy y recreación de servicios al actualizar.
- [Respaldo y restauración de Docker Desktop](https://docs.docker.com/desktop/settings-and-maintenance/backup-and-restore/). Los volúmenes y bind mounts requieren su propio procedimiento de respaldo; el Compose permite reconstruir contenedores.
- [Volúmenes de Compose](https://docs.docker.com/reference/compose-file/volumes/). Referencia para persistencia independiente del ciclo de vida del contenedor.

## Fuentes oficiales de Microsoft

- [Administrar Windows Firewall con PowerShell](https://learn.microsoft.com/en-us/windows/security/operating-system-security/network-security/windows-firewall/configure-with-command-line). La regla del ERP debe limitar el puerto HTTPS a la red autorizada y mantener el firewall habilitado.
- [Red y firewall de WSL](https://learn.microsoft.com/en-us/windows/wsl/networking). Windows 11 aplica también firewall de Hyper-V/WSL; la prueba de acceso LAN debe cubrir ambas capas.
- [Contenedores y WSL en Windows](https://learn.microsoft.com/en-us/windows/dev-environment/docker/overview). Referencia de instalación y ejecución de contenedores Linux sobre Windows.

## Decisiones técnicas que deben cerrarse durante la implementación

| ID | Decisión | Fecha límite |
|---|---|---|
| ARQ-01 | Nombre DNS interno y método de distribución de CA/certificado | Antes de P08-06 |
| ARQ-02 | Puerto HTTPS único y subredes/IP autorizadas | Antes de P08-07 |
| ARQ-03 | Ubicación de datos Docker y capacidad mínima de disco | Antes de P08-05 |
| ARQ-04 | Destino cifrado externo de respaldo | Antes de P09-02 |
| ARQ-05 | RPO/RTO y retención | Antes de P09-01 |
| ARQ-06 | Cuenta Windows del Print Agent y modelo de impresora | Antes de P08-09 |
| ARQ-07 | Medio API ↔ Print Agent y autenticación | Diseño P08-09 |
| ARQ-08 | Arranque automático tras reinicio y necesidad de login Windows | Antes de G8 |
| ARQ-09 | Licenciamiento de Docker Desktop aplicable | Antes de instalar |
| ARQ-10 | Estrategia de imágenes: construir localmente o registro privado | Antes de P08-10 |

## Fuentes internas

- [Auditoría vigente](../../auditoria-2026-09-06/README.md)
- [Plan SAR original](../../plan-sar-2026-09-07/README.md)
- [Tablero de implementación](../../implementacion-sar/README.md)
- [Registro de ejecución](../../implementacion-sar/evidencia/registro.md)
- [Política de sesión y LAN](../../implementacion-sar/sesion-y-transporte-lan.md)
- [PostgreSQL con mínimo privilegio](../../implementacion-sar/postgresql-minimo-privilegio.md)
- [Manual de usuario](../../Manual-de-usuario-Hotel-Maya-Central.docx)
- [Tutoriales por rol](../../tutoriales-video-por-rol/README.md)
