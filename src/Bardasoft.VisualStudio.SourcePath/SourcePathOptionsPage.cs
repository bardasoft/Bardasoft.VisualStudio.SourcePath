#nullable enable

using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace Bardasoft.VisualStudio.SourcePath;

public sealed class SourcePathOptionsPage : DialogPage
{
    [SourcePathCategory(SourcePathText.AppearanceCategory)]
    [SourcePathDisplayName(SourcePathText.ShowFooterName)]
    [SourcePathDescription(SourcePathText.ShowFooterDescription)]
    [DefaultValue(true)]
    public bool IsFooterVisible { get; set; } = true;

    [SourcePathCategory(SourcePathText.AppearanceCategory)]
    [SourcePathDisplayName(SourcePathText.FontFamilyName)]
    [SourcePathDescription(SourcePathText.FontFamilyDescription)]
    [DefaultValue("Segoe UI")]
    public string FontFamily { get; set; } = "Segoe UI";

    [SourcePathCategory(SourcePathText.AppearanceCategory)]
    [SourcePathDisplayName(SourcePathText.FontSizeName)]
    [SourcePathDescription(SourcePathText.FontSizeDescription)]
    [DefaultValue(11d)]
    public double FontSize { get; set; } = 11;

    [SourcePathCategory(SourcePathText.AppearanceCategory)]
    [SourcePathDisplayName(SourcePathText.FooterHeightName)]
    [SourcePathDescription(SourcePathText.FooterHeightDescription)]
    [DefaultValue(24d)]
    public double FooterHeight { get; set; } = 24;

    [SourcePathCategory(SourcePathText.AppearanceCategory)]
    [SourcePathDisplayName(SourcePathText.HorizontalPaddingName)]
    [SourcePathDescription(SourcePathText.HorizontalPaddingDescription)]
    [DefaultValue(8d)]
    public double HorizontalPadding { get; set; } = 8;

    [SourcePathCategory(SourcePathText.PathCategory)]
    [SourcePathDisplayName(SourcePathText.DisplayModeName)]
    [SourcePathDescription(SourcePathText.DisplayModeDescription)]
    [DefaultValue(SourcePathDisplayMode.FullPath)]
    public SourcePathDisplayMode DisplayMode { get; set; } = SourcePathDisplayMode.FullPath;

    [SourcePathCategory(SourcePathText.ActionsCategory)]
    [SourcePathDisplayName(SourcePathText.ShowActionButtonsName)]
    [SourcePathDescription(SourcePathText.ShowActionButtonsDescription)]
    [DefaultValue(true)]
    public bool ShowActionButtons { get; set; } = true;

    [SourcePathCategory(SourcePathText.ActionsCategory)]
    [SourcePathDisplayName(SourcePathText.EnableCtrlClickOpenName)]
    [SourcePathDescription(SourcePathText.EnableCtrlClickOpenDescription)]
    [DefaultValue(true)]
    public bool EnableCtrlClickOpen { get; set; } = true;

    protected override void OnActivate(CancelEventArgs e)
    {
        base.OnActivate(e);
        SourcePathOptionsState.Update(SourcePathOptionsSnapshot.FromPage(this));
    }

    protected override void OnApply(PageApplyEventArgs e)
    {
        base.OnApply(e);
        SourcePathOptionsState.Update(SourcePathOptionsSnapshot.FromPage(this));
    }
}
