@echo off
setlocal EnableExtensions EnableDelayedExpansion
title BAJAR CAMBIOS DESDE GITHUB

REM ============================================================
REM bajar.bat
REM Baja los cambios de GitHub hacia tu repositorio local.
REM Si tienes cambios locales sin guardar en commit, los guarda
REM temporalmente con stash, baja cambios, y luego intenta restaurarlos.
REM ============================================================

call :FindGit
if errorlevel 1 goto :EndWithError

call :FindRepoRoot
if errorlevel 1 goto :EndWithError

cd /d "%REPO_ROOT%"

echo.
echo ============================================================
echo  BAJAR CAMBIOS DESDE GITHUB
echo ============================================================
echo Repositorio:
echo   %REPO_ROOT%
echo.

"%GIT_EXE%" rev-parse --is-inside-work-tree >nul 2>nul
if errorlevel 1 (
    echo ERROR: Esta carpeta no parece ser un repositorio Git.
    goto :EndWithError
)

for /f "delims=" %%b in ('"%GIT_EXE%" rev-parse --abbrev-ref HEAD 2^>nul') do set "BRANCH=%%b"

if "%BRANCH%"=="" (
    echo ERROR: No se pudo detectar la rama actual.
    goto :EndWithError
)

if /I "%BRANCH%"=="HEAD" (
    echo ERROR: Estas en estado HEAD separado. Cambia a una rama normal antes de bajar.
    goto :EndWithError
)

"%GIT_EXE%" remote get-url origin >nul 2>nul
if errorlevel 1 (
    echo ERROR: No existe el remoto "origin".
    echo Debes vincular primero el repositorio local con GitHub.
    goto :EndWithError
)

echo Rama actual:
echo   %BRANCH%
echo.

set "HAS_CHANGES="
for /f "delims=" %%s in ('"%GIT_EXE%" status --porcelain') do (
    set "HAS_CHANGES=1"
)

set "STASHED="

if defined HAS_CHANGES (
    echo Cambios locales sin commit detectados.
    echo Guardandolos temporalmente antes de bajar cambios...
    "%GIT_EXE%" stash push -u -m "Auto stash before pull - %DATE% %TIME%"
    if errorlevel 1 goto :EndWithError
    set "STASHED=1"
) else (
    echo No hay cambios locales pendientes.
)

echo.
echo Descargando informacion remota...
"%GIT_EXE%" fetch origin
if errorlevel 1 goto :EndWithError

echo.
echo Bajando cambios desde GitHub...
"%GIT_EXE%" pull --rebase origin "%BRANCH%"
if errorlevel 1 (
    echo.
    echo ERROR: No se pudo bajar correctamente.
    echo Puede existir un conflicto durante el rebase.
    echo Revisa el estado con Git o Visual Studio.
    goto :EndWithError
)

if defined STASHED (
    echo.
    echo Restaurando tus cambios locales guardados temporalmente...
    "%GIT_EXE%" stash pop
    if errorlevel 1 (
        echo.
        echo ADVERTENCIA: Se bajaron los cambios de GitHub, pero hubo conflicto al restaurar tus cambios locales.
        echo Revisa los archivos en conflicto.
        goto :EndWithError
    )
)

echo.
echo ============================================================
echo  LISTO: cambios bajados correctamente desde GitHub.
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

for %%i in ("%REPO_ROOT%.") do set "REPO_ROOT=%%~fi"

:FindRepoLoop
if exist "%REPO_ROOT%\.git" (
    exit /b 0
)

for %%i in ("%REPO_ROOT%\..") do set "PARENT=%%~fi"

if /I "%PARENT%"=="%REPO_ROOT%" (
    echo ERROR: No se encontro una carpeta .git desde:
    echo   %~dp0
    echo Coloca este archivo dentro de tu repositorio o en una subcarpeta del repositorio.
    exit /b 1
)

set "REPO_ROOT=%PARENT%"
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
