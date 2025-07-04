using System.Text.Json;
using System.Xml.Linq;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;
using Windows.UI.ViewManagement;

namespace AplicacionTFG.Presentation.Inventario;
public class ElementoViewModel : ViewModelBase
{
    #region Localización
    public required string Editar_Loc { get; set; }
    public required string Eliminar_Loc { get; set; }
    public required string Eventos_Loc { get; set; }
    public required string Mensajes_Loc { get; set; }
    public required string Mensaje_Loc { get; set; }
    public required string Mas_Loc { get; set; }
    #endregion

    #region Visiblidad
    private Visibility verMensajes;
    private Visibility verEventos;
    public Visibility VerMensajes { get => verMensajes; set  { verMensajes = value;  OnPropertyChanged(nameof(VerMensajes)); }}
    public Visibility VerEventos { get => verEventos; set { verEventos = value; OnPropertyChanged(nameof(VerEventos)); }}
    private Visibility verNoHay;
    public Visibility VerNoHay { get => verNoHay; set { verNoHay = value; OnPropertyChanged(nameof(VerNoHay)); } }
    private Visibility verMasEventos;
    private Visibility verMasMensajes;
    public Visibility VerMasEventos { get => verMasEventos; set { verMasEventos = value; OnPropertyChanged(nameof(VerMasEventos)); }}
    public Visibility VerMasMensajes { get => verMasMensajes; set { verMasMensajes = value; OnPropertyChanged(nameof(VerMasMensajes)); }}
    private Visibility verEditarEliminar = Visibility.Collapsed; public Visibility VerEditarEliminar { get => verEditarEliminar; set => verEditarEliminar = value; }


    #endregion


    private int indice;
    public int Indice
    {
        get => indice; set
        {
            indice = value;
            VerEventos = indice == 0 ? Visibility.Visible : Visibility.Collapsed;
            VerMensajes = indice == 1 ? Visibility.Visible : Visibility.Collapsed;
            OnPropertyChanged(nameof(Indice));
        }
    }

    private int _PaginaEventos = 0;
    private int _PaginaMensajes = 0;

    public string TipoNombre { get; set; } = string.Empty;
    private bool hayMensaje;
    public bool HayMensaje { get => hayMensaje; set { hayMensaje = value; OnPropertyChanged(nameof(HayMensaje)); } }
    public string Mensaje { get => mensaje; set { 
            HayMensaje =value.Length > 0; 
            mensaje = value; OnPropertyChanged(nameof(Mensaje)); }}

    public InventarioConsulta Elemento { get => elemento; set { elemento = value; OnPropertyChanged(nameof(Elemento)); }}
    private List<InventarioChatConsulta> mensajes = new();
    private List<InventarioEventoConsulta> eventos = new();
    public List<InventarioEventoConsulta> Eventos { get => eventos; set { eventos = value; OnPropertyChanged(nameof(Eventos)); } }
    public List<InventarioChatConsulta> Mensajes { get => mensajes; set { mensajes = value; OnPropertyChanged(nameof(Mensajes)); }}

    #region Comandos
    public ICommand EditarCommand => new RelayCommand(Editar);
    public ICommand EliminarCommand => new RelayCommand(Eliminar);

    public ICommand CargarMasMensajesCommand;
    public ICommand CargarMasEventosCommand => new RelayCommand(CargarMasEventos);
    public ICommand EnviarMensajeCommand => new RelayCommand(EnviarMensaje);




    #endregion

    private readonly InventarioApi _inventarioApi;
    private InventarioConsulta elemento;
    private string mensaje;

    private List<string> _tiposEventos;

#pragma warning disable CS8618
    public ElementoViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, EntityNumber elemento) : base(localizer, navigator, appInfo)
    {
        VerEditarEliminar = Usuario.Tipo <2 ? Visibility.Visible : Visibility.Collapsed;
        _tiposEventos = [_localizer["Salida"], _localizer["Entrada"]];
        VerMensajes = Visibility.Collapsed;
       _inventarioApi = new InventarioApi(Apiurl);
#if __WASM__
        CargarElemento(elemento.number);
#else
        try
        {
            var resultado = _inventarioApi.GetInventarioAsync(elemento.number).Result;
            if (resultado is not null)
            {
                List<string> tipos = [_localizer["Maquinaria"], _localizer["Consumible"]];
                Elemento = JsonSerializer.Deserialize(resultado, InventarioConsultaContext.Default.InventarioConsulta)!;
                TipoNombre = tipos[Elemento.Tipo];
                CargarMasEventos();
                CargarMasMensajes();

            }
            else
                _navigator.ShowMessageDialogAsync(this, title: "Error", content: "No se ha podido cargar el elemento del inventario.");
        }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
#endif
    }
#pragma warning restore CS8618 
    private async void CargarElemento(int id)
    {
        try
        {
            var resultado = await _inventarioApi.GetInventarioAsync(id);
            if (resultado is not null) 
            { 
                Elemento = JsonSerializer.Deserialize(resultado, InventarioConsultaContext.Default.InventarioConsulta)!;
                CargarMasEventos();
                CargarMasMensajes();
            }
            else
                await _navigator.ShowMessageDialogAsync(this, title: "Error", content: "No se ha podido cargar el elemento del inventario.");
            
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _navigator.ShowMessageDialogAsync(this, title:"Fallo", content: ex.Message);
        }
    }

#if __WASM__
    private void CargarMasEventos()
    {
        try
        {
            _PaginaEventos++;
            _inventarioApi.GetInventarioEventosAsync(Elemento!.Id, _PaginaEventos).ContinueWith(t =>
            {
                if (t.Result is not null)
                {
                    var eventosNuevos = JsonSerializer.Deserialize(t.Result, InventarioEventoConsultaContext.Default.ListInventarioEventoConsulta)!;
                    if (eventosNuevos.Count < 20)
                    {
                        VerMasEventos = Visibility.Collapsed;
                    }
                    foreach (InventarioEventoConsulta evento in eventosNuevos)
                    {
                        evento.TipoNombre = _tiposEventos[evento.Tipo] ;
                    }
                    Eventos = Eventos.Union(eventosNuevos).ToList();
                }
            });
            }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
    private void CargarMasMensajes()
    {
        try
        {
            _PaginaMensajes++;
            _inventarioApi.GetInventarioChatsAsync(Elemento!.Id, _PaginaMensajes).ContinueWith(t =>
            {
                if (t.Result is not null)
                {
                    var mensajesNuevos = JsonSerializer.Deserialize(t.Result, InventarioChatConsultaContext.Default.ListInventarioChatConsulta)!;
                    if (mensajes.Count < 20)
                        VerMasMensajes = Visibility.Collapsed;
                    Mensajes = Mensajes.Union(mensajesNuevos).ToList();
                }
            });
            }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#else
    private async void CargarMasEventos()
    {
        try
        {
            _PaginaEventos++;
            var resultado = await _inventarioApi.GetInventarioEventosAsync(Elemento!.Id, _PaginaEventos);
           if (resultado is not null)
            {
                var eventosNuevos = JsonSerializer.Deserialize(resultado, InventarioEventoConsultaContext.Default.ListInventarioEventoConsulta)!;
                if (eventosNuevos.Count < 20)
                    VerMasEventos = Visibility.Collapsed;
                foreach (InventarioEventoConsulta evento in eventosNuevos)
                {
                    evento.TipoNombre = _tiposEventos[evento.Tipo] ;
                }
                Eventos = Eventos.Union(eventosNuevos).ToList();
            }
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    private async void CargarMasMensajes()
    {
        try 
        {
            _PaginaMensajes++;
            var resultado = await _inventarioApi.GetInventarioChatsAsync(Elemento!.Id, _PaginaMensajes);
            if (resultado is not null)
            {
                var mensajesNuevos = JsonSerializer.Deserialize(resultado, InventarioChatConsultaContext.Default.ListInventarioChatConsulta)!;
                if (mensajesNuevos.Count < 20)
                    VerMasMensajes = Visibility.Collapsed;
                Mensajes = Mensajes.Union(mensajesNuevos).ToList();
            }
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#endif


    private async void Editar()
    {
        await _navigator.NavigateViewModelAsync<EdicionElementoViewModel>(this, data: Elemento);
    }

    protected override void CargarPalabras()
    {
        Editar_Loc = _localizer["Editar"];
        Eliminar_Loc = _localizer["Eliminar"];
        Eventos_Loc = _localizer["Eventos"];
        Mensajes_Loc = _localizer["Mensajes"];
        Mensaje_Loc = _localizer["Mensaje"];
        Mas_Loc = _localizer["Mas"];
    }

    private async void Eliminar()
    {
        try
        {
            await _inventarioApi.DeleteInventarioAsync(Elemento!.Id);
            await _navigator.NavigateViewModelAsync<InventarioViewModel>(this);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    private async void EnviarMensaje()
    {
        try
        {
            InventarioChat inventarioChat = new ()
            {
                Mensaje = Mensaje,
                InventarioId = Elemento.Id,
                UsuarioId = Usuario.Id,
                Fecha = DateTime.Now
            };
            InventarioChatConsulta inventarioChatConsulta = new()
            {
                Fecha = inventarioChat.Fecha,
                Mensaje = Mensaje,
                UsuarioNombre = Usuario.Nombre
            };
            await _inventarioApi.PostComentarioInventario(inventarioChat);
            Mensaje = string.Empty;
            if (Mensajes is null)
                Mensajes = new List<InventarioChatConsulta>() { inventarioChatConsulta };
            else
                Mensajes = new List<InventarioChatConsulta>() { inventarioChatConsulta }.Union(Mensajes).ToList();
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }

    }
}

public class EdicionElementoViewModel : ViewModelBase
{
    #region Localización
    public required string Nombre_Loc { get; set; }
    public required string Descripcion_Loc { get; set; }
    public required string Cantidad_Loc { get; set; }
    public required string Medicion_Loc { get; set; }
    public required string Tipo_Loc { get; set; }
    #endregion

    #region Campos
    private string nombre= null!;
    private string descripcion = null!;
    private string unidad = null!;
    private int cantidad;
    private int tipo;

    public List<string> Tipos { get; set; }
    public string Nombre { get => nombre; set { nombre = value; OnPropertyChanged(nameof(Nombre)); } }
    public required string Descripcion { get => descripcion; set { descripcion = value!; OnPropertyChanged(nameof(Descripcion)); } }
    public int Cantidad { get => cantidad;  set { cantidad = value; OnPropertyChanged(nameof(Cantidad)); } }
    public string Unidad { get => unidad; set { unidad = value; OnPropertyChanged(nameof(Unidad)); }}
    public int Tipo { get => tipo; set { tipo= value; OnPropertyChanged(nameof(Tipo)); } }
    #endregion

    #region Comandos
    public ICommand GuardarCommand => new RelayCommand(Guardar);
    public ICommand EliminarCommand => new RelayCommand(Eliminar);

    #endregion
    private readonly InventarioApi _inventarioApi;
    private int id;
    public EdicionElementoViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, InventarioConsulta elemento) : base(localizer, navigator, appInfo)
    {
        Tipos = [_localizer["Maquinaria"], _localizer["Consumible"]];
        id = elemento.Id;
        Nombre = elemento.Nombre;
        Descripcion = elemento.Descripcion;
        Cantidad = elemento.Cantidad;
        Unidad = elemento.Unidad;
        Tipo = elemento.Tipo;
        _inventarioApi = new InventarioApi(Apiurl);
    }

    protected override void CargarPalabras()
    {
        Nombre_Loc = _localizer["Nombre"];
        Descripcion_Loc = _localizer["Descripcion"];
        Cantidad_Loc = _localizer["Cantidad"];
        Medicion_Loc = _localizer["Medida"];
        Tipo_Loc = _localizer["Tipo"];
    }

    private async void Guardar()
    {
#if !WINAPPSDK_PACKAGED
        InputPane.GetForCurrentView().TryHide();
#endif
        try
        {
            InventarioActualizaDto inventario = new InventarioActualizaDto()
            {
                Id = id,
                Nombre = Nombre,
                Tipo = Tipo,
                Descripcion = Descripcion,
                Cantidad = Cantidad,
                UsuarioId = Usuario.Id,
                Unidad = Unidad
            };
            if (!ValidarModelo(inventario))
            {
                await _navigator.ShowMessageDialogAsync(this, title: "Error al guardar", content: _localizer[_mensajeError]);
                return;
            }
            await _inventarioApi.PutInventarioAsync(inventario);
            await _navigator.NavigateViewModelAsync<InventarioViewModel>(this);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    private async void Eliminar()
    {
        try
        {
            await _inventarioApi.DeleteInventarioAsync(id);
            await _navigator.NavigateViewModelAsync<InventarioViewModel>(this);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
}
