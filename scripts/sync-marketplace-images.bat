@echo off
setlocal EnableExtensions EnableDelayedExpansion

set "ROOT=%~dp0.."
set "RES=%ROOT%\src\Bardasoft.VisualStudio.SourcePath\Resources"

set "MKT=%ROOT%\marketplace\images"
if not exist "%ROOT%\marketplace" (
    set "MKT=%ROOT%\..\marketplace\images"
)

if not exist "!MKT!" mkdir "!MKT!"

if exist "!RES!\Icon128.png" copy /Y "!RES!\Icon128.png" "!MKT!\icon.png"
if exist "!RES!\Preview200.png" copy /Y "!RES!\Preview200.png" "!MKT!\preview.png"
if exist "!RES!\marketplace-preview.png" copy /Y "!RES!\marketplace-preview.png" "!MKT!\marketplace-preview.png"
if exist "!RES!\footer-example.png" copy /Y "!RES!\footer-example.png" "!MKT!\footer-example.png"

echo Imagenes sincronizadas hacia:
echo !MKT!
