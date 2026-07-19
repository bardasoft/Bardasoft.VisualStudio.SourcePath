# Dónde editar la información visible de SourcePath

## VSIX local / Extension Manager

Archivo:

```text
src\Bardasoft.VisualStudio.SourcePath\source.extension.vsixmanifest
```

Campos principales:

```xml
<DisplayName>SourcePath</DisplayName>
<Description>...</Description>
<Icon>Resources\Icon128.png</Icon>
<PreviewImage>Resources\Preview200.png</PreviewImage>
```

El nodo `<Description>` es texto plano y debe mantenerse corto.

## Marketplace

Presentación enriquecida:

```text
marketplace\overview.md
```

Manifiesto de publicación:

```text
marketplaces-publish.json
```

Imágenes:

```text
marketplace\images```

## Documentación interna del VSIX

```text
src\Bardasoft.VisualStudio.SourcePath\Docs\README.txt
src\Bardasoft.VisualStudio.SourcePath\Docs\CHANGELOG.txt
src\Bardasoft.VisualStudio.SourcePath\Docs\LICENSE.txt
```
