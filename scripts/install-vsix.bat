@echo off
setlocal EnableExtensions EnableDelayedExpansion

set "SCRIPT_DIR=%~dp0"
set "ROOT_DIR=%SCRIPT_DIR%.."
set "VSIX=%ROOT_DIR%\src\Bardasoft.VisualStudio.SourcePath\bin\Release\Bardasoft.VisualStudio.SourcePath.vsix"
set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"

if not exist "!VSIX!" (
    echo ERROR: No existe el archivo VSIX:
    echo !VSIX!
    echo.
    echo Primero ejecuta:
    echo scripts\build-vsix.bat
    exit /b 1
)

if not exist "!VSWHERE!" (
    echo ERROR: No se encontro vswhere.exe en:
    echo !VSWHERE!
    exit /b 1
)

set "VSIXINSTALLER="
for /f "usebackq delims=" %%i in (`"!VSWHERE!" -latest -products * -find Common7\IDE\VSIXInstaller.exe`) do (
    set "VSIXINSTALLER=%%i"
)

if not defined VSIXINSTALLER (
    echo ERROR: No se encontro VSIXInstaller.exe.
    echo Puedes instalar manualmente haciendo doble clic sobre:
    echo !VSIX!
    exit /b 1
)

echo Instalando VSIX:
echo !VSIX!
echo.

"!VSIXINSTALLER!" "!VSIX!"
exit /b !ERRORLEVEL!
