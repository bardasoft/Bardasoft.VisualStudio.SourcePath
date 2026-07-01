#nullable enable

using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace Bardasoft.VisualStudio.SourcePath;

public sealed class SourcePathOptionsPage : DialogPage
{
    [Category("Apariencia")]
    [DisplayName("Mostrar footer")]
    [Description("Muestra u oculta el footer de Bardasoft SourcePath en el editor.")]
    [DefaultValue(true)]
    public bool IsFooterVisible { get; set; } = true;

    [Category("Apariencia")]
    [DisplayName("Tipo de letra")]
    [Description("Nombre de la familia tipografica usada en el footer.")]
    [DefaultValue("Segoe UI")]
    public string FontFamily { get; set; } = "Segoe UI";

    [Category("Apariencia")]
    [DisplayName("Tamano de letra")]
    [Description("Tamano de letra del footer. Rango recomendado: 8 a 24.")]
    [DefaultValue(11d)]
    public double FontSize { get; set; } = 11;

    [Category("Apariencia")]
    [DisplayName("Alto del footer")]
    [Description("Alto del footer en pixeles. Rango recomendado: 18 a 48.")]
    [DefaultValue(24d)]
    public double FooterHeight { get; set; } = 24;

    [Category("Apariencia")]
    [DisplayName("Padding horizontal")]
    [Description("Espacio horizontal interno del footer en pixeles.")]
    [DefaultValue(8d)]
    public double HorizontalPadding { get; set; } = 8;

    [Category("Ruta")]
    [DisplayName("Formato de ruta")]
    [Description("Define si el footer muestra la ruta completa, la ruta relativa a la solucion o solo el nombre del archivo.")]
    [DefaultValue(SourcePathDisplayMode.FullPath)]
    public SourcePathDisplayMode DisplayMode { get; set; } = SourcePathDisplayMode.FullPath;

    [Category("Acciones")]
    [DisplayName("Mostrar botones")]
    [Description("Muestra botones compactos para copiar ruta, copiar nombre, copiar carpeta y abrir ubicacion.")]
    [DefaultValue(true)]
    public bool ShowActionButtons { get; set; } = true;

    [Category("Acciones")]
    [DisplayName("Ctrl+Click abre ubicacion")]
    [Description("Permite abrir la ubicacion del archivo con Ctrl+Click sobre el footer.")]
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
