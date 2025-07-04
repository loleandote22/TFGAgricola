using System.Text.Json;
using System.Threading.Tasks;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;
using Windows.UI.ViewManagement;

namespace AplicacionTFG.Presentation.Inventario;

public class InventarioViewModel : ViewModelBase
{
    #region Localización
    public required string Titulo_Loc { get; set; }
    public required string Guardar_Loc { get ; set; }
    public required string NoElementos_Loc { get; set; }
    public required string Nombre_Loc { get; set; }
    public required string Cantidad_Loc { get; set; }
    public required string Medicion_Loc { get; set; }
    public required string Descripcion_Loc { get; set; }
    public required string Tipo_Loc { get; set; }
    public required string Añadir_Loc { get; set; }
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
    private Visibility verAnadir= Visibility.Collapsed;
    private Visibility verInventario;
    public Visibility VerNoHay { get => verNoHay; set { verNoHay = value; OnPropertyChanged(nameof(VerNoHay)); } }
    public Visibility VerAnadir { get => verAnadir; set {
           
            verAnadir = value; OnPropertyChanged(nameof(VerAnadir));
            VerInventario = value != Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
        } }
    public Visibility VerInventario { get => verInventario; set { verInventario = value; OnPropertyChanged(nameof(VerInventario)); } }
    private Visibility verBotonAñadir = Visibility.Collapsed;
    public Visibility VerBotonAñadir { get => verBotonAñadir; set => verBotonAñadir = value; }
    #endregion



    public ICommand AddInventarioCommand => new RelayCommand(Añadir);
    public ICommand GuardarCommand => new RelayCommand(Guardar);
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; }
    public int Cantidad { get; set; }
    public string Unidad { get; set; } = "Unidades";

    private int tipo;
    public List<string> Tipos { get; set; } 
    public int Tipo { get => tipo; set  { tipo = value; OnPropertyChanged(nameof(Tipo)); }}

#pragma warning disable CS8618
    public InventarioViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo) : base(localizer, navigator, appInfo) 
    {
        VerBotonAñadir = Usuario.Tipo<2 ? Visibility.Visible : Visibility.Collapsed;
        Tipos = [_localizer["Maquinaria"], _localizer["Consumible"]];
        VerAnadir = Visibility.Collapsed;
        VerNoHay = Visibility.Collapsed;
        _inventarioApi = new(Apiurl);
        CargarInventario();
    }
#pragma warning restore CS8618 

#if __WASM__
    private async void CargarInventario()
    {
        try
        {
            var result =await _inventarioApi.GetInventariosAsync(Usuario.EmpresaId);
            TerminarCarga(result);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#else
    private void CargarInventario()
    {
        try
        {
            var result = _inventarioApi.GetInventariosAsync(Usuario.EmpresaId).GetAwaiter().GetResult();
            TerminarCarga(result);
        }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
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
        Tipo = -1;
        OnPropertyChanged(nameof(Nombre));
        OnPropertyChanged(nameof(Descripcion));
        OnPropertyChanged(nameof(Cantidad));
        OnPropertyChanged(nameof(Tipo));
        VerAnadir = Visibility.Visible;
        
    }

    private async void Guardar()
    {
#if !WINAPPSDK_PACKAGED
        InputPane.GetForCurrentView().TryHide();
#endif
        try
        {
            InventarioDto inventario = new() { Nombre = Nombre, Descripcion = Descripcion, Cantidad = Cantidad, Tipo = Tipo, Unidad = Unidad, EmpresaId = Usuario.EmpresaId };
            if (!ValidarModelo(inventario))
            {
                await _navigator.ShowMessageDialogAsync(this, title: Titulo_Loc, content: _localizer[_mensajeError]);
                return;
            }
            var resultado = await _inventarioApi.PostInventarioAsync(inventario);
            VerAnadir = Visibility.Collapsed;
            if (resultado is not null)
            {
                var elemento = JsonSerializer.Deserialize(resultado!, InventarioConsultaContext.Default.InventarioConsulta);
                var inventarios = Inventarios.ToList();
                inventarios.Add(elemento!);
                Inventarios = inventarios;
                LimpiarCampos();
                OnPropertyChanged(nameof(Inventarios));
            }
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    private void LimpiarCampos()
    {
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Cantidad = 0;
        Tipo = -1;
    }

    protected override void CargarPalabras()
    {
        Titulo_Loc = _localizer["Inventario"];
        Guardar_Loc = _localizer["Guardar"];
        NoElementos_Loc = _localizer["NoElementos"];
        Nombre_Loc = _localizer["Nombre"];
        Cantidad_Loc = _localizer["Cantidad"];
        Medicion_Loc = _localizer["Medida"];
        Descripcion_Loc = _localizer["Descripcion"];
        Tipo_Loc = _localizer["Tipo"];
        Añadir_Loc = _localizer["Añadir"];
    }
}
