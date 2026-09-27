# Evidencia P00 — línea base reproducible

**Ejecución:** 27 de septiembre de 2026
**Rama de trabajo:** `codex/implementacion-final-2026-09-27`
**Referencia anterior a la implementación:** etiqueta `implementation-baseline-2026-09-27`, commit `3403dad5f5c9f6d3ba70b48e3b59b9c7f940c5f0`

## Resultado

La parte ejecutable de P00 quedó aprobada sobre información sintética de QA:

- el respaldo PostgreSQL 16 se restauró en una base aislada y coincidieron los conteos de sus 42 tablas y 345 filas;
- el manifiesto registra hashes SHA-256 de aplicación, pruebas, frontend, migraciones, despliegue, scripts y documentación;
- no había adjuntos operativos en las rutas inventariadas y no se copió PII a esta evidencia;
- se retiraron del control de versiones 261 artefactos generados u obsoletos: `bin`, `obj`, logs, SQLite, bundles, caché del editor y el segundo lockfile;
- Docker Compose rechaza el arranque sin los secretos requeridos y acepta una configuración con valores efímeros de validación;
- la compilación terminó con cero advertencias y cero errores; pasaron 76 pruebas unitarias y 4 pruebas de integración PostgreSQL;
- `npm audit` informó cero vulnerabilidades, y el lint y la compilación de producción del frontend fueron aprobados.

La primera ejecución limpia descubrió que las pruebas dependían de la existencia física de `wwwroot`. Se corrigió agregando un objetivo de MSBuild que crea el directorio antes de compilar; los bundles permanecen ignorados y se regeneran. El log inicial fallido se conserva junto al log exitoso para demostrar la corrección.

## Archivos de evidencia

| Archivo | Contenido |
|---|---|
| [p00-respaldo-restauracion-sintetica.json](p00-respaldo-restauracion-sintetica.json) | Origen sintético, hash del dump, versión de PostgreSQL y comparación exacta de tablas. |
| [p00-inventario.json](p00-inventario.json) | Inventario de código, configuración, semillas, adjuntos y limpieza, sin valores secretos. |
| [p00-manifiesto.json](p00-manifiesto.json) | Hashes y agregados SHA-256 de los componentes disponibles. |
| [p00-qa-resumen.json](p00-qa-resumen.json) | Resultado estructurado de backend, pruebas, frontend y validación Compose. |
| [p00-qa.log](p00-qa.log) | Salida completa de la ejecución aprobada de `scripts/qa.sh`. |
| [p00-qa-initial-failure.log](p00-qa-initial-failure.log) | Fallo reproducible detectado antes de agregar el marcador de `wwwroot`. |

El dump se conserva en `data/backups/p00-2026-09-27/`, ruta excluida de Git. Su integridad puede comprobarse con el SHA-256 registrado en la evidencia JSON.

## Pendiente externo para G0

La base, los adjuntos y los documentos oficiales del hotel no estaban disponibles durante esta ejecución. G0 seguirá abierta hasta que el propietario entregue esas fuentes y se completen P00-02B, P00-03B y P00-04B. La ausencia de esos datos no invalida la QA técnica realizada, pero impide afirmar que la línea base oficial es recuperable.
