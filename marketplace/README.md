# SourcePath

**SourcePath** es una extensión gratuita y de código abierto para Visual Studio que muestra la ruta completa del archivo activo en un footer dentro del editor.

![Preview](src/Bardasoft.VisualStudio.SourcePath/Resources/Preview200.png)

## Características

- Muestra el path completo del archivo activo dentro del editor.
- Agrega un footer visual debajo del editor de texto.
- Permite copiar la ruta completa, el nombre del archivo o la carpeta desde el menú contextual.
- Permite abrir la ubicación del archivo en el Explorador con `Ctrl + clic` sobre el footer.
- Respeta los temas claro/oscuro de Visual Studio mediante `EnvironmentColors`.
- Es gratuito y de código abierto bajo licencia MIT.

## Publicación en Visual Studio Marketplace

Se agregó la carpeta:

```text
marketplace/
```

Archivos principales:

```text
marketplace/overview.md
marketplace/vs-publish.json
marketplace/README_PUBLICACION.md
marketplace/CHECKLIST_PUBLICACION.md
marketplace/images/
```

El archivo `overview.md` es la presentación enriquecida que se usa para Marketplace, con Markdown e imágenes.

## Compilación

```bat
scriptsuild-vsix.bat
```

Salida esperada:

```text
src\Bardasoft.VisualStudio.SourcePathin\Release\Bardasoft.VisualStudio.SourcePath.vsix
```

## Instalación local

```bat
scripts\install-vsix.bat
```

## Publicación

Después de compilar el VSIX y revisar `marketplaces-publish.json`:

```bat
scripts\publish-marketplace.bat TU_PERSONAL_ACCESS_TOKEN
```

También puedes usar PowerShell:

```powershell
.\scripts\publish-marketplace.ps1 -PersonalAccessToken "TU_PERSONAL_ACCESS_TOKEN"
```

## Dónde editar información visible

- Título local y descripción corta: `src\Bardasoft.VisualStudio.SourcePath\source.extension.vsixmanifest`
- Presentación enriquecida Marketplace: `marketplace\overview.md`
- Imágenes Marketplace: `marketplace\images\`
- Icono local VSIX: `src\Bardasoft.VisualStudio.SourcePath\Resources\Icon128.png`
- Preview local VSIX: `src\Bardasoft.VisualStudio.SourcePath\Resources\Preview200.png`
- Donaciones: `DONATIONS.md`, `marketplace\overview.md`, `src\Bardasoft.VisualStudio.SourcePath\Docs\DONATIONS.md`

## Licencia

Este proyecto se distribuye bajo licencia MIT. Consulte el archivo [LICENSE](LICENSE).
