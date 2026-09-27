# 03 — Contenedores y operación en Windows 11

## Decisión de arquitectura

Se utilizarán **contenedores Linux sobre Docker Desktop con WSL 2**. Esta opción permite ejecutar PostgreSQL, ASP.NET y el gateway con las mismas imágenes probadas en desarrollo.

La impresión directa no permanecerá dentro de la API Linux. Se creará un **agente nativo de Windows** instalado como servicio, porque `RawPrinterHelper` depende del spooler de Windows y el controlador actual devuelve `501` fuera de Windows.

## P08. Preparación del equipo

### Requisitos que debe comprobar el instalador

- Windows 11 de 64 bits en una edición y versión soportadas por Docker Desktop.
- WSL 2 actualizado; virtualización habilitada en BIOS/UEFI.
- Al menos 8 GB de RAM; para operación y construcción se debe medir y documentar una reserva mayor si el equipo la necesita.
- Docker Desktop configurado para Linux containers con backend WSL 2.
- Hora de Windows sincronizada y zona `America/Tegucigalpa` aplicada a la aplicación.
- Perfil de red privada; dirección estable o reserva DHCP para el servidor.
- Licencia de Docker Desktop revisada según el tamaño/ingresos de la organización.
- Cuenta administrativa separada de las cuentas diarias del hotel.

### Estructura propuesta en Windows

```text
C:\ProgramData\HotelERP\
├── compose\                 # compose y manifiesto de versión, solo administradores
├── secrets\                 # archivos secretos con ACL restringida
├── certificates\            # clave/certificado del gateway
├── attachments\             # adjuntos fiscales persistentes
├── documents\               # PDF y representaciones conservables
├── backups\                 # respaldos locales cifrados
├── restore-staging\         # restauraciones aisladas
├── logs\                    # logs con rotación
└── evidence\                # resultados de smoke y actualización
```

Los archivos de trabajo del repositorio pueden residir dentro del sistema de archivos WSL para compilación. Los datos operativos no dependerán de la carpeta del repositorio.

## Archivos de distribución que se deben crear o corregir

- `docker/compose.yaml`: base común sin secretos fijos.
- `docker/compose.production.yaml`: puertos, restart, límites, volúmenes y logs de Windows.
- `docker/.env.example`: nombres no secretos y documentación.
- `docker/secrets.example/README.md`: lista de secretos, nunca valores reales.
- `docker/backend/Dockerfile`: build reproducible, usuario no root, healthcheck y versión fija.
- `docker/frontend/Dockerfile`: `npm ci`, build reproducible y gateway final.
- `docker/gateway/nginx.conf`: HTTPS, headers, límites y proxy correcto.
- `docker/migrator/Dockerfile` o target dedicado del backend.
- `scripts/windows/Install-HotelErp.ps1`
- `scripts/windows/Update-HotelErp.ps1`
- `scripts/windows/Backup-HotelErp.ps1`
- `scripts/windows/Restore-HotelErp.ps1`
- `scripts/windows/Test-HotelErp.ps1`
- `scripts/windows/Uninstall-HotelErp.ps1`
- `backend/src/HotelErp.PrintAgent/`: servicio nativo de Windows.

## Servicios del Compose final

### `db`

- Imagen PostgreSQL 16 fijada por versión y, para una liberación, por digest.
- Volumen nombrado exclusivo para `/var/lib/postgresql/data`.
- Sin `ports`; accesible únicamente por la red interna de Compose.
- `POSTGRES_PASSWORD_FILE` desde secret.
- Healthcheck `pg_isready` con `start_period`, intervalo, timeout y reintentos.
- `stop_grace_period` suficiente para cierre limpio.
- Rotación de logs y límites medidos.

### `migrator`

- Usa únicamente la credencial propietaria del esquema.
- Ejecuta migraciones como trabajo de un solo uso.
- Finaliza antes de iniciar API mediante una ejecución explícita del instalador o condición de éxito.
- No forma parte del funcionamiento diario y no comparte su secreto con la API.
- Antes de migrar exige respaldo, manifiesto y comprobación de espacio.

### `api`

- Usa la cuenta PostgreSQL de mínimo privilegio.
- `Database__ApplyMigrationsOnStartup=false`.
- Lee secretos desde archivos montados y no los imprime.
- Monta adjuntos, documentos y logs con rutas Linux configurables.
- Healthcheck de liveness y readiness; readiness valida esquema y DB sin alterar datos.
- Ejecuta como usuario no root, con filesystem de solo lectura salvo montajes requeridos.
- No publica el puerto 8080 al host.
- Confía en cabeceras reenviadas únicamente desde la red del gateway.

### `gateway`

- Único servicio publicado: `443`; `80` solo para redirigir a HTTPS si se decide conservarlo.
- Sirve el frontend y enruta `/api` a la API manteniendo el mismo origen.
- Certificado interno confiable en todos los equipos del hotel.
- HSTS después de verificar el certificado y la recuperación.
- Headers de seguridad, tamaño máximo de carga fiscal, timeouts y protección de rutas internas.
- `/health` público mínimo; readiness detallado solo para administración.

### `backup`

Se decidirá entre un servicio separado o una tarea PowerShell del host. En ambos casos:

- utiliza una credencial de solo respaldo;
- ejecuta `pg_dump` en formato personalizado;
- incluye adjuntos, documentos, configuración no secreta y manifiesto;
- calcula SHA-256, cifra el conjunto y escribe primero en un archivo temporal;
- aplica retención sin eliminar la última copia válida;
- copia a un medio externo o ubicación fuera del equipo;
- nunca construye comandos mediante concatenación de texto no confiable.

## Secretos y configuración

Eliminar del Compose actual:

- contraseña administrativa fija incluida anteriormente en Compose;
- clave JWT fija;
- cadena PostgreSQL con superusuario;
- comandos o rutas reales de respaldo;
- cualquier CAI, RTN o dato del hotel.

Crear secretos separados:

| Secret | Consumidor |
|---|---|
| `postgres_admin_password` | inicialización controlada |
| `postgres_migrator_password` | migrator |
| `postgres_app_password` | API |
| `postgres_backup_password` | respaldo |
| `jwt_signing_key` | API |
| `bootstrap_admin_password` | instalación inicial de un solo uso |
| `print_agent_key` | API y agente de impresión |
| `backup_encryption_key` | proceso de respaldo |
| `tls_private_key` | gateway |

Los secretos vivirán como archivos con ACL para la cuenta administrativa y serán montados mediante Compose secrets. La API incorporará un proveedor de configuración que lea `/run/secrets/*` o un entrypoint que exporte únicamente al proceso final. El bootstrap se deshabilitará después de crear y cambiar la contraseña inicial.

## HTTPS, red y firewall

- Nombre recomendado: `hotel-erp.local` o nombre DNS interno aprobado.
- Certificado con SAN para el nombre elegido; instalar la CA raíz en los equipos clientes.
- Cookie refresh siempre `Secure`; CORS solo al origen HTTPS exacto.
- Regla de Windows Firewall para TCP 443 limitada a `LocalSubnet` o a las subredes/IP autorizadas.
- No abrir 5432, 8080 ni el puerto del agente a la LAN.
- El endpoint del agente se limita al host y a la red de Docker; autentica cada solicitud.
- Probar la regla normal de Windows y el firewall de Hyper-V/WSL 2.

## Agente de impresión nativo

### Diseño

- Proyecto .NET Worker ejecutado como Windows Service.
- Escucha solo en loopback o canal local; si Docker debe alcanzarlo, usar `host.docker.internal` y una regla restringida al adaptador correspondiente.
- Autenticación HMAC o certificado mutuo, nonce, timestamp y protección contra repetición.
- Lista permitida de impresoras definida localmente; la API no puede elegir un nombre arbitrario.
- Recibe un `PrintJobId` y bytes ya generados desde el snapshot fiscal.
- Guarda estado Recibido/Enviado/Fallido, huella y mensaje seguro.
- Reintentar el mismo trabajo nunca genera otra factura ni otro correlativo.
- Ejecuta con una cuenta Windows de mínimo privilegio que tenga acceso al spooler y a la impresora.

### Cambios del ERP

- Sustituir llamadas directas a `RawPrinterHelper` por `IPrinterTransport`.
- Crear `WindowsPrintAgentTransport` y `PreviewOnlyTransport`.
- Persistir `PrintJob` con documento, copia, formato, hash, intentos, usuario y fechas.
- Mantener preview/PDF disponibles si la impresora está fuera de servicio.
- Dejar `RawPrinterHelper` exclusivamente dentro del agente.

## Instalación y actualización

### Instalación limpia

1. Verificar Windows, WSL 2, Docker, disco, hora, red y licencia.
2. Crear carpetas y ACL.
3. Generar secretos y certificado; no copiarlos al repositorio.
4. Construir o cargar imágenes firmadas y registrar digest.
5. Levantar `db` y esperar healthcheck.
6. Crear roles, ejecutar migrador y aplicar privilegios.
7. Levantar API/gateway y ejecutar smoke.
8. Instalar Print Agent; probar comunicación sin impresora.
9. Crear administrador inicial, cambiar contraseña y deshabilitar bootstrap.
10. Configurar tarea de respaldo y arranque/reinicio.

### Actualización

1. Verificar espacio y salud.
2. Crear respaldo y restaurarlo en staging.
3. Descargar/construir imágenes; guardar versión anterior.
4. Detener escrituras, ejecutar migración y smoke.
5. Recrear servicios con `docker compose up -d`, no solo `restart`.
6. Verificar login, roles, DB, documentos, impresión y respaldo.
7. Abrir escritura y registrar el cambio.

### Rollback

- Antes de emitir documentos nuevos: se puede volver a imágenes y base respaldada.
- Después de emitir documentos reales: no restaurar una base antigua como si nada hubiera ocurrido. Aplicar recuperación documentada, preservar documentos emitidos y reconciliar correlativos con contador.

## Pruebas y salida G8

- `docker compose config` no revela secretos ni puertos internos.
- Construcción limpia y análisis de imágenes sin vulnerabilidades críticas conocidas.
- Healthchecks y orden db → migrator → API → gateway comprobados.
- Reinicio de cada contenedor y reinicio completo de Windows sin pérdida.
- Acceso desde un cliente autorizado por HTTPS; acceso externo bloqueado.
- PostgreSQL y API no son accesibles directamente desde la LAN.
- Rotación de JWT/DB/certificado ensayada.
- Instalación y actualización PowerShell funcionan desde una máquina limpia.
- Print Agent caído no impide conservar la factura; su recuperación imprime el mismo trabajo.
- Evidencia de versión, digest, configuración y smoke guardada sin secretos.
