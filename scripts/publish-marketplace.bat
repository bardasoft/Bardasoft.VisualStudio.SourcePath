@echo off
setlocal EnableExtensions EnableDelayedExpansion

set "SCRIPT_DIR=%~dp0"
set "ROOT=%SCRIPT_DIR%.."
set "VSIX=%ROOT%\src\Bardasoft.VisualStudio.SourcePath\bin\Release\Bardasoft.VisualStudio.SourcePath.vsix"

set "MARKETPLACE_DIR=%ROOT%\marketplace"
if not exist "!MARKETPLACE_DIR!\vs-publish.json" (
    set "MARKETPLACE_DIR=%ROOT%\..\marketplace"
)

set "PUBLISH_MANIFEST=vs-publish.json"

if "%~1"=="" (
    echo ERROR: Debes pasar el Personal Access Token como primer argumento.
    echo.
    echo Uso:
    echo   scripts\publish-marketplace.bat TU_PERSONAL_ACCESS_TOKEN
    exit /b 1
)

set "PAT=%~1"

if not exist "!VSIX!" (
    echo ERROR: No existe el archivo VSIX:
    echo !VSIX!
    echo.
    echo Primero ejecuta:
    echo   scripts\build-vsix.bat
    exit /b 1
)

if not exist "!MARKETPLACE_DIR!\!PUBLISH_MANIFEST!" (
    echo ERROR: No existe el publish manifest:
    echo !MARKETPLACE_DIR!\!PUBLISH_MANIFEST!
    echo.
    echo La carpeta marketplace debe estar en una de estas ubicaciones:
    echo   !ROOT!\marketplace
    echo   !ROOT!\..\marketplace
    exit /b 1
)

set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"

if not exist "!VSWHERE!" (
    echo ERROR: No se encontro vswhere.exe:
    echo !VSWHERE!
    exit /b 1
)

set "VSINSTALLDIR="
for /f "usebackq delims=" %%i in (`"!VSWHERE!" -latest -products * -requires Microsoft.VisualStudio.Component.VSSDK -property installationPath`) do (
    set "VSINSTALLDIR=%%i"
)

if not defined VSINSTALLDIR (
    echo ERROR: No se encontro Visual Studio con VSSDK instalado.
    exit /b 1
)

set "VSIXPUBLISHER=!VSINSTALLDIR!\VSSDK\VisualStudioIntegration\Tools\Bin\VsixPublisher.exe"

if not exist "!VSIXPUBLISHER!" (
    echo ERROR: No se encontro VsixPublisher.exe:
    echo !VSIXPUBLISHER!
    exit /b 1
)

pushd "!MARKETPLACE_DIR!"

"!VSIXPUBLISHER!" publish ^
  -payload "!VSIX!" ^
  -publishManifest "!PUBLISH_MANIFEST!" ^
  -personalAccessToken "!PAT!"

set "EXITCODE=!ERRORLEVEL!"
popd

if not "!EXITCODE!"=="0" (
    echo.
    echo ERROR: Fallo la publicacion.
    exit /b !EXITCODE!
)

echo.
echo Publicacion finalizada correctamente.
exit /b 0
