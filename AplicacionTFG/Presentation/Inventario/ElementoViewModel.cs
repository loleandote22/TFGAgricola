using System.Text.Json;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;

namespace AplicacionTFG.Presentation.Inventario;
public class ElementoViewModel : ViewModelBase
{
    #region Localización
    public required string Editar_Loc { get; set; }
    public required string Eliminar_Loc { get; set; }
    #endregion
    
    public Models.Inventario Elemento { get; set; }

    #region Comandos
    public ICommand EditarCommand => new RelayCommand(Editar);
    public ICommand EliminarCommand => new RelayCommand(Eliminar);
    #endregion

    private readonly InventarioApi _inventarioApi;
    public ElementoViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, EntityNumber elemento) : base(localizer, navigator, appInfo)
    {
       
        _inventarioApi = new InventarioApi(Apiurl);
        CargarElemento(elemento.number);
    }
    private async void CargarElemento(int id)
    {
        try {
            var resutlado = await _inventarioApi.GetInventarioAsync(id);
            if (resutlado is not null)
                Elemento = JsonSerializer.Deserialize(resutlado, InventarioContext.Default.Inventario)!;
            if (Elemento is null)
                await _navigator.ShowMessageDialogAsync(this, title: "Error", content: "No se ha podido cargar el elemento del inventario.");
            OnPropertyChanged(nameof(Elemento));
        }
        catch(Exception ex)
        {
            await _navigator.ShowMessageDialogAsync(this, title:"Fallo", content: ex.Message);
        }
       
    }

    private async void Editar()
    {
        await _navigator.NavigateViewModelAsync<EdicionElementoViewModel>(this, data: Elemento);
    }

    protected override void CargarPalabras()
    {
        Editar_Loc = _localizer["Editar"];
        Eliminar_Loc = _localizer["Eliminar"];
    }

    private async void Eliminar()
    {
        await _inventarioApi.DeleteInventarioAsync(Elemento!.Id);
        await _navigator.NavigateViewModelAsync<InventarioViewModel>(this);
    }
}

public class EdicionElementoViewModel : ViewModelBase
{
    #region Localización
    public required string Nombre_Loc { get; set; }
    public required string Descripcion_Loc { get; set; }
    public required string Cantidad_Loc { get; set; }
    public required string Tipo_Loc { get; set; }
    public List<string> Tipos { get; set; } = new() { "Material", "Herramienta", "Equipo" };
    #endregion

    #region Campos
    public string Nombre { get => Elemento.Nombre; set { Elemento.Nombre = value; OnPropertyChanged(nameof(Nombre)); } }
    public string? Descripcion { get => Elemento?.Descripcion; set { Elemento.Descripcion = value!; OnPropertyChanged(nameof(Descripcion)); } }
    public int Cantidad { get => Elemento.Cantidad; set { Elemento.Cantidad = value; OnPropertyChanged(nameof(Cantidad)); } }
    public string Tipo { get => Elemento.Tipo; set { Elemento.Tipo = value; OnPropertyChanged(nameof(Tipo)); } }
    #endregion

    #region Comandos
    public ICommand GuardarCommand => new RelayCommand(Guardar);
    public ICommand EliminarCommand => new RelayCommand(Eliminar);
    #endregion
    public Models.Inventario Elemento { get; set; }
    private readonly InventarioApi _inventarioApi;
    public EdicionElementoViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, Models.Inventario elemento) : base(localizer, navigator, appInfo)
    {
        Elemento = elemento;
        _inventarioApi = new InventarioApi(Apiurl);
    }

    protected override void CargarPalabras()
    {
        Nombre_Loc = _localizer["Nombre"];
        Descripcion_Loc = _localizer["Descripcion"];
        Cantidad_Loc = _localizer["Cantidad"];
        Tipo_Loc = _localizer["Tipo"];
    }

    private async void Guardar()
    {
        if (!ValidarModelo(Elemento))
        {
            await _navigator.ShowMessageDialogAsync(this, title:"Error al guardar", content: _mensajeError);
            return;
        }
        InventarioActualizaDto inventario = new InventarioActualizaDto()
        {
            Id = Elemento!.Id,
            Nombre = Elemento.Nombre,
            Tipo = Elemento.Tipo,
            Descripcion = Elemento.Descripcion,
            Cantidad = Elemento.Cantidad,
            UsuarioId = Usuario.Id
        };
        await _inventarioApi.PutInventarioAsync(inventario);
        await _navigator.NavigateViewModelAsync<InventarioViewModel>(this);
    }

    private async void Eliminar()
    {
        await _inventarioApi.DeleteInventarioAsync(Elemento.Id);
        await _navigator.NavigateViewModelAsync<InventarioViewModel>(this);
    }
}

public class AnadirElementoViewModel : ViewModelBase
{
    #region Localización
    public required string Titulo_Loc { get; set; }
    public required string Guardar_Loc { get; set; }
    public required string Nombre_Loc { get; set; }
    public required string Cantidad_Loc { get; set; }
    public required string Descripcion_Loc { get; set; }
    public required string Tipo_Loc { get; set; }
    #endregion

    private readonly InventarioApi _inventarioApi;

    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; }
    public int Cantidad { get; set; }
    public List<string> Tipos { get; set; } = new() { "Material", "Herramienta", "Equipo" };
    private string tipo;
    public string Tipo { get => tipo; set { tipo = value; OnPropertyChanged(nameof(Tipo)); } }


    public ICommand GuardarCommand => new RelayCommand(Guardar);


    public AnadirElementoViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo) : base(localizer, navigator, appInfo)
    {
        _inventarioApi = new InventarioApi(Apiurl);
    }

    private async void Guardar()
    {
        Models.Inventario inventario = new() { Nombre = Nombre, Descripcion = Descripcion, Cantidad = Cantidad, Tipo = Tipo, EmpresaId = Usuario.EmpresaId.GetValueOrDefault() };
        if (!ValidarModelo(inventario))
        {
            await _navigator.ShowMessageDialogAsync(this, title: Titulo_Loc, content: _mensajeError);
            return;
        }
        await _inventarioApi.PostInventarioAsync(inventario);
        await _navigator.NavigateViewModelAsync<InventarioViewModel>(this, qualifier: Qualifiers.ClearBackStack);
    }

    protected override void CargarPalabras()
    {
        Titulo_Loc = _localizer["Inventario"];
        Guardar_Loc = _localizer["Guardar"];
        Nombre_Loc = _localizer["Nombre"];
        Cantidad_Loc = _localizer["Cantidad"];
        Descripcion_Loc = _localizer["Descripcion"];
        Tipo_Loc = _localizer["Tipo"];
    }
}
