#nullable enable

using System;

namespace Bardasoft.VisualStudio.SourcePath;

internal sealed class SourcePathOptionsSnapshot
{
    public static SourcePathOptionsSnapshot Default { get; } = new SourcePathOptionsSnapshot(
        isFooterVisible: true,
        fontFamily: "Segoe UI",
        fontSize: 11,
        footerHeight: 24,
        horizontalPadding: 8,
        displayMode: SourcePathDisplayMode.FullPath,
        showActionButtons: true,
        enableCtrlClickOpen: true);

    public SourcePathOptionsSnapshot(
        bool isFooterVisible,
        string fontFamily,
        double fontSize,
        double footerHeight,
        double horizontalPadding,
        SourcePathDisplayMode displayMode,
        bool showActionButtons,
        bool enableCtrlClickOpen)
    {
        IsFooterVisible = isFooterVisible;
        FontFamily = string.IsNullOrWhiteSpace(fontFamily)
            ? "Segoe UI"
            : fontFamily.Trim();
        FontSize = Clamp(fontSize, 8, 24);
        FooterHeight = Clamp(footerHeight, 18, 48);
        HorizontalPadding = Clamp(horizontalPadding, 0, 24);
        DisplayMode = displayMode;
        ShowActionButtons = showActionButtons;
        EnableCtrlClickOpen = enableCtrlClickOpen;
    }

    public bool IsFooterVisible { get; }

    public string FontFamily { get; }

    public double FontSize { get; }

    public double FooterHeight { get; }

    public double HorizontalPadding { get; }

    public SourcePathDisplayMode DisplayMode { get; }

    public bool ShowActionButtons { get; }

    public bool EnableCtrlClickOpen { get; }

    public static SourcePathOptionsSnapshot FromPage(SourcePathOptionsPage page)
    {
        if (page is null)
        {
            throw new ArgumentNullException(nameof(page));
        }

        return new SourcePathOptionsSnapshot(
            page.IsFooterVisible,
            page.FontFamily,
            page.FontSize,
            page.FooterHeight,
            page.HorizontalPadding,
            page.DisplayMode,
            page.ShowActionButtons,
            page.EnableCtrlClickOpen);
    }

    private static double Clamp(double value, double min, double max)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            return min;
        }

        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }
}
