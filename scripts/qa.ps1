param(
    [string]$PostgresAdminConnection = $env:HOTEL_ERP_TEST_ADMIN_CONNECTION
)

$ErrorActionPreference = 'Stop'
$rootDir = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path

function Invoke-QaStep {
    param(
        [string]$Name,
        [scriptblock]$Action
    )

    Write-Host "[QA] $Name" -ForegroundColor Cyan
    & $Action
    if ($LASTEXITCODE -ne 0) {
        throw "Falló: $Name (código $LASTEXITCODE)"
    }
}

$contractOutput = Join-Path ([System.IO.Path]::GetTempPath()) ("hotel-erp-contract-" + [Guid]::NewGuid().ToString('N') + '.json')
$previousContractOutput = $env:HOTEL_ERP_OPENAPI_OUTPUT
try {
Invoke-QaStep 'Verificando límites de repositorios' {
    node (Join-Path $rootDir 'scripts/check-repository-boundaries.mjs')
}
Invoke-QaStep 'Restaurando backend' {
    dotnet restore (Join-Path $rootDir 'backend/hotel-erp.slnx') --nologo
}
Invoke-QaStep 'Compilando backend' {
    dotnet build (Join-Path $rootDir 'backend/hotel-erp.slnx') --no-restore --nologo
}
Invoke-QaStep 'Ejecutando pruebas unitarias' {
    dotnet test (Join-Path $rootDir 'backend/tests/hotel-erp.UnitTests/hotel-erp.UnitTests.csproj') --no-restore --nologo
}

if (-not [string]::IsNullOrWhiteSpace($PostgresAdminConnection)) {
    $previousConnection = $env:HOTEL_ERP_TEST_ADMIN_CONNECTION
    try {
        $env:HOTEL_ERP_TEST_ADMIN_CONNECTION = $PostgresAdminConnection
        $env:HOTEL_ERP_OPENAPI_OUTPUT = $contractOutput
        Invoke-QaStep 'Ejecutando integración PostgreSQL' {
            dotnet test (Join-Path $rootDir 'backend/tests/hotel-erp.IntegrationTests/hotel-erp.IntegrationTests.csproj') --no-restore --nologo
        }
    }
    finally {
        $env:HOTEL_ERP_TEST_ADMIN_CONNECTION = $previousConnection
    }
}
else {
    Write-Host '[QA] Integración omitida: configure HOTEL_ERP_TEST_ADMIN_CONNECTION.' -ForegroundColor Yellow
}

Invoke-QaStep 'Validando expediente fiscal N01-N06' {
    node --test (Join-Path $rootDir 'scripts/tests/profile-dossier.test.mjs')
}

$frontendDir = Join-Path $rootDir 'frontend'
Invoke-QaStep 'Instalando frontend desde package-lock.json' {
    npm --prefix $frontendDir ci
}
Invoke-QaStep 'Auditando dependencias frontend' {
    npm --prefix $frontendDir audit --audit-level=low
}
$contractDir = Join-Path $rootDir 'scripts/api-contract'
Invoke-QaStep 'Instalando herramientas del contrato' { npm --prefix $contractDir ci }
Invoke-QaStep 'Auditando herramientas del contrato' { npm --prefix $contractDir audit --audit-level=low }
Invoke-QaStep 'Validando contrato y tipos TypeScript' {
    if (Test-Path $contractOutput) {
        node (Join-Path $contractDir 'generate.mjs') --check --live-schema $contractOutput
    } else {
        node (Join-Path $contractDir 'generate.mjs') --check
    }
}
Invoke-QaStep 'Ejecutando lint frontend'  {
    npm --prefix $frontendDir run lint
}
Invoke-QaStep 'Generando frontend de producción' {
    npm --prefix $frontendDir run build
}
Invoke-QaStep 'Validando espacios y marcadores de conflicto' {
    git -C $rootDir diff --check -- . ':(exclude)backend/src/hotel-erp.Api/bin/**' ':(exclude)backend/src/hotel-erp.Api/obj/**' ':(exclude)backend/src/hotel-erp.Api/wwwroot/assets/**'
}

Write-Host '[QA] Todo aprobado' -ForegroundColor Green

}
finally {
    $env:HOTEL_ERP_OPENAPI_OUTPUT = $previousContractOutput
    if (Test-Path $contractOutput) { Remove-Item -LiteralPath $contractOutput }
}
