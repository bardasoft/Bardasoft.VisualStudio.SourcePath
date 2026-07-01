param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$SolutionRoot = Resolve-Path (Join-Path $ScriptDir "..")
$ProjectName = "Bardasoft.VisualStudio.SourcePath"
$ProjectDir = Join-Path $SolutionRoot "src\$ProjectName"
$OutputDir = Join-Path $ProjectDir "bin\$Configuration"
$DllPath = Join-Path $OutputDir "$ProjectName.dll"
$PdbPath = Join-Path $OutputDir "$ProjectName.pdb"
$PkgDefPath = Join-Path $OutputDir "$ProjectName.pkgdef"
$SourceManifestPath = Join-Path $ProjectDir "source.extension.vsixmanifest"
$VsixPath = Join-Path $OutputDir "$ProjectName.vsix"
$StagingDir = Join-Path $ProjectDir "obj\vsix\$Configuration\manual"

if (-not (Test-Path $DllPath)) {
    throw "No existe el DLL compilado: $DllPath"
}

if (-not (Test-Path $SourceManifestPath)) {
    throw "No existe el manifiesto VSIX fuente: $SourceManifestPath"
}

if (Test-Path $StagingDir) {
    Remove-Item $StagingDir -Recurse -Force
}

New-Item -ItemType Directory -Path $StagingDir | Out-Null

Copy-Item $DllPath -Destination (Join-Path $StagingDir "$ProjectName.dll") -Force

if (-not (Test-Path $PkgDefPath)) {
    throw "No existe el pkgdef generado: $PkgDefPath"
}

Copy-Item $PkgDefPath -Destination (Join-Path $StagingDir "$ProjectName.pkgdef") -Force

if (Test-Path $PdbPath) {
    Copy-Item $PdbPath -Destination (Join-Path $StagingDir "$ProjectName.pdb") -Force
}


# Copia archivos de metadatos visuales y documentacion incluidos en el manifiesto VSIX.
$metadataFiles = @(
    "Resources\Icon128.png",
    "Resources\Preview200.png",
    "Resources\SourcePath.Icon128.svg",
    "Resources\SourcePath.Preview200.svg",
    "Docs\README.txt",
    "Docs\CHANGELOG.txt",
    "Docs\LICENSE.txt",
    "Docs\DONATIONS.md",
    "README.md"
)

foreach ($relativeFile in $metadataFiles) {
    $sourceFile = Join-Path $ProjectDir $relativeFile
    if (Test-Path $sourceFile) {
        $destinationFile = Join-Path $StagingDir $relativeFile
        $destinationDir = Split-Path -Parent $destinationFile
        if (-not (Test-Path $destinationDir)) {
            New-Item -ItemType Directory -Path $destinationDir -Force | Out-Null
        }
        Copy-Item $sourceFile -Destination $destinationFile -Force
    }
}

[xml]$manifest = Get-Content -Raw -Path $SourceManifestPath
$ns = New-Object System.Xml.XmlNamespaceManager($manifest.NameTable)
$ns.AddNamespace("vsx", "http://schemas.microsoft.com/developer/vsx-schema/2011")
$ns.AddNamespace("d", "http://schemas.microsoft.com/developer/vsx-schema-design/2011")

$asset = $manifest.SelectSingleNode("//vsx:Assets/vsx:Asset[@Type='Microsoft.VisualStudio.MefComponent']", $ns)
if ($null -eq $asset) {
    $assets = $manifest.SelectSingleNode("//vsx:Assets", $ns)
    if ($null -eq $assets) {
        $assets = $manifest.CreateElement("Assets", "http://schemas.microsoft.com/developer/vsx-schema/2011")
        $manifest.PackageManifest.AppendChild($assets) | Out-Null
    }

    $asset = $manifest.CreateElement("Asset", "http://schemas.microsoft.com/developer/vsx-schema/2011")
    $asset.SetAttribute("Type", "Microsoft.VisualStudio.MefComponent")
    $assets.AppendChild($asset) | Out-Null
}

$asset.SetAttribute("Path", "$ProjectName.dll")
$asset.RemoveAttribute("Source", "http://schemas.microsoft.com/developer/vsx-schema-design/2011")
$asset.RemoveAttribute("ProjectName", "http://schemas.microsoft.com/developer/vsx-schema-design/2011")

$vsPackageAsset = $manifest.SelectSingleNode("//vsx:Assets/vsx:Asset[@Type='Microsoft.VisualStudio.VsPackage']", $ns)
if ($null -eq $vsPackageAsset) {
    $assets = $manifest.SelectSingleNode("//vsx:Assets", $ns)
    if ($null -eq $assets) {
        $assets = $manifest.CreateElement("Assets", "http://schemas.microsoft.com/developer/vsx-schema/2011")
        $manifest.PackageManifest.AppendChild($assets) | Out-Null
    }

    $vsPackageAsset = $manifest.CreateElement("Asset", "http://schemas.microsoft.com/developer/vsx-schema/2011")
    $vsPackageAsset.SetAttribute("Type", "Microsoft.VisualStudio.VsPackage")
    $assets.AppendChild($vsPackageAsset) | Out-Null
}

$vsPackageAsset.SetAttribute("Path", "$ProjectName.pkgdef")
$vsPackageAsset.RemoveAttribute("Source", "http://schemas.microsoft.com/developer/vsx-schema-design/2011")
$vsPackageAsset.RemoveAttribute("ProjectName", "http://schemas.microsoft.com/developer/vsx-schema-design/2011")

$prerequisites = $manifest.SelectSingleNode("//vsx:Prerequisites", $ns)
if ($null -eq $prerequisites) {
    $prerequisites = $manifest.CreateElement("Prerequisites", "http://schemas.microsoft.com/developer/vsx-schema/2011")
    $installation = $manifest.SelectSingleNode("//vsx:Installation", $ns)
    if ($null -ne $installation -and $null -ne $installation.NextSibling) {
        $manifest.PackageManifest.InsertAfter($prerequisites, $installation) | Out-Null
    }
    else {
        $manifest.PackageManifest.AppendChild($prerequisites) | Out-Null
    }
}

$coreEditor = $manifest.SelectSingleNode("//vsx:Prerequisite[@Id='Microsoft.VisualStudio.Component.CoreEditor']", $ns)
if ($null -eq $coreEditor) {
    $coreEditor = $manifest.CreateElement("Prerequisite", "http://schemas.microsoft.com/developer/vsx-schema/2011")
    $coreEditor.SetAttribute("Id", "Microsoft.VisualStudio.Component.CoreEditor")
    $coreEditor.SetAttribute("Version", "[17.0,19.0)")
    $coreEditor.SetAttribute("DisplayName", "Visual Studio core editor")
    $prerequisites.AppendChild($coreEditor) | Out-Null
}

$FinalManifestPath = Join-Path $StagingDir "extension.vsixmanifest"
$settings = New-Object System.Xml.XmlWriterSettings
$settings.Indent = $true
$settings.Encoding = New-Object System.Text.UTF8Encoding($false)
$writer = [System.Xml.XmlWriter]::Create($FinalManifestPath, $settings)
try {
    $manifest.Save($writer)
}
finally {
    $writer.Close()
}

$contentTypesPath = Join-Path $StagingDir "[Content_Types].xml"
$contentTypesXml = @'
<?xml version="1.0" encoding="utf-8"?>
<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
  <Default Extension="dll" ContentType="application/octet-stream" />
  <Default Extension="pdb" ContentType="application/octet-stream" />
  <Default Extension="pkgdef" ContentType="text/plain" />
  <Default Extension="vsixmanifest" ContentType="text/xml" />
  <Default Extension="png" ContentType="image/png" />
  <Default Extension="md" ContentType="text/markdown" />
  <Default Extension="txt" ContentType="text/plain" />
  <Default Extension="svg" ContentType="image/svg+xml" />
</Types>
'@
[System.IO.File]::WriteAllText(
    $contentTypesPath,
    $contentTypesXml,
    (New-Object System.Text.UTF8Encoding($false)))

if (Test-Path $VsixPath) {
    Remove-Item $VsixPath -Force
}

$tempZip = Join-Path $OutputDir "$ProjectName.zip"
if (Test-Path $tempZip) {
    Remove-Item $tempZip -Force
}

Push-Location $StagingDir
try {
    Compress-Archive -Path * -DestinationPath $tempZip -Force
}
finally {
    Pop-Location
}

Rename-Item -Path $tempZip -NewName "$ProjectName.vsix" -Force

if (-not (Test-Path $VsixPath)) {
    throw "No se pudo crear el VSIX: $VsixPath"
}

Write-Host "VSIX generado manualmente:" -ForegroundColor Green
Write-Host $VsixPath -ForegroundColor Green
