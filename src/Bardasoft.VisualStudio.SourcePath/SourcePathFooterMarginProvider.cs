#nullable enable

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Bardasoft.VisualStudio.SourcePath;

/// <summary>
/// Proveedor MEF que registra un margen inferior dentro del editor de texto de Visual Studio.
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
                "El host del editor no contiene una vista de texto válida.",
                nameof(wpfTextViewHost));
        }

        if (TextDocumentFactoryService is null)
        {
            throw new InvalidOperationException(
                "No se pudo importar ITextDocumentFactoryService desde MEF.");
        }

        return new SourcePathFooterMargin(
            wpfTextViewHost.TextView,
            TextDocumentFactoryService);
    }
}
