using System.Text.Json;
using System.Threading.Tasks;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;

namespace AplicacionTFG.Presentation.Inventario;

public class InventarioViewModel : ViewModelBase
{
    #region Localización
    public required string Titulo_Loc { get; set; }
    public required string Guardar_Loc { get ; set; }
    public required string NoElementos_Loc { get; set; }
    public required string Nombre_Loc { get; set; }
    public required string Cantidad_Loc { get; set; }
    public required string Descripcion_Loc { get; set; }
    public required string Tipo_Loc { get; set; }
    #endregion

    #region Elementos
    private InventarioConsulta? inventarioSeleccionado;
    public required List<InventarioConsulta> Inventarios { get; set; }
    public InventarioConsulta? InventarioSeleccionado
    {
        get => inventarioSeleccionado;
        set
        {
            if (value is null) return;
            inventarioSeleccionado = value;
            OnPropertyChanged(nameof(InventarioSeleccionado));
            Navegar();
        }
    }
    #endregion

    private readonly InventarioApi _inventarioApi ;

    #region Visibilidad
    private Visibility verNoHay;
    private Visibility verAnadir;
    private Visibility verInventario;
    public Visibility VerNoHay { get => verNoHay; set { verNoHay = value; OnPropertyChanged(nameof(VerNoHay)); } }
    public Visibility VerAnadir { get => verAnadir; set {
            VerInventario = value != Visibility.Visible ?  Visibility.Visible : Visibility.Collapsed;
            verAnadir = value; OnPropertyChanged(nameof(VerAnadir)); } }
    public Visibility VerInventario { get => verInventario; set { verInventario = value; OnPropertyChanged(nameof(VerInventario)); } }
    #endregion

    

    public ICommand AddInventarioCommand => new RelayCommand(Añadir);
    public ICommand GuardarCommand => new RelayCommand(Guardar);
    public ICommand AddInventarioPequeñoCommand => new RelayCommand(AñadirPequeño);
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; }
    public int Cantidad { get; set; }

    private string tipo;
    public List<string> Tipos { get; set; } = new() { "Material", "Herramienta", "Equipo" };
    public string Tipo { get => tipo; set  { tipo = value; OnPropertyChanged(nameof(Tipo)); }}
#pragma warning disable CS8618
    public InventarioViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo) : base(localizer, navigator, appInfo) 
    {
        VerAnadir = Visibility.Collapsed;
        VerNoHay = Visibility.Collapsed;
        _inventarioApi = new(Apiurl);
        CargarInventario();
    }
#pragma warning restore CS8618 

#if __WASM__
    private async void CargarInventario()
    {
        var result =await _inventarioApi.GetInventariosAsync(Usuario.EmpresaId.GetValueOrDefault());
        TerminarCarga(result);
    }
#else
    private void CargarInventario()
    {
        var result = _inventarioApi.GetInventariosAsync(Usuario.EmpresaId.GetValueOrDefault()).GetAwaiter().GetResult();
        TerminarCarga(result);
    }
#endif
    private void TerminarCarga(string? resultado)
    {
        if (string.IsNullOrEmpty(resultado))
        {
            VerNoHay = Visibility.Visible;
            return;
        }
        var deserializedResult = JsonSerializer.Deserialize(resultado, InventarioConsultaContext.Default.ListInventarioConsulta);
        Inventarios = deserializedResult!;
        OnPropertyChanged(nameof(Inventarios));
        VerNoHay = Inventarios.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    }
    private void Navegar()
    {
        _navigator.NavigateViewModelAsync<ElementoViewModel>(this, qualifier: Qualifiers.ClearBackStack, data: new EntityNumber(InventarioSeleccionado!.Id));
    }

    private void Añadir()
    {
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Cantidad = 0;
        Tipo = string.Empty;
        OnPropertyChanged(nameof(Nombre));
        OnPropertyChanged(nameof(Descripcion));
        OnPropertyChanged(nameof(Cantidad));
        OnPropertyChanged(nameof(Tipo));
        VerAnadir = Visibility.Visible;
        
    }

    private void AñadirPequeño()
    {
        _navigator.NavigateViewModelAsync<AnadirElementoViewModel>(this);
    }
    private async void Guardar()
    {
        Models.Inventario inventario = new() { Nombre = Nombre, Descripcion = Descripcion, Cantidad = Cantidad, Tipo = Tipo, EmpresaId = Usuario.EmpresaId.GetValueOrDefault() };
        if (!ValidarModelo(inventario))
        {
            await _navigator.ShowMessageDialogAsync(this, title: Titulo_Loc, content:_mensajeError);
            return;
        }
        VerAnadir = Visibility.Collapsed;
        var resultado = await _inventarioApi.PostInventarioAsync(inventario);
        var elemento = JsonSerializer.Deserialize<Models.InventarioConsulta>(resultado!, InventarioConsultaContext.Default.InventarioConsulta);
        var inventarios = Inventarios.ToList();
        inventarios.Add(elemento!);
        Inventarios = inventarios;
        LimpiarCampos();
        OnPropertyChanged(nameof(Inventarios));
    }

    private void LimpiarCampos()
    {
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Cantidad = 0;
        Tipo = string.Empty;
    }

    protected override void CargarPalabras()
    {
        Titulo_Loc = _localizer["Inventario"];
        Guardar_Loc = _localizer["Guardar"];
        NoElementos_Loc = _localizer["NoElementos"];
        Nombre_Loc = _localizer["Nombre"];
        Cantidad_Loc = _localizer["Cantidad"];
        Descripcion_Loc = _localizer["Descripcion"];
        Tipo_Loc = _localizer["Tipo"];
    }
}
