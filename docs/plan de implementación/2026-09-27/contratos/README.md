# Contrato API versionado

`openapi-v1.json` es el snapshot del documento real generado por ASP.NET durante una integración con PostgreSQL sintético. No contiene registros de huéspedes ni credenciales. El endpoint `/openapi/v1.json` conserva su disponibilidad exclusiva en Development.

## Comprobar

Desde la raíz del proyecto:

```bash
npm --prefix scripts/api-contract ci
npm --prefix frontend run api:check
```

Esto compara el snapshot y los tipos generados. Para contrastar también con la API real, configure `HOTEL_ERP_TEST_ADMIN_CONNECTION` fuera del repositorio y ejecute `bash scripts/qa.sh`, o `./scripts/qa.ps1` en Windows. La integración crea una base aislada, exporta su OpenAPI a un archivo temporal y QA comprueba las diferencias. No use la base productiva como destino de pruebas.

## Actualizar tras un cambio intencional

```bash
# La conexión administrativa de pruebas ya debe estar en el entorno.
HOTEL_ERP_OPENAPI_OUTPUT=/tmp/hotel-erp-openapi.json dotnet test backend/tests/hotel-erp.IntegrationTests/hotel-erp.IntegrationTests.csproj --filter FullyQualifiedName~BootstrapPasswordChangeAndLogout_InvalidatePreviousTokens
node scripts/api-contract/generate.mjs --live-schema /tmp/hotel-erp-openapi.json
npm --prefix frontend run api:check
```

Revise el diff del snapshot, de `frontend/src/types/api.generated.ts` y de los consumidores. El archivo indicado con `--live-schema` debe existir. Adapte la ruta temporal y la asignación de variables al usar PowerShell. Después ejecute QA integral. `npm --prefix frontend run api:generate` regenera tipos desde el snapshot ya guardado; no obtiene un contrato nuevo del servidor.

El generador está aislado en `scripts/api-contract` para respetar su dependencia de TypeScript 5 sin modificar TypeScript 6 del frontend. Ambos lockfiles se auditan en QA.

[Alcance y pendientes de P02-H1](../p02-base-tecnica/p02-h1-contrato-paginacion.md).
