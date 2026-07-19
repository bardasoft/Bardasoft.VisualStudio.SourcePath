# Publicación en Visual Studio Marketplace

Estos archivos preparan la publicación de **SourcePath**.

## Archivos agregados

```text
marketplace/
  overview.md
  vs-publish.json
  vs-publish.template.json
  README_PUBLICACION.md
  CHECKLIST_PUBLICACION.md
  images/
    icon.png
    preview.png
    marketplace-preview.png
    footer-example.png
```

## Qué debes revisar antes de publicar

### Publisher ID

En `marketplace/vs-publish.json` revisa:

```json
"publisher": "Bardasoft"
```

Ese valor debe coincidir exactamente con tu Publisher ID real en Visual Studio Marketplace.

### Nombre interno

```json
"internalName": "SourcePath"
```

Este valor debe mantenerse estable después de publicar.

### Repositorio GitHub

```json
"repo": "https://github.com/Bardasoft/Bardasoft.VisualStudio.SourcePath"
```

Actualízalo si tu repositorio público final usa otra URL.

### PayPal

Reemplaza el placeholder:

```text
https://www.paypal.com/donate/?hosted_button_id=EM5DZL2RPA8SL
```

## Compilar

Desde la raíz del proyecto:

```bat
scriptsuild-vsix.bat
```

## Publicar

```bat
scripts\publish-marketplace.bat TU_PERSONAL_ACCESS_TOKEN
```

O con PowerShell:

```powershell
.\scripts\publish-marketplace.ps1 -PersonalAccessToken "TU_PERSONAL_ACCESS_TOKEN"
```

## Seguridad

No guardes el Personal Access Token dentro del repositorio ni en archivos de texto.
