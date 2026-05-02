param(
    [Parameter(Mandatory = $true)]
    [string]$PersonalAccessToken
)

$ErrorActionPreference = "Stop"

$Root = Resolve-Path (Join-Path $PSScriptRoot "..")
$Vsix = Join-Path $Root "src\Bardasoft.VisualStudio.SourcePath\bin\Release\Bardasoft.VisualStudio.SourcePath.vsix"

$MarketplaceDir = Join-Path $Root "marketplace"
if (-not (Test-Path (Join-Path $MarketplaceDir "vs-publish.json"))) {
    $MarketplaceDir = Join-Path $Root "..\marketplace"
}

$PublishManifest = Join-Path $MarketplaceDir "vs-publish.json"

if (-not (Test-Path $Vsix)) {
    throw "No existe el archivo VSIX. Primero ejecuta scripts\build-vsix.bat. Ruta: $Vsix"
}

if (-not (Test-Path $PublishManifest)) {
    throw "No existe el publish manifest: $PublishManifest"
}

$VsWhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"

if (-not (Test-Path $VsWhere)) {
    throw "No se encontro vswhere.exe: $VsWhere"
}

$VsInstallDir = & $VsWhere -latest -products * -requires Microsoft.VisualStudio.Component.VSSDK -property installationPath

if ([string]::IsNullOrWhiteSpace($VsInstallDir)) {
    throw "No se encontro Visual Studio con VSSDK instalado."
}

$VsixPublisher = Join-Path $VsInstallDir "VSSDK\VisualStudioIntegration\Tools\Bin\VsixPublisher.exe"

if (-not (Test-Path $VsixPublisher)) {
    throw "No se encontro VsixPublisher.exe: $VsixPublisher"
}

Push-Location $MarketplaceDir
try {
    & $VsixPublisher publish `
        -payload $Vsix `
        -publishManifest "vs-publish.json" `
        -personalAccessToken $PersonalAccessToken
}
finally {
    Pop-Location
}
