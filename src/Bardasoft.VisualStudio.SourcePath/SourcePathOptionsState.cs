#nullable enable

using System;

namespace Bardasoft.VisualStudio.SourcePath;

internal static class SourcePathOptionsState
{
    private static SourcePathOptionsSnapshot _current = SourcePathOptionsSnapshot.Default;

    public static event EventHandler? Changed;

    public static SourcePathOptionsSnapshot Current => _current;

    public static void Update(SourcePathOptionsSnapshot options)
    {
        _current = options ?? SourcePathOptionsSnapshot.Default;
        Changed?.Invoke(null, EventArgs.Empty);
    }
}
