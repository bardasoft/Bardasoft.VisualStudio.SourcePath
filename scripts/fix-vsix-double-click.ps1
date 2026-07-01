param(
    [switch]$RestoreVisualStudioLauncher
)

$ErrorActionPreference = "Stop"

$vsixExtensionKey = "HKCU:\Software\Classes\.vsix"
$vsixCommandKey = "HKCU:\Software\Classes\VisualStudio.Launcher.vsix\shell\open\command"

if ($RestoreVisualStudioLauncher) {
    Remove-Item -Path "HKCU:\Software\Classes\.vsix" -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item -Path "HKCU:\Software\Classes\VisualStudio.Launcher.vsix" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "Se elimino la anulacion por usuario. Windows volvera a usar la asociacion de Visual Studio." -ForegroundColor Green
    exit 0
}

$setupServiceDir = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\resources\app\ServiceHub\Services\Microsoft.VisualStudio.Setup.Service"
$guiInstaller = Join-Path $setupServiceDir "VsixInstaller\VSIXInstaller.exe"

if (-not (Test-Path $guiInstaller)) {
    $guiInstaller = Get-ChildItem -Path $setupServiceDir -Filter VSIXInstaller.exe -Recurse -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -like "*\VsixInstaller\VSIXInstaller.exe" } |
        Select-Object -First 1 -ExpandProperty FullName
}

if (-not $guiInstaller -or -not (Test-Path $guiInstaller)) {
    throw "No se encontro el VSIXInstaller grafico bajo: $setupServiceDir"
}

New-Item -Path $vsixExtensionKey -Force | Out-Null
Set-ItemProperty -Path $vsixExtensionKey -Name "(default)" -Value "VisualStudio.Launcher.vsix"
Set-ItemProperty -Path $vsixExtensionKey -Name "Content Type" -Value "application/vsix"

New-Item -Path $vsixCommandKey -Force | Out-Null
Set-ItemProperty -Path $vsixCommandKey -Name "(default)" -Value "`"$guiInstaller`" `"%1`""

Write-Host "El doble clic de archivos .vsix ahora usa el instalador grafico:" -ForegroundColor Green
Write-Host $guiInstaller -ForegroundColor Green
