#nullable enable

using System;
using System.IO;

namespace Bardasoft.VisualStudio.SourcePath;

internal static class SourcePathPathFormatter
{
    public static string Format(
        string filePath,
        SourcePathDisplayMode displayMode,
        string? solutionDirectory)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return string.Empty;
        }

        switch (displayMode)
        {
            case SourcePathDisplayMode.FileName:
                return Path.GetFileName(filePath);

            case SourcePathDisplayMode.RelativeToSolution:
                return TryMakeRelativePath(solutionDirectory, filePath) ?? filePath;

            default:
                return filePath;
        }
    }

    public static string? TryMakeRelativePath(string? baseDirectory, string filePath)
    {
        if (string.IsNullOrWhiteSpace(baseDirectory) ||
            string.IsNullOrWhiteSpace(filePath))
        {
            return null;
        }

        try
        {
            var baseUri = new Uri(EnsureTrailingDirectorySeparator(baseDirectory!));
            var fileUri = new Uri(filePath);

            if (!baseUri.IsBaseOf(fileUri))
            {
                return null;
            }

            string relativePath = Uri.UnescapeDataString(
                baseUri.MakeRelativeUri(fileUri).ToString());

            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }
        catch (UriFormatException)
        {
            return null;
        }
        catch (NotSupportedException)
        {
            return null;
        }
    }

    private static string EnsureTrailingDirectorySeparator(string directoryPath)
    {
        if (directoryPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) ||
            directoryPath.EndsWith(Path.AltDirectorySeparatorChar.ToString(), StringComparison.Ordinal))
        {
            return directoryPath;
        }

        return directoryPath + Path.DirectorySeparatorChar;
    }
}
