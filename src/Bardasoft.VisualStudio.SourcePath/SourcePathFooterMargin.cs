#nullable enable

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Bardasoft.VisualStudio.SourcePath;

/// <summary>
/// Margen inferior que muestra la ruta física completa del archivo abierto en el editor.
/// </summary>
internal sealed class SourcePathFooterMargin : Border, IWpfTextViewMargin
{
    public const string MarginName = "BardasoftSourcePathMargin";

    private const string NoPhysicalPathText = "Archivo sin ruta física en disco";

    private readonly IWpfTextView _textView;
    private readonly ITextDocumentFactoryService _textDocumentFactoryService;
    private readonly TextBlock _textBlock;

    private ITextDocument? _textDocument;
    private bool _isDisposed;

    public SourcePathFooterMargin(
        IWpfTextView textView,
        ITextDocumentFactoryService textDocumentFactoryService)
    {
        _textView = textView ?? throw new ArgumentNullException(nameof(textView));
        _textDocumentFactoryService = textDocumentFactoryService
            ?? throw new ArgumentNullException(nameof(textDocumentFactoryService));

        Height = 24;
        MinHeight = 24;
        Padding = new Thickness(8, 2, 8, 2);
        SnapsToDevicePixels = true;
        ClipToBounds = true;

        SetResourceReference(
            BackgroundProperty,
            EnvironmentColors.ToolWindowBackgroundBrushKey);

        _textBlock = new TextBlock
        {
            VerticalAlignment = VerticalAlignment.Center,
            TextTrimming = TextTrimming.CharacterEllipsis,
            TextWrapping = TextWrapping.NoWrap,
            FontSize = 11
        };

        _textBlock.SetResourceReference(
            TextBlock.ForegroundProperty,
            EnvironmentColors.ToolWindowTextBrushKey);

        Child = _textBlock;
        ContextMenu = CreateContextMenu();

        TryAttachTextDocument();
        UpdateDisplayedPath();

        _textView.Closed += OnTextViewClosed;
    }

    public FrameworkElement VisualElement => this;

    public double MarginSize => ActualHeight > 0 ? ActualHeight : Height;

    public bool Enabled => !_isDisposed && Visibility == Visibility.Visible;

    public ITextViewMargin? GetTextViewMargin(string marginName)
    {
        if (string.Equals(marginName, MarginName, StringComparison.OrdinalIgnoreCase))
        {
            return this;
        }

        return null;
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        _textView.Closed -= OnTextViewClosed;
        DetachTextDocument();

        ContextMenu = null;
        Child = null;
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        if (_isDisposed)
        {
            return;
        }

        bool ctrlPressed = Keyboard.Modifiers.HasFlag(ModifierKeys.Control);

        if (!ctrlPressed)
        {
            return;
        }

        string? filePath = GetCurrentFilePath();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            return;
        }

        OpenInExplorer(filePath);
        e.Handled = true;
    }

    private void OnTextViewClosed(object sender, EventArgs e)
    {
        Dispose();
    }

    private void OnFileActionOccurred(
        object sender,
        TextDocumentFileActionEventArgs e)
    {
        UpdateDisplayedPath();
    }

    private void TryAttachTextDocument()
    {
        DetachTextDocument();

        ITextBuffer documentBuffer = _textView.TextDataModel.DocumentBuffer;

        if (_textDocumentFactoryService.TryGetTextDocument(
                documentBuffer,
                out ITextDocument textDocument))
        {
            _textDocument = textDocument;
            _textDocument.FileActionOccurred += OnFileActionOccurred;
        }
    }

    private void DetachTextDocument()
    {
        if (_textDocument is not null)
        {
            _textDocument.FileActionOccurred -= OnFileActionOccurred;
            _textDocument = null;
        }
    }

    private void UpdateDisplayedPath()
    {
        if (_isDisposed)
        {
            return;
        }

        string? filePath = GetCurrentFilePath();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            _textBlock.Text = NoPhysicalPathText;
            _textBlock.ToolTip = NoPhysicalPathText;
            return;
        }

        _textBlock.Text = filePath;
        _textBlock.ToolTip = filePath;
    }

    private string? GetCurrentFilePath()
    {
        if (_isDisposed)
        {
            return null;
        }

        if (_textDocument is not null &&
            !string.IsNullOrWhiteSpace(_textDocument.FilePath))
        {
            return _textDocument.FilePath;
        }

        ITextBuffer documentBuffer = _textView.TextDataModel.DocumentBuffer;

        if (_textDocumentFactoryService.TryGetTextDocument(
                documentBuffer,
                out ITextDocument textDocument) &&
            !string.IsNullOrWhiteSpace(textDocument.FilePath))
        {
            if (!ReferenceEquals(_textDocument, textDocument))
            {
                DetachTextDocument();
                _textDocument = textDocument;
                _textDocument.FileActionOccurred += OnFileActionOccurred;
            }

            return textDocument.FilePath;
        }

        return null;
    }

    private ContextMenu CreateContextMenu()
    {
        var contextMenu = new ContextMenu();

        contextMenu.Items.Add(CreateMenuItem(
            "Copiar ruta completa",
            (_, _) => CopyFullPath()));

        contextMenu.Items.Add(CreateMenuItem(
            "Copiar nombre del archivo",
            (_, _) => CopyFileName()));

        contextMenu.Items.Add(CreateMenuItem(
            "Copiar carpeta",
            (_, _) => CopyFolderPath()));

        contextMenu.Items.Add(new Separator());

        contextMenu.Items.Add(CreateMenuItem(
            "Abrir ubicación en el Explorador",
            (_, _) => OpenContainingFolder()));

        return contextMenu;
    }

    private static MenuItem CreateMenuItem(
        string header,
        RoutedEventHandler clickHandler)
    {
        var item = new MenuItem
        {
            Header = header
        };

        item.Click += clickHandler;

        return item;
    }

    private void CopyFullPath()
    {
        string? filePath = GetCurrentFilePath();

        if (!string.IsNullOrWhiteSpace(filePath))
        {
            Clipboard.SetText(filePath);
        }
    }

    private void CopyFileName()
    {
        string? filePath = GetCurrentFilePath();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            return;
        }

        string fileName = Path.GetFileName(filePath);

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            Clipboard.SetText(fileName);
        }
    }

    private void CopyFolderPath()
    {
        string? filePath = GetCurrentFilePath();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            return;
        }

        string? folderPath = Path.GetDirectoryName(filePath);

        if (!string.IsNullOrWhiteSpace(folderPath))
        {
            Clipboard.SetText(folderPath);
        }
    }

    private void OpenContainingFolder()
    {
        string? filePath = GetCurrentFilePath();

        if (!string.IsNullOrWhiteSpace(filePath))
        {
            OpenInExplorer(filePath);
        }
    }

    private static void OpenInExplorer(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return;
        }

        string? argument = null;

        if (File.Exists(filePath))
        {
            argument = "/select,\"" + filePath + "\"";
        }
        else if (Directory.Exists(filePath))
        {
            argument = "\"" + filePath + "\"";
        }
        else
        {
            string? folderPath = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(folderPath) && Directory.Exists(folderPath))
            {
                argument = "\"" + folderPath + "\"";
            }
        }

        if (string.IsNullOrWhiteSpace(argument))
        {
            return;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = "explorer.exe",
            Arguments = argument,
            UseShellExecute = true
        };

        Process.Start(startInfo);
    }
}
