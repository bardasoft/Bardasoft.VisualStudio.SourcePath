@echo off
setlocal EnableExtensions EnableDelayedExpansion
title SUBIR CAMBIOS A GITHUB

REM ============================================================
REM subir.bat
REM Guarda cambios locales en un commit y los sube a GitHub.
REM
REM Uso:
REM   subir.bat
REM
REM Uso con mensaje personalizado:
REM   subir.bat "Fix GitHub language detection"
REM
REM IMPORTANTE:
REM Esta version corrige el error:
REM   No se esperaba GitHub en este momento.
REM ============================================================

call :FindGit
if errorlevel 1 goto :EndWithError

call :FindRepoRoot
if errorlevel 1 goto :EndWithError

cd /d "!REPO_ROOT!"

echo.
echo ============================================================
echo  SUBIR CAMBIOS A GITHUB
echo ============================================================
echo Repositorio:
echo   !REPO_ROOT!
echo.

"!GIT_EXE!" rev-parse --is-inside-work-tree >nul 2>nul
if errorlevel 1 (
    echo ERROR: Esta carpeta no parece ser un repositorio Git.
    goto :EndWithError
)

for /f "delims=" %%b in ('"!GIT_EXE!" rev-parse --abbrev-ref HEAD 2^>nul') do set "BRANCH=%%b"

if not defined BRANCH (
    echo ERROR: No se pudo detectar la rama actual.
    goto :EndWithError
)

if /I "!BRANCH!"=="HEAD" (
    echo ERROR: Estas en estado HEAD separado. Cambia a una rama normal antes de subir.
    goto :EndWithError
)

"!GIT_EXE!" remote get-url origin >nul 2>nul
if errorlevel 1 (
    echo ERROR: No existe el remoto "origin".
    echo Debes vincular primero el repositorio local con GitHub.
    goto :EndWithError
)

echo Rama actual:
echo   !BRANCH!
echo.

REM Leer mensaje de commit de forma segura.
REM %~1 quita las comillas externas del primer argumento.
set "COMMIT_MESSAGE=%~1"

if not defined COMMIT_MESSAGE (
    set "COMMIT_MESSAGE=Update local changes"
)

echo Mensaje de commit:
echo   !COMMIT_MESSAGE!
echo.

echo Revisando cambios locales...
set "HAS_CHANGES="

for /f "delims=" %%s in ('"!GIT_EXE!" status --porcelain') do (
    set "HAS_CHANGES=1"
)

if defined HAS_CHANGES (
    echo.
    echo Cambios detectados. Preparando archivos...
    "!GIT_EXE!" add -A
    if errorlevel 1 goto :EndWithError

    echo.
    echo Creando commit...
    "!GIT_EXE!" commit -m "!COMMIT_MESSAGE!"
    if errorlevel 1 goto :EndWithError
) else (
    echo No hay cambios locales para confirmar.
)

echo.
echo Descargando informacion remota...
"!GIT_EXE!" fetch origin
if errorlevel 1 goto :EndWithError

echo.
echo Actualizando tu rama local antes de subir...
"!GIT_EXE!" pull --rebase origin "!BRANCH!"
if errorlevel 1 (
    echo.
    echo ERROR: No se pudo actualizar la rama local.
    echo Puede existir un conflicto. Debes resolverlo y volver a ejecutar subir.bat.
    goto :EndWithError
)

echo.
echo Subiendo cambios a GitHub...
"!GIT_EXE!" push origin "!BRANCH!"
if errorlevel 1 (
    echo.
    echo ERROR: No se pudo hacer push hacia GitHub.
    goto :EndWithError
)

echo.
echo ============================================================
echo  LISTO: cambios subidos correctamente a GitHub.
echo ============================================================
goto :EndOk

:FindGit
set "GIT_EXE="

where git >nul 2>nul
if not errorlevel 1 (
    for /f "delims=" %%g in ('where git') do (
        if not defined GIT_EXE set "GIT_EXE=%%g"
    )
)

if not defined GIT_EXE (
    if exist "%ProgramFiles%\Git\cmd\git.exe" set "GIT_EXE=%ProgramFiles%\Git\cmd\git.exe"
)

if not defined GIT_EXE (
    if exist "%ProgramFiles(x86)%\Git\cmd\git.exe" set "GIT_EXE=%ProgramFiles(x86)%\Git\cmd\git.exe"
)

if not defined GIT_EXE (
    echo ERROR: No se encontro git.exe.
    echo Instala Git for Windows o abre este .bat desde una consola donde Git este disponible.
    exit /b 1
)

exit /b 0

:FindRepoRoot
set "REPO_ROOT=%~dp0"

for %%i in ("!REPO_ROOT!.") do set "REPO_ROOT=%%~fi"

:FindRepoLoop
if exist "!REPO_ROOT!\.git" (
    exit /b 0
)

for %%i in ("!REPO_ROOT!\..") do set "PARENT=%%~fi"

if /I "!PARENT!"=="!REPO_ROOT!" (
    echo ERROR: No se encontro una carpeta .git desde:
    echo   %~dp0
    echo Coloca este archivo en la misma carpeta donde esta el archivo .sln.
    exit /b 1
)

set "REPO_ROOT=!PARENT!"
goto :FindRepoLoop

:EndWithError
echo.
echo ============================================================
echo  PROCESO DETENIDO POR ERROR
echo ============================================================
pause
exit /b 1

:EndOk
echo.
pause
exit /b 0
