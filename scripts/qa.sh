#!/usr/bin/env bash
set -euo pipefail

root_dir=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)

node "$root_dir/scripts/check-repository-boundaries.mjs"
qa_contract_file=$(mktemp)
trap 'rm "$qa_contract_file"' EXIT

echo "[QA] Restaurando backend"
dotnet restore "$root_dir/backend/hotel-erp.slnx" --nologo

echo "[QA] Compilando backend"
dotnet build "$root_dir/backend/hotel-erp.slnx" --no-restore --nologo

echo "[QA] Ejecutando pruebas unitarias"
dotnet test "$root_dir/backend/tests/hotel-erp.UnitTests/hotel-erp.UnitTests.csproj" --no-restore --nologo

if [[ -n "${HOTEL_ERP_TEST_ADMIN_CONNECTION:-}" ]]; then
  echo "[QA] Ejecutando integración PostgreSQL"
  export HOTEL_ERP_OPENAPI_OUTPUT="$qa_contract_file"
  dotnet test "$root_dir/backend/tests/hotel-erp.IntegrationTests/hotel-erp.IntegrationTests.csproj" --no-restore --nologo
else
  echo "[QA] Integración omitida: configure HOTEL_ERP_TEST_ADMIN_CONNECTION."
fi

echo "[QA] Validando expediente fiscal N01-N06"
node --test "$root_dir/scripts/tests/profile-dossier.test.mjs"

echo "[QA] Instalando frontend desde package-lock.json"
npm --prefix "$root_dir/frontend" ci

echo "[QA] Auditando dependencias frontend"
npm --prefix "$root_dir/frontend" audit --audit-level=low

echo "[QA] Instalando y auditando herramientas del contrato"
npm --prefix "$root_dir/scripts/api-contract" ci
npm --prefix "$root_dir/scripts/api-contract" audit --audit-level=low
if [[ -s "$qa_contract_file" ]]; then
  node "$root_dir/scripts/api-contract/generate.mjs" --check --live-schema "$qa_contract_file"
else
  node "$root_dir/scripts/api-contract/generate.mjs" --check
fi

echo "[QA] Ejecutando lint frontend"
npm --prefix "$root_dir/frontend" run lint

echo "[QA] Generando frontend de producción"
npm --prefix "$root_dir/frontend" run build

echo "[QA] Validando espacios y marcadores de conflicto"
git -C "$root_dir" diff --check -- . \
  ':(exclude)backend/src/hotel-erp.Api/bin/**' \
  ':(exclude)backend/src/hotel-erp.Api/obj/**' \
  ':(exclude)backend/src/hotel-erp.Api/wwwroot/assets/**'

echo "[QA] Todo aprobado"
