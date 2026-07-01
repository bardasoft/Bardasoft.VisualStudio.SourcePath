#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace Bardasoft.VisualStudio.SourcePath;

[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[InstalledProductRegistration("SourcePath", "Muestra la ruta del archivo activo en el editor.", "2.1.0")]
[ProvideOptionPage(typeof(SourcePathOptionsPage), "SourcePath", "General", 0, 0, true)]
[ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string, PackageAutoLoadFlags.BackgroundLoad)]
[Guid(PackageGuidString)]
public sealed class SourcePathPackage : AsyncPackage
{
    public const string PackageGuidString = "e29cfc9d-f42f-43ee-a7b5-e292d15ae5cb";

    protected override async Task InitializeAsync(
        CancellationToken cancellationToken,
        IProgress<ServiceProgressData> progress)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var optionsPage = (SourcePathOptionsPage)GetDialogPage(typeof(SourcePathOptionsPage));
        SourcePathOptionsState.Update(SourcePathOptionsSnapshot.FromPage(optionsPage));
    }
}
