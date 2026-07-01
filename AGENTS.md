# AGENTS.md - SourcePath

## Proyecto

Este repositorio contiene **SourcePath**, una extension VSIX gratuita y open-source para **Visual Studio 2022**.

La extension agrega un margen inferior discreto dentro del editor de Visual Studio y muestra la ruta fisica completa del archivo activo. Tambien permite:

- copiar la ruta completa;
- copiar solo el nombre del archivo;
- copiar la carpeta contenedora;
- abrir la ubicacion en Windows Explorer;
- usar `Ctrl+Click` sobre el footer para abrir la ubicacion del archivo.

## Raiz y estructura

La raiz de trabajo es:

```text
E:\source\Bardasoft.SourcePath\Bardasoft.VisualStudio.SourcePath
```

Estructura principal:

```text
AGENTS.md
.codegraph\
  .gitignore
  codegraph.db
.gitattributes
.gitignore
Bardasoft.VisualStudio.SourcePath.sln
Build.bat
Install.bat
README.md
LICENSE
DONATIONS.md
scripts\
  build-vsix.bat
  install-vsix.bat
  fix-vsix-double-click.ps1
  package-vsix.ps1
  publish-marketplace.ps1
  publish-marketplace.bat
  sync-marketplace-images.bat
src\
  Bardasoft.VisualStudio.SourcePath\
    Bardasoft.VisualStudio.SourcePath.csproj
    SourcePathFooterMargin.cs
    SourcePathFooterMarginProvider.cs
    source.extension.vsixmanifest
    Properties\AssemblyInfo.cs
    Docs\
    Resources\
tests\
  Bardasoft.VisualStudio.SourcePath.Tests\
    Bardasoft.VisualStudio.SourcePath.Tests.csproj
    SourcePathPathFormatterTests.cs
    MSTestSettings.cs
```

En el workspace local tambien puede existir una carpeta hermana `..\marketplace\` con material de publicacion. Esa carpeta no forma parte de la raiz GitHub de `bardasoft/Bardasoft.VisualStudio.SourcePath` a menos que se agregue explicitamente.

## Tecnologia y version actual

- Tipo de proyecto: extension VSIX para Visual Studio.
- Visual Studio destino: Visual Studio 2022, rango `[17.0,19.0)`.
- Target framework: `.NET Framework 4.7.2`.
- Lenguaje: C# con `LangVersion` en `latest` y `Nullable` habilitado.
- Componente principal: margen WPF del editor via MEF.
- Paquetes principales:
  - `Microsoft.VisualStudio.Text.UI.Wpf`
  - `Microsoft.VisualStudio.Text.UI`
  - `Microsoft.VisualStudio.Text.Data`
  - `Microsoft.VisualStudio.Text.Logic`
  - `Microsoft.VisualStudio.Utilities`
  - `Microsoft.VisualStudio.CoreUtility`
  - `Microsoft.VisualStudio.Shell.15.0`
  - `Microsoft.VisualStudio.Shell.Framework`
  - `Microsoft.VSSDK.BuildTools`
- Version actual declarada:
  - `source.extension.vsixmanifest`: `2.1.0`
  - `AssemblyInfo.cs`: `2.1.0.0`

Cuando se cambie la version de publicacion, mantener sincronizados:

```text
src\Bardasoft.VisualStudio.SourcePath\source.extension.vsixmanifest
src\Bardasoft.VisualStudio.SourcePath\Properties\AssemblyInfo.cs
src\Bardasoft.VisualStudio.SourcePath\Docs\CHANGELOG.txt
```

## Herramientas necesarias

### Visual Studio y SDK

- Visual Studio 2022 con soporte para extensiones:
  - workload/componente de desarrollo de extensiones de Visual Studio;
  - `Microsoft.VisualStudio.Component.CoreEditor`;
  - `Microsoft.VisualStudio.Component.VSSDK`;
  - MSBuild instalado.
- .NET Framework 4.7.2 Developer Pack o targeting pack disponible para compilar `net472`.
- VSSDK Build Tools restaurado por NuGet mediante:

```text
Microsoft.VSSDK.BuildTools
```

### Herramientas usadas por scripts

- `vswhere.exe`
  - Ruta esperada:

```text
%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe
```

  - Lo usan `scripts\build-vsix.bat`, `scripts\install-vsix.bat` y `scripts\publish-marketplace.ps1` para localizar MSBuild, VSIXInstaller y VsixPublisher.

- MSBuild
  - Debe localizarse con `vswhere`.
  - Usar MSBuild como herramienta principal de build. No reemplazar por `dotnet build` salvo que se pruebe explicitamente que genera el VSIX correctamente en este proyecto clasico.

- PowerShell
  - Necesario para `scripts\package-vsix.ps1` y `scripts\publish-marketplace.ps1`.
  - `package-vsix.ps1` usa APIs XML y `Compress-Archive` para construir manualmente el VSIX cuando MSBuild no produce el contenedor.

- `VSIXInstaller.exe`
  - Lo usa `scripts\install-vsix.bat`.
  - Permite instalar localmente:

```text
src\Bardasoft.VisualStudio.SourcePath\bin\Release\Bardasoft.VisualStudio.SourcePath.vsix
```

- `scripts\fix-vsix-double-click.ps1`
  - Repara la asociacion por usuario de archivos `.vsix` cuando Windows/Visual Studio abre primero un bootstrapper de consola y aparece una ventana negra antes del instalador.
  - Apunta el doble clic al `VSIXInstaller.exe` grafico bajo `Microsoft.VisualStudio.Setup.Service\VsixInstaller`.
  - Es reversible con:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts\fix-vsix-double-click.ps1 -RestoreVisualStudioLauncher
```

- `VsixPublisher.exe`
  - Lo usa `scripts\publish-marketplace.ps1`.
  - Requiere Visual Studio con VSSDK instalado.
  - Requiere `PersonalAccessToken`; nunca guardar el token en archivos del repo.

### Herramientas de exploracion y mantenimiento

- CodeGraph
  - Ya existe `.codegraph/` en la raiz del proyecto.
  - Usar para preguntas estructurales, simbolos, dependencias, impacto y flujo de llamadas.

- `rg`
  - Usar para busquedas literales rapidas en archivos, textos de documentacion, manifiestos, mensajes, nombres de recursos y scripts.

- Git
  - Usar para revisar cambios cuando el repositorio este inicializado o cuando se trabaje desde una raiz con `.git`.
  - Si `git status` falla porque la carpeta no es repo, no asumir que no hay cambios: revisar archivos editados directamente.

- Windows Explorer
  - Necesario para validar la funcion `OpenInExplorer`.
  - La extension invoca `explorer.exe` con `/select,"<archivo>"` cuando el archivo existe.

- Visual Studio experimental o instancia local de prueba
  - Recomendado para validar cambios visuales/interactivos.
  - Probar con temas claro y oscuro, archivos con ruta fisica, documentos sin ruta fisica y rutas largas.

## Codigo principal

### `SourcePathFooterMarginProvider.cs`

Registra el margen del editor con MEF:

- `IWpfTextViewMarginProvider`
- `[Export(typeof(IWpfTextViewMarginProvider))]`
- `[MarginContainer(PredefinedMarginNames.Bottom)]`
- `[Order(After = PredefinedMarginNames.HorizontalScrollBar)]`
- `[ContentType("text")]`
- `[TextViewRole(PredefinedTextViewRoles.Document)]`

Este archivo debe mantenerse pequeno. Su responsabilidad es validar dependencias MEF y crear `SourcePathFooterMargin`.

### `SourcePathFooterMargin.cs`

Implementa el margen visual y la logica de uso:

- hereda de `Border`;
- implementa `IWpfTextViewMargin`;
- obtiene la ruta mediante `ITextDocumentFactoryService`;
- escucha `ITextDocument.FileActionOccurred`;
- se libera cuando `_textView.Closed` ocurre;
- usa recursos de tema de Visual Studio:
  - `EnvironmentColors.ToolWindowBackgroundBrushKey`;
  - `EnvironmentColors.ToolWindowTextBrushKey`;
- usa `Clipboard.SetText(...)` para copiado;
- usa `explorer.exe` para abrir carpeta o seleccionar archivo.

Preservar el comportamiento de disposal:

- no dejar handlers vivos despues de `Dispose()`;
- desuscribirse de `_textView.Closed`;
- desuscribirse de `_textDocument.FileActionOccurred`;
- evitar ejecutar acciones si `_isDisposed` es `true`.

## Reglas de edicion

- Mantener el namespace:

```csharp
namespace Bardasoft.VisualStudio.SourcePath;
```

- Mantener `#nullable enable` en archivos C# existentes.
- Evitar dependencias nuevas salvo que sean necesarias para la extension y compatibles con VSIX/.NET Framework 4.7.2.
- No convertir el proyecto a SDK-style sin una razon explicita: es un proyecto VSIX clasico con imports y targets del VSSDK.
- No cambiar IDs, publisher o nombre del paquete sin indicacion explicita:
  - Identity Id: `Bardasoft.VisualStudio.SourcePath`
  - Publisher: `bardasoft`
  - DisplayName: `SourcePath`
- Mantener `Assets/Microsoft.VisualStudio.MefComponent` apuntando correctamente al proyecto o al DLL empaquetado.
- Si se cambian recursos visuales, sincronizar:
  - `src\Bardasoft.VisualStudio.SourcePath\Resources\`
  - `marketplace\images\`
  - manifiesto VSIX si cambia icono o preview.
- No publicar al marketplace sin confirmacion explicita del usuario.
- No borrar ni regenerar `marketplace\vs-publish.json` sin revisar si contiene datos reales de publicacion.
- No incluir tokens, PATs ni secretos en el repositorio.

## Comandos

Ejecutar comandos desde:

```powershell
cd E:\source\Bardasoft.SourcePath\Bardasoft.VisualStudio.SourcePath
```

Compilar y generar VSIX:

```bat
Build.bat
```

o:

```bat
scripts\build-vsix.bat
```

El script usa `vswhere.exe` para localizar MSBuild y ejecuta:

```bat
MSBuild Bardasoft.VisualStudio.SourcePath.sln /t:Restore;Build /p:Configuration=Release /p:Platform="Any CPU" /p:CreateVsixContainer=true /p:DeployExtension=false /p:DeployVSTemplates=false /p:VisualStudioVersion=17.0 /v:m
```

Ejecutar pruebas de refactorizacion:

```powershell
dotnet test tests\Bardasoft.VisualStudio.SourcePath.Tests\Bardasoft.VisualStudio.SourcePath.Tests.csproj --no-build -c Release
```

Si MSBuild compila el DLL pero no genera el contenedor VSIX, el script llama:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts\package-vsix.ps1 -Configuration Release
```

Instalar localmente el VSIX compilado:

```bat
Install.bat
```

o:

```bat
scripts\install-vsix.bat
```

Reparar doble clic de `.vsix` si aparece una ventana de consola antes del instalador:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts\fix-vsix-double-click.ps1
```

El VSIX esperado queda en:

```text
src\Bardasoft.VisualStudio.SourcePath\bin\Release\Bardasoft.VisualStudio.SourcePath.vsix
```

Publicar en marketplace solo con autorizacion explicita:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts\publish-marketplace.ps1 -PersonalAccessToken <PAT>
```

## Validacion recomendada

Para cambios de codigo C#:

1. Ejecutar `Build.bat`.
2. Ejecutar `dotnet test tests\Bardasoft.VisualStudio.SourcePath.Tests\Bardasoft.VisualStudio.SourcePath.Tests.csproj --no-build -c Release`.
3. Confirmar que se genero el `.vsix` en `bin\Release`.
4. Instalar con `Install.bat` si el cambio afecta comportamiento visual o interaccion.
5. Probar en Visual Studio 2022:
   - abrir un archivo con ruta fisica;
   - confirmar que aparece el footer;
   - confirmar ellipsis para rutas largas;
   - probar menu contextual;
   - probar copiar ruta, nombre y carpeta;
   - probar abrir ubicacion en Explorer;
   - probar `Ctrl+Click`;
   - cerrar el editor y verificar que no hay errores.

Para cambios de manifiesto o marketplace:

1. Revisar `source.extension.vsixmanifest`.
2. Revisar `Docs\README.txt`, `Docs\CHANGELOG.txt`, `Docs\LICENSE.txt` y `Docs\DONATIONS.md`.
3. Revisar imagenes en `Resources\` y `marketplace\images\`.
4. Ejecutar `Build.bat`.
5. Inspeccionar el VSIX generado si el cambio afecta assets o metadata.

## Marketplace y documentacion

La documentacion publica vive en dos zonas:

- README/documentacion del proyecto:

```text
README.md
src\Bardasoft.VisualStudio.SourcePath\Docs\
```

- Marketplace local, si existe en el workspace:

```text
..\marketplace\
```

Antes de cambiar descripciones, claims, capturas o version, revisar:

```text
..\marketplace\WHERE_TO_EDIT.md
..\marketplace\CHECKLIST_PUBLICACION.md
..\marketplace\README_PUBLICACION.md
..\marketplace\overview.md
..\marketplace\MARKETPLACE.md
```

## CodeGraph

CodeGraph builds a semantic knowledge graph of codebases for faster code exploration.

Este proyecto tiene `.codegraph/` inicializado en la raiz:

```text
E:\source\Bardasoft.SourcePath\Bardasoft.VisualStudio.SourcePath\.codegraph\
```

Usar herramientas CodeGraph primero para preguntas estructurales: arquitectura, simbolos, call-flow, impacto y ubicacion de definiciones. Preferir CodeGraph sobre busquedas amplias con `rg` cuando la pregunta sea estructural.

Comando de inicializacion usado:

```powershell
codegraph init -i
```

Si el indice parece desactualizado, revisar su estado antes de contestar o cambiar codigo. No borrar `.codegraph/` salvo que el usuario lo pida.

Si en otra copia del proyecto `.codegraph/` no existe, preguntar primero:

```text
Noto que este proyecto no tiene CodeGraph inicializado. Quieres que ejecute `codegraph init -i` para crear el indice?
```

### Cuando preferir CodeGraph

| Pregunta | Herramienta |
|---|---|
| Donde esta definido X? | `codegraph_search` |
| Que llama a Y? | `codegraph_callers` |
| Que llama Y? | `codegraph_callees` |
| Como llega X a Y? | `codegraph_trace` |
| Que se rompe si cambio Z? | `codegraph_impact` |
| Mostrar firma/source/docstring | `codegraph_node` |
| Contexto enfocado de un area | `codegraph_context` |
| Explorar varios simbolos relacionados | `codegraph_explore` |
| Archivos bajo una ruta | `codegraph_files` |
| Salud del indice | `codegraph_status` |

Para preguntas de texto literal, mensajes, comentarios, contenido de manifiestos o archivos de documentacion, usar `rg`/lectura directa.

## Notas para futuros agentes

- Este repo es pequeno: preferir cambios puntuales y faciles de revisar.
- El usuario quiere trabajar sobre esta extension para mejorarla; conservar el comportamiento existente salvo que pida cambiarlo.
- Al tocar UI del editor, pensar en temas claro/oscuro, altura fija del footer, clipping, ellipsis y limpieza visual.
- Al tocar interacciones, cuidar que no falle cuando el documento no tenga ruta fisica.
- Al tocar Explorer/Clipboard, validar `null`, rutas vacias, rutas inexistentes y carpetas existentes.
- Al tocar empaquetado, verificar que el VSIX incluya DLL, manifiesto final, docs y recursos.
- Al refactorizar logica pura, agregar o actualizar pruebas en `tests\Bardasoft.VisualStudio.SourcePath.Tests`.
- Mantener los textos visibles de la extension en espanol salvo que el usuario pida localizacion bilingue o inglesa.
