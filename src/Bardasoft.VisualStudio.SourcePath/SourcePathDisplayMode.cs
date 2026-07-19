#nullable enable

using System.ComponentModel;

namespace Bardasoft.VisualStudio.SourcePath;

/// <summary>
/// Defines how the active file path is displayed in the footer.
/// </summary>
[TypeConverter(typeof(SourcePathDisplayModeConverter))]
public enum SourcePathDisplayMode
{
    FullPath = 0,
    RelativeToSolution = 1,
    FileName = 2
}
