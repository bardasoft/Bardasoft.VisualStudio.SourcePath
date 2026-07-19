#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Bardasoft.VisualStudio.SourcePath;

internal static class SourcePathText
{
    public const string NoPhysicalPath = nameof(NoPhysicalPath);
    public const string CopyFullPath = nameof(CopyFullPath);
    public const string CopyFileName = nameof(CopyFileName);
    public const string CopyFolderPath = nameof(CopyFolderPath);
    public const string OpenLocationInExplorer = nameof(OpenLocationInExplorer);
    public const string AppearanceCategory = nameof(AppearanceCategory);
    public const string PathCategory = nameof(PathCategory);
    public const string ActionsCategory = nameof(ActionsCategory);
    public const string ShowFooterName = nameof(ShowFooterName);
    public const string ShowFooterDescription = nameof(ShowFooterDescription);
    public const string FontFamilyName = nameof(FontFamilyName);
    public const string FontFamilyDescription = nameof(FontFamilyDescription);
    public const string FontSizeName = nameof(FontSizeName);
    public const string FontSizeDescription = nameof(FontSizeDescription);
    public const string FooterHeightName = nameof(FooterHeightName);
    public const string FooterHeightDescription = nameof(FooterHeightDescription);
    public const string HorizontalPaddingName = nameof(HorizontalPaddingName);
    public const string HorizontalPaddingDescription = nameof(HorizontalPaddingDescription);
    public const string DisplayModeName = nameof(DisplayModeName);
    public const string DisplayModeDescription = nameof(DisplayModeDescription);
    public const string ShowActionButtonsName = nameof(ShowActionButtonsName);
    public const string ShowActionButtonsDescription = nameof(ShowActionButtonsDescription);
    public const string EnableCtrlClickOpenName = nameof(EnableCtrlClickOpenName);
    public const string EnableCtrlClickOpenDescription = nameof(EnableCtrlClickOpenDescription);
    public const string DisplayModeFullPath = nameof(DisplayModeFullPath);
    public const string DisplayModeRelativeToSolution = nameof(DisplayModeRelativeToSolution);
    public const string DisplayModeFileName = nameof(DisplayModeFileName);
    public const string TextViewHostMissing = nameof(TextViewHostMissing);
    public const string TextDocumentFactoryMissing = nameof(TextDocumentFactoryMissing);
    public const string ServiceProviderMissing = nameof(ServiceProviderMissing);

    private static readonly string[] RequiredKeys =
    {
        NoPhysicalPath,
        CopyFullPath,
        CopyFileName,
        CopyFolderPath,
        OpenLocationInExplorer,
        AppearanceCategory,
        PathCategory,
        ActionsCategory,
        ShowFooterName,
        ShowFooterDescription,
        FontFamilyName,
        FontFamilyDescription,
        FontSizeName,
        FontSizeDescription,
        FooterHeightName,
        FooterHeightDescription,
        HorizontalPaddingName,
        HorizontalPaddingDescription,
        DisplayModeName,
        DisplayModeDescription,
        ShowActionButtonsName,
        ShowActionButtonsDescription,
        EnableCtrlClickOpenName,
        EnableCtrlClickOpenDescription,
        DisplayModeFullPath,
        DisplayModeRelativeToSolution,
        DisplayModeFileName,
        TextViewHostMissing,
        TextDocumentFactoryMissing,
        ServiceProviderMissing
    };

    private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Translations =
        new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["en"] = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [NoPhysicalPath] = "File has no physical path on disk",
                [CopyFullPath] = "Copy full path",
                [CopyFileName] = "Copy file name",
                [CopyFolderPath] = "Copy folder path",
                [OpenLocationInExplorer] = "Open location in Explorer",
                [AppearanceCategory] = "Appearance",
                [PathCategory] = "Path",
                [ActionsCategory] = "Actions",
                [ShowFooterName] = "Show footer",
                [ShowFooterDescription] = "Shows or hides the SourcePath footer in the editor.",
                [FontFamilyName] = "Font family",
                [FontFamilyDescription] = "Font family used by the footer.",
                [FontSizeName] = "Font size",
                [FontSizeDescription] = "Footer font size. Recommended range: 8 to 24.",
                [FooterHeightName] = "Footer height",
                [FooterHeightDescription] = "Footer height in pixels. Recommended range: 18 to 48.",
                [HorizontalPaddingName] = "Horizontal padding",
                [HorizontalPaddingDescription] = "Horizontal inner spacing for the footer, in pixels.",
                [DisplayModeName] = "Path format",
                [DisplayModeDescription] = "Controls whether the footer shows the full path, the solution-relative path, or only the file name.",
                [ShowActionButtonsName] = "Show buttons",
                [ShowActionButtonsDescription] = "Shows compact buttons to copy the path, copy the file name, copy the folder path, and open the location.",
                [EnableCtrlClickOpenName] = "Ctrl+Click opens location",
                [EnableCtrlClickOpenDescription] = "Allows opening the file location with Ctrl+Click on the footer.",
                [DisplayModeFullPath] = "Full path",
                [DisplayModeRelativeToSolution] = "Relative to solution",
                [DisplayModeFileName] = "File name",
                [TextViewHostMissing] = "The editor host does not contain a valid text view.",
                [TextDocumentFactoryMissing] = "ITextDocumentFactoryService could not be imported from MEF.",
                [ServiceProviderMissing] = "SVsServiceProvider could not be imported from MEF."
            },
            ["es"] = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [NoPhysicalPath] = "Archivo sin ruta física en disco",
                [CopyFullPath] = "Copiar ruta completa",
                [CopyFileName] = "Copiar nombre del archivo",
                [CopyFolderPath] = "Copiar ruta de carpeta",
                [OpenLocationInExplorer] = "Abrir ubicación en el Explorador",
                [AppearanceCategory] = "Apariencia",
                [PathCategory] = "Ruta",
                [ActionsCategory] = "Acciones",
                [ShowFooterName] = "Mostrar footer",
                [ShowFooterDescription] = "Muestra u oculta el footer de SourcePath en el editor.",
                [FontFamilyName] = "Tipo de letra",
                [FontFamilyDescription] = "Familia tipográfica usada en el footer.",
                [FontSizeName] = "Tamaño de letra",
                [FontSizeDescription] = "Tamaño de letra del footer. Rango recomendado: 8 a 24.",
                [FooterHeightName] = "Alto del footer",
                [FooterHeightDescription] = "Alto del footer en píxeles. Rango recomendado: 18 a 48.",
                [HorizontalPaddingName] = "Padding horizontal",
                [HorizontalPaddingDescription] = "Espacio horizontal interno del footer, en píxeles.",
                [DisplayModeName] = "Formato de ruta",
                [DisplayModeDescription] = "Define si el footer muestra la ruta completa, la ruta relativa a la solución o solo el nombre del archivo.",
                [ShowActionButtonsName] = "Mostrar botones",
                [ShowActionButtonsDescription] = "Muestra botones compactos para copiar ruta, copiar nombre, copiar carpeta y abrir ubicación.",
                [EnableCtrlClickOpenName] = "Ctrl+Click abre ubicación",
                [EnableCtrlClickOpenDescription] = "Permite abrir la ubicación del archivo con Ctrl+Click sobre el footer.",
                [DisplayModeFullPath] = "Ruta completa",
                [DisplayModeRelativeToSolution] = "Relativa a la solución",
                [DisplayModeFileName] = "Nombre de archivo",
                [TextViewHostMissing] = "El host del editor no contiene una vista de texto válida.",
                [TextDocumentFactoryMissing] = "No se pudo importar ITextDocumentFactoryService desde MEF.",
                [ServiceProviderMissing] = "No se pudo importar SVsServiceProvider desde MEF."
            },
            ["it"] = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [NoPhysicalPath] = "Il file non ha un percorso fisico su disco",
                [CopyFullPath] = "Copia percorso completo",
                [CopyFileName] = "Copia nome file",
                [CopyFolderPath] = "Copia percorso cartella",
                [OpenLocationInExplorer] = "Apri posizione in Explorer",
                [AppearanceCategory] = "Aspetto",
                [PathCategory] = "Percorso",
                [ActionsCategory] = "Azioni",
                [ShowFooterName] = "Mostra footer",
                [ShowFooterDescription] = "Mostra o nasconde il footer di SourcePath nell'editor.",
                [FontFamilyName] = "Tipo di carattere",
                [FontFamilyDescription] = "Famiglia di caratteri usata dal footer.",
                [FontSizeName] = "Dimensione carattere",
                [FontSizeDescription] = "Dimensione del carattere del footer. Intervallo consigliato: da 8 a 24.",
                [FooterHeightName] = "Altezza footer",
                [FooterHeightDescription] = "Altezza del footer in pixel. Intervallo consigliato: da 18 a 48.",
                [HorizontalPaddingName] = "Spaziatura orizzontale",
                [HorizontalPaddingDescription] = "Spaziatura interna orizzontale del footer, in pixel.",
                [DisplayModeName] = "Formato percorso",
                [DisplayModeDescription] = "Controlla se il footer mostra il percorso completo, il percorso relativo alla soluzione o solo il nome file.",
                [ShowActionButtonsName] = "Mostra pulsanti",
                [ShowActionButtonsDescription] = "Mostra pulsanti compatti per copiare percorso, nome file, percorso cartella e aprire la posizione.",
                [EnableCtrlClickOpenName] = "Ctrl+clic apre la posizione",
                [EnableCtrlClickOpenDescription] = "Consente di aprire la posizione del file con Ctrl+clic sul footer.",
                [DisplayModeFullPath] = "Percorso completo",
                [DisplayModeRelativeToSolution] = "Relativo alla soluzione",
                [DisplayModeFileName] = "Nome file",
                [TextViewHostMissing] = "L'host dell'editor non contiene una vista di testo valida.",
                [TextDocumentFactoryMissing] = "Impossibile importare ITextDocumentFactoryService da MEF.",
                [ServiceProviderMissing] = "Impossibile importare SVsServiceProvider da MEF."
            },
            ["pt"] = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [NoPhysicalPath] = "O arquivo não tem caminho físico no disco",
                [CopyFullPath] = "Copiar caminho completo",
                [CopyFileName] = "Copiar nome do arquivo",
                [CopyFolderPath] = "Copiar caminho da pasta",
                [OpenLocationInExplorer] = "Abrir local no Explorer",
                [AppearanceCategory] = "Aparência",
                [PathCategory] = "Caminho",
                [ActionsCategory] = "Ações",
                [ShowFooterName] = "Mostrar footer",
                [ShowFooterDescription] = "Mostra ou oculta o footer do SourcePath no editor.",
                [FontFamilyName] = "Família da fonte",
                [FontFamilyDescription] = "Família da fonte usada pelo footer.",
                [FontSizeName] = "Tamanho da fonte",
                [FontSizeDescription] = "Tamanho da fonte do footer. Intervalo recomendado: 8 a 24.",
                [FooterHeightName] = "Altura do footer",
                [FooterHeightDescription] = "Altura do footer em pixels. Intervalo recomendado: 18 a 48.",
                [HorizontalPaddingName] = "Espaçamento horizontal",
                [HorizontalPaddingDescription] = "Espaçamento interno horizontal do footer, em pixels.",
                [DisplayModeName] = "Formato do caminho",
                [DisplayModeDescription] = "Controla se o footer mostra o caminho completo, o caminho relativo à solução ou apenas o nome do arquivo.",
                [ShowActionButtonsName] = "Mostrar botões",
                [ShowActionButtonsDescription] = "Mostra botões compactos para copiar caminho, copiar nome do arquivo, copiar caminho da pasta e abrir o local.",
                [EnableCtrlClickOpenName] = "Ctrl+Clique abre o local",
                [EnableCtrlClickOpenDescription] = "Permite abrir o local do arquivo com Ctrl+Clique no footer.",
                [DisplayModeFullPath] = "Caminho completo",
                [DisplayModeRelativeToSolution] = "Relativo à solução",
                [DisplayModeFileName] = "Nome do arquivo",
                [TextViewHostMissing] = "O host do editor não contém uma exibição de texto válida.",
                [TextDocumentFactoryMissing] = "Não foi possível importar ITextDocumentFactoryService do MEF.",
                [ServiceProviderMissing] = "Não foi possível importar SVsServiceProvider do MEF."
            },
            ["fr"] = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [NoPhysicalPath] = "Le fichier n'a pas de chemin physique sur le disque",
                [CopyFullPath] = "Copier le chemin complet",
                [CopyFileName] = "Copier le nom du fichier",
                [CopyFolderPath] = "Copier le chemin du dossier",
                [OpenLocationInExplorer] = "Ouvrir l'emplacement dans l'Explorateur",
                [AppearanceCategory] = "Apparence",
                [PathCategory] = "Chemin",
                [ActionsCategory] = "Actions",
                [ShowFooterName] = "Afficher le footer",
                [ShowFooterDescription] = "Affiche ou masque le footer SourcePath dans l'éditeur.",
                [FontFamilyName] = "Police",
                [FontFamilyDescription] = "Famille de police utilisée par le footer.",
                [FontSizeName] = "Taille de police",
                [FontSizeDescription] = "Taille de police du footer. Plage recommandée : 8 à 24.",
                [FooterHeightName] = "Hauteur du footer",
                [FooterHeightDescription] = "Hauteur du footer en pixels. Plage recommandée : 18 à 48.",
                [HorizontalPaddingName] = "Marge interne horizontale",
                [HorizontalPaddingDescription] = "Espacement interne horizontal du footer, en pixels.",
                [DisplayModeName] = "Format du chemin",
                [DisplayModeDescription] = "Détermine si le footer affiche le chemin complet, le chemin relatif à la solution ou seulement le nom du fichier.",
                [ShowActionButtonsName] = "Afficher les boutons",
                [ShowActionButtonsDescription] = "Affiche des boutons compacts pour copier le chemin, copier le nom du fichier, copier le chemin du dossier et ouvrir l'emplacement.",
                [EnableCtrlClickOpenName] = "Ctrl+clic ouvre l'emplacement",
                [EnableCtrlClickOpenDescription] = "Permet d'ouvrir l'emplacement du fichier avec Ctrl+clic sur le footer.",
                [DisplayModeFullPath] = "Chemin complet",
                [DisplayModeRelativeToSolution] = "Relatif à la solution",
                [DisplayModeFileName] = "Nom du fichier",
                [TextViewHostMissing] = "L'hôte de l'éditeur ne contient pas de vue texte valide.",
                [TextDocumentFactoryMissing] = "Impossible d'importer ITextDocumentFactoryService depuis MEF.",
                [ServiceProviderMissing] = "Impossible d'importer SVsServiceProvider depuis MEF."
            },
            ["ru"] = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [NoPhysicalPath] = "У файла нет физического пути на диске",
                [CopyFullPath] = "Копировать полный путь",
                [CopyFileName] = "Копировать имя файла",
                [CopyFolderPath] = "Копировать путь к папке",
                [OpenLocationInExplorer] = "Открыть расположение в Проводнике",
                [AppearanceCategory] = "Внешний вид",
                [PathCategory] = "Путь",
                [ActionsCategory] = "Действия",
                [ShowFooterName] = "Показывать footer",
                [ShowFooterDescription] = "Показывает или скрывает footer SourcePath в редакторе.",
                [FontFamilyName] = "Шрифт",
                [FontFamilyDescription] = "Семейство шрифтов, используемое footer.",
                [FontSizeName] = "Размер шрифта",
                [FontSizeDescription] = "Размер шрифта footer. Рекомендуемый диапазон: от 8 до 24.",
                [FooterHeightName] = "Высота footer",
                [FooterHeightDescription] = "Высота footer в пикселях. Рекомендуемый диапазон: от 18 до 48.",
                [HorizontalPaddingName] = "Горизонтальный отступ",
                [HorizontalPaddingDescription] = "Внутренний горизонтальный отступ footer в пикселях.",
                [DisplayModeName] = "Формат пути",
                [DisplayModeDescription] = "Определяет, показывает ли footer полный путь, путь относительно решения или только имя файла.",
                [ShowActionButtonsName] = "Показывать кнопки",
                [ShowActionButtonsDescription] = "Показывает компактные кнопки для копирования пути, имени файла, пути к папке и открытия расположения.",
                [EnableCtrlClickOpenName] = "Ctrl+щелчок открывает расположение",
                [EnableCtrlClickOpenDescription] = "Позволяет открыть расположение файла с помощью Ctrl+щелчка по footer.",
                [DisplayModeFullPath] = "Полный путь",
                [DisplayModeRelativeToSolution] = "Относительно решения",
                [DisplayModeFileName] = "Имя файла",
                [TextViewHostMissing] = "У хоста редактора нет допустимого текстового представления.",
                [TextDocumentFactoryMissing] = "Не удалось импортировать ITextDocumentFactoryService из MEF.",
                [ServiceProviderMissing] = "Не удалось импортировать SVsServiceProvider из MEF."
            },
            ["zh"] = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [NoPhysicalPath] = "文件在磁盘上没有物理路径",
                [CopyFullPath] = "复制完整路径",
                [CopyFileName] = "复制文件名",
                [CopyFolderPath] = "复制文件夹路径",
                [OpenLocationInExplorer] = "在资源管理器中打开位置",
                [AppearanceCategory] = "外观",
                [PathCategory] = "路径",
                [ActionsCategory] = "操作",
                [ShowFooterName] = "显示 footer",
                [ShowFooterDescription] = "在编辑器中显示或隐藏 SourcePath footer。",
                [FontFamilyName] = "字体",
                [FontFamilyDescription] = "footer 使用的字体系列。",
                [FontSizeName] = "字体大小",
                [FontSizeDescription] = "footer 字体大小。建议范围：8 到 24。",
                [FooterHeightName] = "footer 高度",
                [FooterHeightDescription] = "footer 高度（像素）。建议范围：18 到 48。",
                [HorizontalPaddingName] = "水平内边距",
                [HorizontalPaddingDescription] = "footer 的水平内部间距（像素）。",
                [DisplayModeName] = "路径格式",
                [DisplayModeDescription] = "控制 footer 显示完整路径、相对于解决方案的路径，或仅显示文件名。",
                [ShowActionButtonsName] = "显示按钮",
                [ShowActionButtonsDescription] = "显示紧凑按钮，用于复制路径、复制文件名、复制文件夹路径和打开位置。",
                [EnableCtrlClickOpenName] = "Ctrl+单击打开位置",
                [EnableCtrlClickOpenDescription] = "允许在 footer 上按 Ctrl+单击打开文件位置。",
                [DisplayModeFullPath] = "完整路径",
                [DisplayModeRelativeToSolution] = "相对于解决方案",
                [DisplayModeFileName] = "文件名",
                [TextViewHostMissing] = "编辑器主机不包含有效的文本视图。",
                [TextDocumentFactoryMissing] = "无法从 MEF 导入 ITextDocumentFactoryService。",
                [ServiceProviderMissing] = "无法从 MEF 导入 SVsServiceProvider。"
            },
            ["hi"] = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [NoPhysicalPath] = "फ़ाइल का डिस्क पर कोई भौतिक पथ नहीं है",
                [CopyFullPath] = "पूरा पथ कॉपी करें",
                [CopyFileName] = "फ़ाइल नाम कॉपी करें",
                [CopyFolderPath] = "फ़ोल्डर पथ कॉपी करें",
                [OpenLocationInExplorer] = "Explorer में स्थान खोलें",
                [AppearanceCategory] = "दिखावट",
                [PathCategory] = "पथ",
                [ActionsCategory] = "क्रियाएँ",
                [ShowFooterName] = "footer दिखाएँ",
                [ShowFooterDescription] = "एडिटर में SourcePath footer दिखाता या छिपाता है।",
                [FontFamilyName] = "फ़ॉन्ट परिवार",
                [FontFamilyDescription] = "footer द्वारा उपयोग किया गया फ़ॉन्ट परिवार।",
                [FontSizeName] = "फ़ॉन्ट आकार",
                [FontSizeDescription] = "footer फ़ॉन्ट आकार। अनुशंसित सीमा: 8 से 24.",
                [FooterHeightName] = "footer ऊँचाई",
                [FooterHeightDescription] = "footer की ऊँचाई पिक्सेल में। अनुशंसित सीमा: 18 से 48.",
                [HorizontalPaddingName] = "क्षैतिज padding",
                [HorizontalPaddingDescription] = "footer की क्षैतिज आंतरिक दूरी, पिक्सेल में।",
                [DisplayModeName] = "पथ प्रारूप",
                [DisplayModeDescription] = "यह नियंत्रित करता है कि footer पूरा पथ, समाधान-सापेक्ष पथ या केवल फ़ाइल नाम दिखाए।",
                [ShowActionButtonsName] = "बटन दिखाएँ",
                [ShowActionButtonsDescription] = "पथ, फ़ाइल नाम, फ़ोल्डर पथ कॉपी करने और स्थान खोलने के लिए छोटे बटन दिखाता है।",
                [EnableCtrlClickOpenName] = "Ctrl+Click स्थान खोलता है",
                [EnableCtrlClickOpenDescription] = "footer पर Ctrl+Click से फ़ाइल स्थान खोलने देता है।",
                [DisplayModeFullPath] = "पूरा पथ",
                [DisplayModeRelativeToSolution] = "समाधान के सापेक्ष",
                [DisplayModeFileName] = "फ़ाइल नाम",
                [TextViewHostMissing] = "एडिटर होस्ट में मान्य टेक्स्ट व्यू नहीं है।",
                [TextDocumentFactoryMissing] = "MEF से ITextDocumentFactoryService आयात नहीं किया जा सका।",
                [ServiceProviderMissing] = "MEF से SVsServiceProvider आयात नहीं किया जा सका।"
            },
            ["ja"] = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [NoPhysicalPath] = "ファイルにはディスク上の物理パスがありません",
                [CopyFullPath] = "完全なパスをコピー",
                [CopyFileName] = "ファイル名をコピー",
                [CopyFolderPath] = "フォルダー パスをコピー",
                [OpenLocationInExplorer] = "エクスプローラーで場所を開く",
                [AppearanceCategory] = "外観",
                [PathCategory] = "パス",
                [ActionsCategory] = "操作",
                [ShowFooterName] = "footer を表示",
                [ShowFooterDescription] = "エディター内の SourcePath footer を表示または非表示にします。",
                [FontFamilyName] = "フォント ファミリ",
                [FontFamilyDescription] = "footer で使用するフォント ファミリ。",
                [FontSizeName] = "フォント サイズ",
                [FontSizeDescription] = "footer のフォント サイズ。推奨範囲: 8 から 24。",
                [FooterHeightName] = "footer の高さ",
                [FooterHeightDescription] = "footer の高さ (ピクセル)。推奨範囲: 18 から 48。",
                [HorizontalPaddingName] = "水平パディング",
                [HorizontalPaddingDescription] = "footer の水平内側余白 (ピクセル)。",
                [DisplayModeName] = "パス形式",
                [DisplayModeDescription] = "footer に完全なパス、ソリューション相対パス、またはファイル名のみを表示するかを制御します。",
                [ShowActionButtonsName] = "ボタンを表示",
                [ShowActionButtonsDescription] = "パス、ファイル名、フォルダー パスのコピー、および場所を開くためのコンパクトなボタンを表示します。",
                [EnableCtrlClickOpenName] = "Ctrl+クリックで場所を開く",
                [EnableCtrlClickOpenDescription] = "footer で Ctrl+クリックするとファイルの場所を開けます。",
                [DisplayModeFullPath] = "完全なパス",
                [DisplayModeRelativeToSolution] = "ソリューション相対",
                [DisplayModeFileName] = "ファイル名",
                [TextViewHostMissing] = "エディター ホストに有効なテキスト ビューが含まれていません。",
                [TextDocumentFactoryMissing] = "MEF から ITextDocumentFactoryService をインポートできませんでした。",
                [ServiceProviderMissing] = "MEF から SVsServiceProvider をインポートできませんでした。"
            },
            ["de"] = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [NoPhysicalPath] = "Die Datei hat keinen physischen Pfad auf dem Datenträger",
                [CopyFullPath] = "Vollständigen Pfad kopieren",
                [CopyFileName] = "Dateinamen kopieren",
                [CopyFolderPath] = "Ordnerpfad kopieren",
                [OpenLocationInExplorer] = "Speicherort im Explorer öffnen",
                [AppearanceCategory] = "Darstellung",
                [PathCategory] = "Pfad",
                [ActionsCategory] = "Aktionen",
                [ShowFooterName] = "Footer anzeigen",
                [ShowFooterDescription] = "Zeigt den SourcePath-Footer im Editor an oder blendet ihn aus.",
                [FontFamilyName] = "Schriftart",
                [FontFamilyDescription] = "Vom Footer verwendete Schriftfamilie.",
                [FontSizeName] = "Schriftgröße",
                [FontSizeDescription] = "Schriftgröße des Footers. Empfohlener Bereich: 8 bis 24.",
                [FooterHeightName] = "Footer-Höhe",
                [FooterHeightDescription] = "Höhe des Footers in Pixeln. Empfohlener Bereich: 18 bis 48.",
                [HorizontalPaddingName] = "Horizontaler Innenabstand",
                [HorizontalPaddingDescription] = "Horizontaler Innenabstand des Footers in Pixeln.",
                [DisplayModeName] = "Pfadformat",
                [DisplayModeDescription] = "Legt fest, ob der Footer den vollständigen Pfad, den lösungsrelativen Pfad oder nur den Dateinamen anzeigt.",
                [ShowActionButtonsName] = "Schaltflächen anzeigen",
                [ShowActionButtonsDescription] = "Zeigt kompakte Schaltflächen zum Kopieren von Pfad, Dateiname und Ordnerpfad sowie zum Öffnen des Speicherorts.",
                [EnableCtrlClickOpenName] = "Strg+Klick öffnet Speicherort",
                [EnableCtrlClickOpenDescription] = "Ermöglicht das Öffnen des Dateispeicherorts mit Strg+Klick auf den Footer.",
                [DisplayModeFullPath] = "Vollständiger Pfad",
                [DisplayModeRelativeToSolution] = "Relativ zur Projektmappe",
                [DisplayModeFileName] = "Dateiname",
                [TextViewHostMissing] = "Der Editorhost enthält keine gültige Textansicht.",
                [TextDocumentFactoryMissing] = "ITextDocumentFactoryService konnte nicht aus MEF importiert werden.",
                [ServiceProviderMissing] = "SVsServiceProvider konnte nicht aus MEF importiert werden."
            }
        };

    public static string Get(string key)
    {
        return Get(key, CultureInfo.CurrentUICulture);
    }

    internal static string Get(string key, CultureInfo? culture)
    {
        string language = GetSupportedLanguage(culture);

        if (Translations.TryGetValue(language, out IReadOnlyDictionary<string, string>? localized) &&
            localized.TryGetValue(key, out string? text))
        {
            return text;
        }

        return Translations["en"].TryGetValue(key, out string? englishText)
            ? englishText
            : key;
    }

    internal static IEnumerable<string> GetSupportedLanguages()
    {
        return Translations.Keys;
    }

    internal static IEnumerable<string> GetRequiredKeys()
    {
        return RequiredKeys;
    }

    internal static bool HasText(string language, string key)
    {
        return Translations.TryGetValue(language, out IReadOnlyDictionary<string, string>? localized) &&
            localized.ContainsKey(key);
    }

    private static string GetSupportedLanguage(CultureInfo? culture)
    {
        if (culture is null)
        {
            return "en";
        }

        string cultureName = culture.Name;

        if (cultureName.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
        {
            return "zh";
        }

        if (cultureName.StartsWith("pt", StringComparison.OrdinalIgnoreCase))
        {
            return "pt";
        }

        string language = culture.TwoLetterISOLanguageName.ToLowerInvariant();

        return Translations.ContainsKey(language)
            ? language
            : "en";
    }
}

[AttributeUsage(AttributeTargets.All)]
internal sealed class SourcePathCategoryAttribute : CategoryAttribute
{
    public SourcePathCategoryAttribute(string key)
        : base(key)
    {
    }

    protected override string GetLocalizedString(string value)
    {
        return SourcePathText.Get(value);
    }
}

[AttributeUsage(AttributeTargets.All)]
internal sealed class SourcePathDisplayNameAttribute : DisplayNameAttribute
{
    public SourcePathDisplayNameAttribute(string key)
        : base(key)
    {
    }

    public override string DisplayName => SourcePathText.Get(DisplayNameValue);
}

[AttributeUsage(AttributeTargets.All)]
internal sealed class SourcePathDescriptionAttribute : DescriptionAttribute
{
    public SourcePathDescriptionAttribute(string key)
        : base(key)
    {
    }

    public override string Description => SourcePathText.Get(DescriptionValue);
}

internal sealed class SourcePathDisplayModeConverter : EnumConverter
{
    public SourcePathDisplayModeConverter()
        : base(typeof(SourcePathDisplayMode))
    {
    }

    public override object? ConvertTo(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object? value,
        Type destinationType)
    {
        if (destinationType == typeof(string) && value is SourcePathDisplayMode displayMode)
        {
            return SourcePathText.Get(GetTextKey(displayMode), culture);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }

    public override object? ConvertFrom(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object value)
    {
        if (value is string text)
        {
            foreach (SourcePathDisplayMode displayMode in Enum.GetValues(typeof(SourcePathDisplayMode)))
            {
                string localizedText = SourcePathText.Get(GetTextKey(displayMode), culture);

                if (string.Equals(text, localizedText, StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(text, displayMode.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return displayMode;
                }
            }
        }

        return base.ConvertFrom(context, culture, value);
    }

    private static string GetTextKey(SourcePathDisplayMode displayMode)
    {
        switch (displayMode)
        {
            case SourcePathDisplayMode.FileName:
                return SourcePathText.DisplayModeFileName;

            case SourcePathDisplayMode.RelativeToSolution:
                return SourcePathText.DisplayModeRelativeToSolution;

            default:
                return SourcePathText.DisplayModeFullPath;
        }
    }
}
