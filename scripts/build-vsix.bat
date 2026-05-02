@echo off
setlocal EnableExtensions EnableDelayedExpansion

set "SCRIPT_DIR=%~dp0"
set "ROOT_DIR=%SCRIPT_DIR%.."
set "SLN=%ROOT_DIR%\Bardasoft.VisualStudio.SourcePath.sln"
set "PROJECT_DIR=%ROOT_DIR%\src\Bardasoft.VisualStudio.SourcePath"
set "OUT_DIR=%PROJECT_DIR%\bin\Release"
set "VSIX=%OUT_DIR%\Bardasoft.VisualStudio.SourcePath.vsix"
set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"

if not exist "!SLN!" (
    echo ERROR: No existe la solucion:
    echo !SLN!
    exit /b 1
)

if not exist "!VSWHERE!" (
    echo ERROR: No se encontro vswhere.exe en:
    echo !VSWHERE!
    exit /b 1
)

set "MSBUILD="
for /f "usebackq delims=" %%i in (`"!VSWHERE!" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do (
    set "MSBUILD=%%i"
)

if not defined MSBUILD (
    echo ERROR: No se encontro MSBuild mediante vswhere.
    exit /b 1
)

echo Compilando solucion:
echo !SLN!
echo.

"!MSBUILD!" "!SLN!" ^
  /t:Restore;Build ^
  /p:Configuration=Release ^
  /p:Platform="Any CPU" ^
  /p:CreateVsixContainer=true ^
  /p:DeployExtension=false ^
  /p:DeployVSTemplates=false ^
  /p:VisualStudioVersion=17.0 ^
  /v:m

if errorlevel 1 (
    echo.
    echo ERROR: Fallo la compilacion.
    exit /b 1
)

if exist "!VSIX!" (
    echo.
    echo VSIX generado correctamente por MSBuild:
    echo !VSIX!
    exit /b 0
)

echo.
echo MSBuild compilo el DLL, pero no genero el contenedor VSIX.
echo Ejecutando empaquetado manual compatible con VSIX...
echo.

powershell -NoProfile -ExecutionPolicy Bypass -File "!SCRIPT_DIR!package-vsix.ps1" -Configuration Release

if errorlevel 1 (
    echo.
    echo ERROR: Fallo el empaquetado manual del VSIX.
    exit /b 1
)

if not exist "!VSIX!" (
    echo.
    echo ERROR: No se encontro el VSIX despues del empaquetado manual:
    echo !VSIX!
    exit /b 1
)

echo.
echo VSIX generado correctamente:
echo !VSIX!
exit /b 0
