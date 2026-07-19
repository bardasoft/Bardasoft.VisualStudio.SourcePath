#nullable enable

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Bardasoft.VisualStudio.SourcePath;

/// <summary>
/// MEF provider that registers a bottom margin in the Visual Studio text editor.
/// </summary>
[Export(typeof(IWpfTextViewMarginProvider))]
[Name(SourcePathFooterMargin.MarginName)]
[MarginContainer(PredefinedMarginNames.Bottom)]
[Order(After = PredefinedMarginNames.HorizontalScrollBar)]
[ContentType("text")]
[TextViewRole(PredefinedTextViewRoles.Document)]
internal sealed class SourcePathFooterMarginProvider : IWpfTextViewMarginProvider
{
    [Import]
    internal ITextDocumentFactoryService TextDocumentFactoryService { get; set; } = null!;

    [Import(typeof(SVsServiceProvider))]
    internal IServiceProvider ServiceProvider { get; set; } = null!;

    public IWpfTextViewMargin CreateMargin(
        IWpfTextViewHost wpfTextViewHost,
        IWpfTextViewMargin marginContainer)
    {
        if (wpfTextViewHost is null)
        {
            throw new ArgumentNullException(nameof(wpfTextViewHost));
        }

        if (wpfTextViewHost.TextView is null)
        {
            throw new ArgumentException(
                SourcePathText.Get(SourcePathText.TextViewHostMissing),
                nameof(wpfTextViewHost));
        }

        if (TextDocumentFactoryService is null)
        {
            throw new InvalidOperationException(
                SourcePathText.Get(SourcePathText.TextDocumentFactoryMissing));
        }

        if (ServiceProvider is null)
        {
            throw new InvalidOperationException(
                SourcePathText.Get(SourcePathText.ServiceProviderMissing));
        }

        return new SourcePathFooterMargin(
            wpfTextViewHost.TextView,
            TextDocumentFactoryService,
            ServiceProvider);
    }
}
