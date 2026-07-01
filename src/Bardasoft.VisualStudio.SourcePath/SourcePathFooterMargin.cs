#nullable enable

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using IoPath = System.IO.Path;
using WpfPath = System.Windows.Shapes.Path;

namespace Bardasoft.VisualStudio.SourcePath;

/// <summary>
/// Margen inferior que muestra la ruta fisica del archivo abierto en el editor.
/// </summary>
internal sealed class SourcePathFooterMargin : Border, IWpfTextViewMargin
{
    public const string MarginName = "BardasoftSourcePathMargin";

    private const string NoPhysicalPathText = "Archivo sin ruta física en disco";

    private readonly IWpfTextView _textView;
    private readonly ITextDocumentFactoryService _textDocumentFactoryService;
    private readonly IServiceProvider _serviceProvider;
    private readonly Grid _layout;
    private readonly TextBox _pathTextBox;
    private readonly StackPanel _buttonPanel;

    private ITextDocument? _textDocument;
    private bool _isDisposed;
    private SourcePathOptionsSnapshot _options = SourcePathOptionsState.Current;

    public SourcePathFooterMargin(
        IWpfTextView textView,
        ITextDocumentFactoryService textDocumentFactoryService,
        IServiceProvider serviceProvider)
    {
        _textView = textView ?? throw new ArgumentNullException(nameof(textView));
        _textDocumentFactoryService = textDocumentFactoryService
            ?? throw new ArgumentNullException(nameof(textDocumentFactoryService));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        SnapsToDevicePixels = true;
        ClipToBounds = true;

        SetResourceReference(
            BackgroundProperty,
            EnvironmentColors.ToolWindowBackgroundBrushKey);

        _layout = new Grid
        {
            SnapsToDevicePixels = true
        };

        _layout.ColumnDefinitions.Add(new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        });
        _layout.ColumnDefinitions.Add(new ColumnDefinition
        {
            Width = GridLength.Auto
        });

        _pathTextBox = CreatePathTextBox();
        _buttonPanel = CreateButtonPanel();

        Grid.SetColumn(_pathTextBox, 0);
        Grid.SetColumn(_buttonPanel, 1);
        _layout.Children.Add(_pathTextBox);
        _layout.Children.Add(_buttonPanel);

        Child = _layout;
        ContextMenu = CreateContextMenu();

        AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnFooterMouseLeftButtonUp), true);
        SourcePathOptionsState.Changed += OnOptionsChanged;

        TryAttachTextDocument();
        ApplyOptions();
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
        SourcePathOptionsState.Changed -= OnOptionsChanged;
        RemoveHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnFooterMouseLeftButtonUp));
        DetachTextDocument();

        ContextMenu = null;
        Child = null;
    }

    private TextBox CreatePathTextBox()
    {
        var textBox = new TextBox
        {
            IsReadOnly = true,
            IsReadOnlyCaretVisible = false,
            BorderThickness = new Thickness(0),
            Background = Brushes.Transparent,
            CaretBrush = Brushes.Transparent,
            FocusVisualStyle = null,
            VerticalAlignment = VerticalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            TextWrapping = TextWrapping.NoWrap,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
            VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
            MinWidth = 0,
            Padding = new Thickness(0),
            IsTabStop = false
        };

        textBox.Template = CreateFlatTextBoxTemplate();

        textBox.SetResourceReference(
            Control.ForegroundProperty,
            EnvironmentColors.ToolWindowTextBrushKey);

        textBox.ContextMenu = CreateContextMenu();

        return textBox;
    }

    private static ControlTemplate CreateFlatTextBoxTemplate()
    {
        var contentHost = new FrameworkElementFactory(typeof(ScrollViewer));
        contentHost.Name = "PART_ContentHost";
        contentHost.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
        contentHost.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
        contentHost.SetValue(ScrollViewer.BackgroundProperty, Brushes.Transparent);

        return new ControlTemplate(typeof(TextBox))
        {
            VisualTree = contentHost
        };
    }

    private StackPanel CreateButtonPanel()
    {
        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center
        };

        panel.Children.Add(CreateIconButton(
            "Copiar ruta completa",
            IconGeometry.Copy,
            (_, _) => CopyFullPath()));

        panel.Children.Add(CreateIconButton(
            "Copiar nombre del archivo",
            IconGeometry.File,
            (_, _) => CopyFileName()));

        panel.Children.Add(CreateIconButton(
            "Copiar carpeta",
            IconGeometry.Folder,
            (_, _) => CopyFolderPath()));

        panel.Children.Add(CreateIconButton(
            "Abrir ubicacion en el Explorador",
            IconGeometry.OpenFolder,
            (_, _) => OpenContainingFolder()));

        return panel;
    }

    private static Button CreateIconButton(
        string toolTip,
        Geometry iconGeometry,
        RoutedEventHandler clickHandler)
    {
        var icon = new WpfPath
        {
            Data = iconGeometry,
            Width = 13,
            Height = 13,
            Stretch = Stretch.Uniform,
            StrokeThickness = 1.35,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round,
            StrokeLineJoin = PenLineJoin.Round,
            Fill = Brushes.Transparent,
            SnapsToDevicePixels = true
        };

        icon.SetResourceReference(
            Shape.StrokeProperty,
            EnvironmentColors.ToolWindowTextBrushKey);

        var button = new Button
        {
            Width = 22,
            Height = 20,
            Padding = new Thickness(0),
            Margin = new Thickness(2, 0, 0, 0),
            BorderThickness = new Thickness(0),
            Background = Brushes.Transparent,
            Content = icon,
            Focusable = false,
            ToolTip = toolTip
        };

        button.Click += clickHandler;

        return button;
    }

    private void OnFooterMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_isDisposed || !_options.EnableCtrlClickOpen)
        {
            return;
        }

        if (e.OriginalSource is DependencyObject source &&
            IsInsideButton(source))
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

    private static bool IsInsideButton(DependencyObject source)
    {
        DependencyObject? current = source;

        while (current is not null)
        {
            if (current is Button)
            {
                return true;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return false;
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

    private void OnOptionsChanged(object? sender, EventArgs e)
    {
        if (_isDisposed)
        {
            return;
        }

        _options = SourcePathOptionsState.Current;
        ApplyOptions();
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

    private void ApplyOptions()
    {
        Height = _options.FooterHeight;
        MinHeight = _options.FooterHeight;
        Padding = new Thickness(_options.HorizontalPadding, 2, _options.HorizontalPadding, 2);
        Visibility = _options.IsFooterVisible ? Visibility.Visible : Visibility.Collapsed;

        _pathTextBox.FontFamily = new FontFamily(_options.FontFamily);
        _pathTextBox.FontSize = _options.FontSize;
        _buttonPanel.Visibility = _options.ShowActionButtons
            ? Visibility.Visible
            : Visibility.Collapsed;
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
            _pathTextBox.Text = NoPhysicalPathText;
            _pathTextBox.ToolTip = NoPhysicalPathText;
            return;
        }

        string displayPath = SourcePathPathFormatter.Format(
            filePath!,
            _options.DisplayMode,
            GetSolutionDirectory());
        _pathTextBox.Text = displayPath;
        _pathTextBox.ToolTip = filePath;
    }

    private string? GetSolutionDirectory()
    {
        if (!ThreadHelper.CheckAccess())
        {
            return null;
        }

        ThreadHelper.ThrowIfNotOnUIThread();

        if (_serviceProvider.GetService(typeof(SVsSolution)) is not IVsSolution solution)
        {
            return null;
        }

        int result = solution.GetSolutionInfo(
            out string solutionDirectory,
            out _,
            out _);

        if (result < 0 || string.IsNullOrWhiteSpace(solutionDirectory))
        {
            return null;
        }

        return solutionDirectory;
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
            "Abrir ubicacion en el Explorador",
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

        string fileName = IoPath.GetFileName(filePath);

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

        string? folderPath = IoPath.GetDirectoryName(filePath);

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
            string? folderPath = IoPath.GetDirectoryName(filePath);

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

    private static class IconGeometry
    {
        public static Geometry Copy { get; } = Geometry.Parse(
            "M5,1.5 H12.5 V9 H10.5 M3,4 H10 V13 H3 Z");

        public static Geometry File { get; } = Geometry.Parse(
            "M4,1.5 H9 L12,4.5 V13 H4 Z M9,1.5 V4.5 H12");

        public static Geometry Folder { get; } = Geometry.Parse(
            "M2,4.5 H5.5 L7,6 H14 V12.5 H2 Z");

        public static Geometry OpenFolder { get; } = Geometry.Parse(
            "M2,5 H5.5 L7,6.5 H14 M2,5 V13 H12.5 L14,7 H4 L2,13 M8.5,8.5 H11.5 M11.5,8.5 V11.5");
    }
}
