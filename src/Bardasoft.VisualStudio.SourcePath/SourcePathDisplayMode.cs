#nullable enable

namespace Bardasoft.VisualStudio.SourcePath;

/// <summary>
/// Define como se muestra la ruta del archivo activo en el footer.
/// </summary>
public enum SourcePathDisplayMode
{
    FullPath = 0,
    RelativeToSolution = 1,
    FileName = 2
}
