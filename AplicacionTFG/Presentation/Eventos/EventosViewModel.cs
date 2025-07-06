using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI;
using Windows.UI;
using Windows.UI.ViewManagement;
namespace AplicacionTFG.Presentation.Eventos;
public class EventosMesViewModel : ViewModelBase
{
    #region Auxiliar
    private DateOnly _fechaBusqueda;
    private readonly int _idUsuario = 0;
    private readonly int _tipoUsuario = 2;
    private int diaSeleccionado;
    public int DiaSeleccionado { get => diaSeleccionado; set { diaSeleccionado = value; VerDia(value); }}
    public Visibility AñadirEventosVisibility { get; set; }
    public Visibility EmpleadosVisibility { get; set; }
    public List<UsuarioNombre> Empleados { get; set; } = null!;
    public UsuarioNombre EmpleadoSeleccionado { get => empleadoSeleccionado; set  { empleadoSeleccionado = value; ActualizarEventos(); } }
    #endregion

    #region Localización
    private string fecha_Loc="";
    public string Calendario_Loc { get; set; } = "";
    public string Hoy_Loc { get; set; }= "";
    public string Fecha_Loc { get => fecha_Loc; set { fecha_Loc = value; OnPropertyChanged(nameof(Fecha_Loc)); }}
    public string Lunes_Loc { get; set; } = "";
    public string Martes_Loc { get; set; } = "";
    public string Miercoles_Loc { get; set; } = "";
    public string Jueves_Loc { get; set; } = "";
    public string Viernes_Loc { get; set; } = "";
    public string Sabado_Loc { get; set; } = "";
    public string Domingo_Loc { get; set; } = "";
    public string LunesCorto_Loc { get; set; } = "";
    public string MartesCorto_Loc { get; set; } = "";
    public string MiercolesCorto_Loc { get; set; } = "";
    public string JuevesCorto_Loc { get; set; } = "";
    public string ViernesCorto_Loc { get; set; } = "";
    public string SabadoCorto_Loc { get; set; } = "";
    public string DomingoCorto_Loc { get; set; } = "";
    public string Tipo_Loc { get; set; } = "";
    public string MesAnterior_Loc { get; set; } = "";
    public string MesSiguiente_Loc { get; set; } = "";
    public string Añadir_Loc { get; set; } = "";

    public string Empleado_Loc { get; set; } = "";

    #endregion

    #region Comandos
    public RelayCommand VerMesAnteriorCommand => new (() =>
    {
        _fechaBusqueda = _fechaBusqueda.AddMonths(-1);
#if __WASM__
        CargarEventos( _fechaBusqueda.Month, _fechaBusqueda.Year);
#else
        CargarEventosComando( _fechaBusqueda.Month, _fechaBusqueda.Year);
#endif
    });

    public RelayCommand VerMesSiguienteCommand => new (() =>
    {
        _fechaBusqueda = _fechaBusqueda.AddMonths(1);
#if __WASM__
        CargarEventos(_fechaBusqueda.Month, _fechaBusqueda.Year);
#else
        CargarEventosComando(_fechaBusqueda.Month, _fechaBusqueda.Year);
#endif
    });

    public RelayCommand VerMesActualCommand => new (() =>
    {
        _fechaBusqueda = DateOnly.FromDateTime(DateTime.Now);
#if __WASM__
        CargarEventos( _fechaBusqueda.Month, _fechaBusqueda.Year);
#else
        CargarEventosComando( _fechaBusqueda.Month, _fechaBusqueda.Year);
#endif
    });

    public RelayCommand AñadirEventoCommand => new (() =>
    {
        _navigator.NavigateViewModelAsync<AñadirEventoViewModel>(this, qualifier: Qualifiers.ClearBackStack, new EntityDateNumber(DateTime.Now, _idUsuario));
    });
    #endregion


    public List<string> Tipos { get; set; } = null!;

    private int tipoSeleccionado = 0;
    public int TipoSeleccionado { get => tipoSeleccionado; set { tipoSeleccionado = value; ActualizarEventos(); }}

    private readonly EventoApi _eventoApi;
    private readonly UsuarioApi _usuarioApi;
    private List<EventoMes> _eventos = null!;
    public List<Dia> Dias { get; set; } = null!;
    public int ColumnasEmpleados { get => columnasEmpleados; set { columnasEmpleados = value; OnPropertyChanged(nameof(ColumnasEmpleados)); }}

    private bool esPropio = true;
    private bool noCargar = true;
    private UsuarioNombre empleadoSeleccionado = null!;
    private int columnasEmpleados;

    public EventosMesViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo) : base(localizer, navigator, appInfo)
    {
        TransientSettings.Set("Calendario", true);
        _tipoUsuario = Usuario.Tipo;
        _idUsuario = Usuario.Id;
        _eventoApi = new EventoApi(Apiurl);
        _usuarioApi = new UsuarioApi(Apiurl);
        _fechaBusqueda = DateOnly.FromDateTime(DateTime.Now);
        AñadirEventosVisibility = _tipoUsuario <2 ? Visibility.Visible: Visibility.Collapsed;
        EmpleadosVisibility = Usuario.Tipo<2 ? Visibility.Visible : Visibility.Collapsed;
        CargarUsuarios();
        noCargar = false;
        ColumnasEmpleados = Usuario.Tipo < 2 ? 6 : 0;
        CargarEventos( _fechaBusqueda.Month, _fechaBusqueda.Year);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="navigator"> Permite "navegar entre páginas"</param>
    /// <param name="localizer"> Permite cargar los textos en distintos idiomas </param>
    /// <param name="appInfo"> Permite obtener los valores de variables del programa</param>
    /// <param name="idUsuario"> Usuario del que se quieren saber los eventos</param>
    /// <param name="origen"> Saber si se llama desde el perfil de un Usuario o desde el módulo de eventos</param>
    public EventosMesViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, int idUsuario, string? origen="Home") : base(localizer, navigator, appInfo)
    {
        _idUsuario = idUsuario;
        _eventoApi = new EventoApi(Apiurl);
        _usuarioApi = new UsuarioApi(Apiurl);
        _fechaBusqueda = DateOnly.FromDateTime(DateTime.Now);
        AñadirEventosVisibility = Visibility.Collapsed;
        EmpleadoSeleccionado = new UsuarioNombre() { Id = idUsuario, Nombre = "" };
        EmpleadosVisibility = Visibility.Collapsed;
        noCargar = false;
        CargarEventos(_fechaBusqueda.Month, _fechaBusqueda.Year);
        esPropio = false;
    }
#if __WASM__
    private async void CargarEventos( int mes, int año)
    {
        try
        {
            Fecha_Loc = _fechaBusqueda.ToString("MMMM - yyyy", System.Globalization.CultureInfo.CurrentCulture);
            var result = string.Empty;
            if (_tipoUsuario == 2)
                result =await _eventoApi.GetEventosMesUsuarioAsync(_idUsuario, mes , año);
            else
                result =await _eventoApi.GetEventosMesEmpresaAsync(Usuario.EmpresaId, mes , año);
                TerminarCarga(result);
        }catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    private async void CargarUsuarios()
    {
        try{
            var result = await _usuarioApi.GetNombreUsuariosEmpresa(Usuario.EmpresaId);
            TerminarCargaUsuarios(result);
        }catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
        }
#else
    private void CargarEventos(int mes, int año)
    {
        try { 
            Fecha_Loc = _fechaBusqueda.ToString("MMMM - yyyy", System.Globalization.CultureInfo.CurrentCulture);
            var result = "";
            if (_tipoUsuario == 2)
                result = _eventoApi.GetEventosMesUsuarioAsync(_idUsuario, mes, año).GetAwaiter().GetResult();
            else
                 result = _eventoApi.GetEventosMesEmpresaAsync(Usuario.EmpresaId, mes, año).GetAwaiter().GetResult();

            TerminarCarga(result);
        }catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
    private void CargarUsuarios()
    {
        try
        {
            var result = _usuarioApi.GetNombreUsuariosEmpresa(Usuario.EmpresaId).GetAwaiter().GetResult();
            TerminarCargaUsuarios(result);
        }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mes"></param>
    /// <param name="año"></param>
    private async void CargarEventosComando(int mes, int año)
    {
        try 
        { 
            Fecha_Loc = _fechaBusqueda.ToString("MMMM - yyyy", System.Globalization.CultureInfo.CurrentCulture);
            string? result = null;
            if (_tipoUsuario == 2)
                result = await _eventoApi.GetEventosMesUsuarioAsync(_idUsuario, mes, año);
            else
                result = await _eventoApi.GetEventosMesEmpresaAsync(Usuario.EmpresaId, mes, año);
            TerminarCarga(result);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#endif

    private void TerminarCarga(string? result)
    {
        if (string.IsNullOrEmpty(result))
        {
            Dias = new();
            return;
        }
        try
        {
            _eventos = JsonSerializer.Deserialize(result, EventoMesContext.Default.ListEventoMes)!;
            ActualizarEventos();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar los eventos: {ex.Message}");
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: "Parece que algo salió mal");
        }
    }



    private void TerminarCargaUsuarios(string? result)
    {
        try
        {
            if (result is not null)
                Empleados = new List<UsuarioNombre> { new UsuarioNombre() { Id = -1, Nombre = _localizer["Todos"] } }.Concat(JsonSerializer.Deserialize(result, UsuarioNombreContext.Default.ListUsuarioNombre)!).ToList();
            EmpleadoSeleccionado = Empleados.FirstOrDefault()!;
            OnPropertyChanged(nameof(Empleados));
            OnPropertyChanged(nameof(EmpleadoSeleccionado));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar los usuarios: {ex.Message}");
        }
       
    }

    private void ActualizarEventos()
    {
        if (noCargar)
            return;
        var diaUno = new DateOnly(_fechaBusqueda.Year, _fechaBusqueda.Month, 1);
        var diasDelMes = DateTime.DaysInMonth(_fechaBusqueda.Year, _fechaBusqueda.Month);
        var diaInicio = diaUno.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)diaUno.DayOfWeek - 1;
        var DiasTemporal = new List<Dia>();
        for (int i = 0; i < diaInicio; i++)
            DiasTemporal.Add(new Dia());
        for (int i = 1; i <= diasDelMes; i++)
        {
            var dia = new DateOnly(_fechaBusqueda.Year, _fechaBusqueda.Month, i);
            try
            {
                DiasTemporal.Add(new()
                {
                    Numero = i,
                    Eventos = _eventos
                               .Where(e => ((e.Inicio.Day == i && e.Fin is null) || (DateOnly.FromDateTime(e.Inicio) <= dia && dia <= DateOnly.FromDateTime(e.Fin.GetValueOrDefault()))) && (e.Tipo == TipoSeleccionado - 1 || TipoSeleccionado == 0) && (e.UsuarioId == EmpleadoSeleccionado.Id || EmpleadoSeleccionado.Id == -1))
                               .OrderBy(e => e.Inicio).ToList()
                });
            }
            catch (Exception ex)
            {
                i--;
                Console.WriteLine($"Error al procesar el día {i}: {ex.Message} habiendo {_eventos.Count} para elegir");
            }
        }
        Dias = DiasTemporal;
        OnPropertyChanged(nameof(Dias));
    }


    private void VerDia(int dia)
    {
        if (dia > 0 && esPropio)
        {
            DateTime fecha = new(_fechaBusqueda.Year, _fechaBusqueda.Month, dia);
            int _idUsuario = _tipoUsuario == 2 ? Usuario.Id : EmpleadoSeleccionado.Id;
            _navigator.NavigateViewModelAsync<EventosDiaViewModel>(this, qualifier: Qualifiers.ClearBackStack, data: new EntityDateNumber(fecha, _idUsuario));
        }
    }

    protected override void CargarPalabras()
    {
        Tipos = new (){ _localizer["Todos"], _localizer["Tarea"], _localizer["Recordatorio"] };
        var lunes = new DateOnly(2023, 1, 2);
        var martes = lunes.AddDays(1);
        var miercoles = lunes.AddDays(2);
        var jueves = lunes.AddDays(3);
        var viernes = lunes.AddDays(4);
        var sabado = lunes.AddDays(5);
        var domingo = lunes.AddDays(6);
        Lunes_Loc = lunes.ToString("dddd", System.Globalization.CultureInfo.CurrentCulture);
        Martes_Loc = martes.ToString("dddd", System.Globalization.CultureInfo.CurrentCulture);
        Miercoles_Loc = miercoles.ToString("dddd", System.Globalization.CultureInfo.CurrentCulture);
        Jueves_Loc = jueves.ToString("dddd", System.Globalization.CultureInfo.CurrentCulture);
        Viernes_Loc = viernes.ToString("dddd", System.Globalization.CultureInfo.CurrentCulture);
        Sabado_Loc = sabado.ToString("dddd", System.Globalization.CultureInfo.CurrentCulture);
        Domingo_Loc = domingo.ToString("dddd", System.Globalization.CultureInfo.CurrentCulture);
        LunesCorto_Loc = lunes.ToString("ddd", System.Globalization.CultureInfo.CurrentCulture);
        MartesCorto_Loc = martes.ToString("ddd", System.Globalization.CultureInfo.CurrentCulture);
        MiercolesCorto_Loc = miercoles.ToString("ddd", System.Globalization.CultureInfo.CurrentCulture);
        JuevesCorto_Loc = jueves.ToString("ddd", System.Globalization.CultureInfo.CurrentCulture);
        ViernesCorto_Loc = viernes.ToString("ddd", System.Globalization.CultureInfo.CurrentCulture);
        SabadoCorto_Loc = sabado.ToString("ddd", System.Globalization.CultureInfo.CurrentCulture);
        DomingoCorto_Loc = domingo.ToString("ddd", System.Globalization.CultureInfo.CurrentCulture);
        Hoy_Loc = _localizer["Hoy"];
        Calendario_Loc = _localizer["Calendario"];
        Tipo_Loc = _localizer["Tipo"];
        MesAnterior_Loc = _localizer["MesAnterior"];
        MesSiguiente_Loc = _localizer["MesSiguiente"];
        Añadir_Loc = _localizer["Añadir"];
        Empleado_Loc = _localizer["Empleado"];
    }
}


public struct Dia
{
    public int Numero { get; set; }
    public List<EventoMes> Eventos { get; set; }
}

public class EventosDiaViewModel : ViewModelBase
{
    #region Auxiliar
    private DateOnly _fechaBusqueda;
    private readonly int _tipoUsuario = 2;
    private readonly int _idUsuario = 0;
    public Visibility VerNoHayEventos { get => Eventos.Count == 0 ? Visibility.Visible : Visibility.Collapsed; }
    #endregion

    #region Localización
    public string Titulo_Loc { get; set; } = null!;
    private string fecha_Loc = "";
    public string Fecha_Loc { get => fecha_Loc; set { fecha_Loc = value; OnPropertyChanged(nameof(Fecha_Loc)); } }
    public required string NoHayEventos_Loc { get; set; } 
    public required string Añadir_Loc { get; set; }
    #endregion

   
    public Visibility AñadirEventosVisibility { get; set; } = Visibility.Collapsed;

    #region Eventos
    public List<EventoDia> Eventos { get; set; } = null!;

    private EventoDia eventoSeleccionado = null!;
    public EventoDia EventoSeleccionado { get => eventoSeleccionado; set { eventoSeleccionado = value; VerEvento(); } }
    #endregion

    #region Comandos
    public RelayCommand AñadirEventoCommand => new(() =>
    {
        _navigator.NavigateViewModelAsync<AñadirEventoViewModel>(this, qualifier: Qualifiers.ClearBackStack, new EntityDateNumber(new DateTime(_fechaBusqueda, TimeOnly.FromDateTime(DateTime.Now)), _idUsuario));
    });

    public RelayCommand VerDiaAnteriorCommand => new(() =>
    {
        _fechaBusqueda = _fechaBusqueda.AddDays(-1);
#if __WASM__
        CargarEventos();
#else
        CargarEventosComando();
#endif
    });

    public RelayCommand VerDiaSiguienteCommand => new(() =>
    {
        _fechaBusqueda = _fechaBusqueda.AddDays(1);
#if __WASM__
        CargarEventos();
#else
        CargarEventosComando();
#endif
    });
    #endregion

    private readonly EventoApi _eventoApi;
    public EventosDiaViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, EntityDateNumber diaUsuario) : base(localizer, navigator, appInfo)
    {
        _fechaBusqueda = DateOnly.FromDateTime(diaUsuario.date);
        _idUsuario = diaUsuario.number;
        _tipoUsuario = Usuario.Tipo;
        AñadirEventosVisibility = _tipoUsuario < 2 ? Visibility.Visible : Visibility.Collapsed;
        Fecha_Loc = _fechaBusqueda.ToString("dddd dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
        _eventoApi = new EventoApi(Apiurl);
        CargarEventos();
    }
    

    private async void CargarEventosComando()
    {
        try 
        { 
            Fecha_Loc = _fechaBusqueda.ToString("dddd dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
            string? result = null;
            if (_tipoUsuario<2 && _idUsuario <1)
                result = await _eventoApi.GetEventosDiaEmpresaAsync(Usuario.EmpresaId, _fechaBusqueda.Day, _fechaBusqueda.Month, _fechaBusqueda.Year);
            else
                result = await _eventoApi.GetEventosDiaUsuarioAsync(_idUsuario, _fechaBusqueda.Day, _fechaBusqueda.Month, _fechaBusqueda.Year);
            if (result is not null)
            {
                Eventos = JsonSerializer.Deserialize(result, EventoDiaContext.Default.ListEventoDia)!;
                OnPropertyChanged(nameof(Eventos));
                OnPropertyChanged(nameof(VerNoHayEventos));
            }
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#if __WASM__
    private async void CargarEventos()
    {
        try{
            Fecha_Loc = _fechaBusqueda.ToString("dddd dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
            string? result = null;
      
            if (_tipoUsuario<2 && _idUsuario <1)
                result = await _eventoApi.GetEventosDiaEmpresaAsync(Usuario.EmpresaId, _fechaBusqueda.Day, _fechaBusqueda.Month, _fechaBusqueda.Year);
            else
                result = await _eventoApi.GetEventosDiaUsuarioAsync(_idUsuario, _fechaBusqueda.Day, _fechaBusqueda.Month, _fechaBusqueda.Year);
            if (result is not null)
                    Eventos = JsonSerializer.Deserialize(result, EventoDiaContext.Default.ListEventoDia)!;
                
                OnPropertyChanged(nameof(Eventos));
                OnPropertyChanged(nameof(VerNoHayEventos));
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#else
    private void CargarEventos()
    {
        try 
        { 
            Fecha_Loc = _fechaBusqueda.ToString("dddd dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
            string? result = null;
            if (_tipoUsuario < 2 && _idUsuario < 1)
                result = _eventoApi.GetEventosDiaEmpresaAsync(Usuario.EmpresaId, _fechaBusqueda.Day, _fechaBusqueda.Month, _fechaBusqueda.Year).GetAwaiter().GetResult();
            else
                result = _eventoApi.GetEventosDiaUsuarioAsync(_idUsuario, _fechaBusqueda.Day, _fechaBusqueda.Month, _fechaBusqueda.Year).GetAwaiter().GetResult();
            if (result is not null)
                Eventos = JsonSerializer.Deserialize(result, EventoDiaContext.Default.ListEventoDia)!;
        }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#endif

    private void VerEvento()
    {
        _navigator.NavigateViewModelAsync<EventoViewModel>(this, qualifier: Qualifiers.ClearBackStack, data: new EntityNumber(EventoSeleccionado.Id));
    }
    protected override void CargarPalabras()
    {
        Titulo_Loc = _localizer["Eventos del Día"];
        NoHayEventos_Loc = _localizer["NoElementos"];
        Añadir_Loc = _localizer["Añadir"];
    }
}

public class EventoViewModel : ViewModelBase
{
    #region Localización
    private string tipo = string.Empty;
    private string fechaInicio = string.Empty;
    private string fechaFin = string.Empty;
    public string FechaFin { get => fechaFin; set => fechaFin = value; }
    public string FechaInicio { get => fechaInicio; set => fechaInicio = value; }
    public string Tipo { get => tipo; set => tipo = value; }
    public string Editar_Loc { get; set; } = null!;
    public string Eliminar_Loc { get; set; } = null!;
    public required string Añadir_Loc { get; set; }
    public required string Aceptar_Loc { get; set; }
    public required string Cancelar_Loc { get; set; }
    public required string Detalle_Loc { get; set; }
    public required string Cantidad_Loc { get; set; }
    public required string Unidad_Loc { get; set; }
    public required string Completado_Loc { get; set; }
    public required string Nombre_Loc { get; set; }
    public required string Inicio_Loc { get; set; }
    public required string Fin_Loc { get; set; }
    public required string Ubicacion_Loc { get; set; }
    public required string Descripcion_Loc { get; set; }
    public required string Actualizacion_Loc { get; set; }
    public required string Actualizaciones_Loc { get; set; }
    public required string NoElementos_Loc { get; set; }
    #endregion

    #region Comandos
    public ICommand EditarCommand => new RelayCommand(Editar);
    public ICommand EliminarCommand => new RelayCommand(Eliminar);

    public IAsyncRelayCommand<XamlRoot> AñadirActualizacionCommand => new AsyncRelayCommand<XamlRoot>(async (xamlRoot) =>
    {
        var dialog = new ActualizacionEventoContentDialog()
        {
            DataContext = this
        };

        dialog.XamlRoot = xamlRoot;
        ContentDialogResult resultado = await dialog.ShowAsync();
        if (resultado == ContentDialogResult.Primary)
        {
            try 
            {
#if !WINAPPSDK_PACKAGED
                InputPane.GetForCurrentView().TryHide();
#endif
                var insertar = new TareaActualizacionDto() { Cantidad = Cantidad, Fecha = DiaSeleccionado.Date + HoraSeleccionada, TareaDetalleId = Evento.TareaDetalle!.Id, UsuarioId = Usuario.Id };
                var result = await _eventoApi.PostTareaActualizacion(insertar);
                if (result is not null)
                {
                    var actualizacion = JsonSerializer.Deserialize(result, TareaActualizacionContext.Default.TareaActualizacion)!;
                    if (Evento.TareaDetalle!.Actualizaciones is null)
                        Evento.TareaDetalle!.Actualizaciones = new List<TareaActualizacion>();
                    Evento.TareaDetalle!.Actualizaciones!.Add(actualizacion);
                    Actualizaciones = new ObservableCollection<TareaActualizacion>(Evento.TareaDetalle!.Actualizaciones.OrderBy(t => t.Fecha));
                    Finalizada = Evento.TareaDetalle!.Cantidad<= Actualizaciones.Sum(a => a.Cantidad);
                    DiaSeleccionado = DateTimeOffset.Now;
                    HoraSeleccionada = DateTime.Now.TimeOfDay;
                    NoHayActualizacionesVisibility = Visibility.Collapsed;
                    OnPropertyChanged(nameof(NoHayActualizacionesVisibility));
                    OnPropertyChanged(nameof(Actualizaciones));
                }
            }
            catch (HttpRequestException)
            {
                await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
            }
        }
    });
    public ICommand DeleteCommand { get; }

    #endregion

    #region Visibilidad
    public Visibility DetalleVisibility { get => detalleVisibility; set => detalleVisibility = value; }
    public Visibility NoHayActualizacionesVisibility { get => noHayActualizacionesVisibility; set => noHayActualizacionesVisibility = value; }

    private Visibility detalleVisibility = Visibility.Collapsed;
    private Visibility noHayActualizacionesVisibility;
    private Visibility editarEliminarVisibility;
    public Visibility EditarEliminarVisibility { get => editarEliminarVisibility; set => editarEliminarVisibility = value; }
    #endregion

    #region Campos
    private double cantidad;
    private bool finalizada;

    public double Cantidad { get => cantidad; set { cantidad = value; OnPropertyChanged(nameof(Cantidad)); } }
    public DateTimeOffset DiaSeleccionado { get; set; }
    public TimeSpan HoraSeleccionada { get; set; }

    public ObservableCollection<TareaActualizacion> Actualizaciones { get; set; }
    public bool Finalizada { get => finalizada; set  { finalizada = value; 
            OnPropertyChanged(nameof(Finalizada)); }}
    #endregion

    private readonly EventoApi _eventoApi;
    public Evento Evento { get; set; } = null!;
    
    
    public EventoViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, EntityNumber id): base(localizer, navigator, appInfo)
    {
        EditarEliminarVisibility = Usuario.Tipo < 2 ? Visibility.Visible : Visibility.Collapsed;
        DeleteCommand = new RelayCommand<TareaActualizacion>(OnDelete);
        DiaSeleccionado = DateTimeOffset.Now;
        HoraSeleccionada = DateTime.Now.TimeOfDay;
        _eventoApi = new(Apiurl);
        CargarEvento(id.number);
    }
#if __WASM__
    private async void CargarEvento(int id)
    {
        try
        {
            var result = await _eventoApi.GetEvento(id);
                if (result is null)
                    return;
             TerminarCarga(result);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#else
    private void CargarEvento(int id)
    {
        try 
        { 
            var result = _eventoApi.GetEvento(id).GetAwaiter().GetResult();
            if (result is null)
                return;
           TerminarCarga(result);
        }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#endif

    private void TerminarCarga(string result )
    {
        Evento = JsonSerializer.Deserialize(result, EventoContext.Default.Evento)!;
       
        List<string> Tipos = new() { _localizer["Tarea"], _localizer["Recordatorio"] };
        Tipo = Tipos[Evento.Tipo];
        OnPropertyChanged(nameof(Tipo));
        if (Evento.Tipo == 0)
        {
            Finalizada = Evento.TareaDetalle!.Finalizada;
            DetalleVisibility = Visibility.Visible;
            if (Evento.TareaDetalle.Actualizaciones is not null && Evento.TareaDetalle.Actualizaciones.Count > 0)
            {
                NoHayActualizacionesVisibility = Visibility.Collapsed;
                Actualizaciones = new ObservableCollection<TareaActualizacion>(Evento.TareaDetalle.Actualizaciones);
            }
            else
            {
                NoHayActualizacionesVisibility = Visibility.Visible;
            }
        }
        else
            DetalleVisibility = Visibility.Collapsed;
        OnPropertyChanged(nameof(Actualizaciones));
        OnPropertyChanged(nameof(Evento));
        FechaInicio = Evento.Inicio.ToString(System.Globalization.CultureInfo.CurrentCulture);
        FechaFin = Evento.Fin.HasValue ? Evento.Fin.Value.ToString(System.Globalization.CultureInfo.CurrentCulture) : "";
        OnPropertyChanged(nameof(FechaInicio));
        OnPropertyChanged(nameof(FechaFin));
    }



    private async void Editar()
    {
        await _navigator.NavigateViewModelAsync<EditarEventoViewModel>(this, qualifier: Qualifiers.ClearBackStack, data: new EntityNumber(Evento.Id));
    }

    private async void Eliminar()
    {
        try 
        {
            await _eventoApi.DeleteEventoAsync(Evento.Id);
            await _navigator.NavigateViewModelAsync<EventosMesViewModel>(this);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    private void OnDelete(TareaActualizacion? item)
    {
        try 
        {
            if (item != null && Actualizaciones.Contains(item))
            {
                if (Usuario.Tipo== 2 && item.UsuarioId != Usuario.Id)
                {
                    _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["NoEliminarActualizacionesAjena"]);
                    return;
                }
                var result =  _eventoApi.DeleteActualizacion(item.Id);
                Actualizaciones.Remove(item);
                Finalizada = Evento.TareaDetalle!.Cantidad <= Actualizaciones.Sum(ac => ac.Cantidad);
                if (Actualizaciones.Count == 0)
                {
                    NoHayActualizacionesVisibility = Visibility.Visible;
                    OnPropertyChanged(nameof(NoHayActualizacionesVisibility));
                }
                Evento.TareaDetalle!.Actualizaciones!.Remove(item);
            }
        }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    protected override void CargarPalabras()
    {
        Editar_Loc = _localizer["Editar"];
        Eliminar_Loc = _localizer["Eliminar"];
        Añadir_Loc = _localizer["AnadirActualizacion"];
        Aceptar_Loc = _localizer["Aceptar"];
        Cancelar_Loc = _localizer["Cancelar"];
        Detalle_Loc = _localizer["Detalle"];
        Cantidad_Loc = _localizer["Cantidad"];
        Unidad_Loc = _localizer["Unidad"];
        Completado_Loc = _localizer["Completado"];
        Nombre_Loc = _localizer["Nombre"];
        Inicio_Loc = _localizer["Inicio"];
        Fin_Loc = _localizer["Fin"];
        Ubicacion_Loc = _localizer["Ubicacion"];
        Descripcion_Loc = _localizer["Descripcion"];
        Actualizacion_Loc = _localizer["Actualizacion"];
        Actualizaciones_Loc = _localizer["Actualizaciones"];
        NoElementos_Loc = _localizer["NoElementos"];
    }
}


public class EditarEventoViewModel : ViewModelBase
{
    #region Localización
    public required string Eventos_Loc { get; set; }
    public required string Nombre_Loc { get; set; }
    public required string Descripcion_Loc { get; set; }
    public required string Ubicacion_Loc { get; set; }
    public required string Color_Loc { get; set; }
    public required string Inicio_Loc { get; set; }
    public required string Fin_Loc { get; set; }
    public required string Tipo_Loc { get; set; }
    public required string Dia_Loc { get; set; }
    public required string Guardar_Loc { get; set; }
    public required string Cantidad_Loc { get; set; }
    public required string Unidad_Loc { get; set; }
    public required string Titulo_Loc { get; set; }
    public required string Detalle_Loc { get; set; }
    public required string Completado_Loc { get; set; }
    #endregion

    #region Campos
    public string Nombre { get => nombre; set { nombre = value; OnPropertyChanged(nameof(Nombre)); } }
    public string Descripcion { get => descripcion; set { descripcion = value; OnPropertyChanged(nameof(Descripcion)); } }
    public string Ubicacion { get => ubicacion; set { ubicacion = value; OnPropertyChanged(nameof(Ubicacion)); } }
 
    public DateTimeOffset? DiaInicio{ get; set; } = null;
    private string nombre = null!;
    private string descripcion = null!;
    private string ubicacion = null!;
    private double cantidad = 0;
    private string unidad = null!;
    private string _color;
    private bool finalizada = false;
    public TimeSpan? HoraInicio { get; set; } = null;
    public DateTimeOffset? DiaFin { get; set; } = null;
    public TimeSpan? HoraFin { get; set; } = null;
    public double Cantidad { get => cantidad; set => cantidad = value; }
    public string Unidad { get => unidad; set => unidad = value; }
    public List<UsuarioNombre> Empleados { get; set; } = null!;
    public UsuarioNombre EmpleadoSeleccionado { get; set; } = null!;
    public bool Finalizada { get => finalizada; set { finalizada = value; OnPropertyChanged(nameof(Finalizada)); }}
    #endregion



    private readonly EventoApi _eventoApi;
    private readonly int _idEvento;
    public Evento Evento { get; set; } = null!;
    private List<TareaActualizacion> actualizaciones = new List<TareaActualizacion>(); 
    public List<TareaActualizacion> Actualizaciones { get => actualizaciones; set => actualizaciones = value; }

    public ICommand GuardarEventoCommand => new RelayCommand(Guardar);

   

    public Visibility TareaVisibility { get => tareaVisibility; set => tareaVisibility = value; }

    private Visibility tareaVisibility;

    private readonly IMessenger _messenger;
    public EditarEventoViewModel(IMessenger messenger, INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, EntityNumber id) : base(localizer, navigator, appInfo)
    {
        _messenger = messenger;
        _eventoApi = new(Apiurl);
        _idEvento = id.number;
        CargarEvento();
    }
#if __WASM__
  private async void CargarEvento()
    {
        try
        {
            var result = await _eventoApi.GetEvento(_idEvento);
            if (result is null)
                return;
            Evento = JsonSerializer.Deserialize(result, EventoContext.Default.Evento)!;
            CargarCampos(Evento);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#else
    private void CargarEvento()
    {
        try 
        { 
            var result = _eventoApi.GetEvento(_idEvento).GetAwaiter().GetResult();
            if (result is null)
                return;
            Evento = JsonSerializer.Deserialize(result, EventoContext.Default.Evento)!;
            CargarCampos(Evento);
        }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#endif

    private void CargarCampos(Evento evento)
    {
        Nombre = evento.Nombre;
        Descripcion = evento.Descripcion ?? string.Empty;
        Ubicacion = evento.Ubicacion ?? string.Empty;
        DiaInicio = new DateTimeOffset(evento.Inicio);
        HoraInicio = evento.Inicio.TimeOfDay;
        _color = evento.Color;
        if (evento.Fin.HasValue)
        {
            DiaFin = new DateTimeOffset(evento.Fin.Value);
            HoraFin = evento.Fin.Value.TimeOfDay;
            OnPropertyChanged(nameof(HoraFin));
            OnPropertyChanged(nameof(DiaFin));
        }
        else
        {
            DiaFin = null;
            HoraFin = null;
        }
        if (evento.TareaDetalle is not null)
        {
            Cantidad = evento.TareaDetalle?.Cantidad ?? 0;
            Unidad = evento.TareaDetalle?.Unidad ?? string.Empty;
            Finalizada = Evento.TareaDetalle!.Finalizada;
        }
        if (evento.Tipo != 1) 
            TareaVisibility = Visibility.Collapsed;

        OnPropertyChanged(nameof(DiaInicio));
        OnPropertyChanged(nameof(HoraInicio));
    }

    private async void Guardar()
    {
#if !WINAPPSDK_PACKAGED
        InputPane.GetForCurrentView().TryHide();
#endif
        Evento eventoguarda = new Evento()
        {
            Id = Evento.Id,
            Nombre = Nombre,
            Descripcion = Descripcion,
            Ubicacion = Ubicacion,
            Color = _color,
            Inicio = DiaInicio!.Value.Date + HoraInicio!.Value,
            Fin = DiaFin.HasValue ? DiaFin.Value.Date + HoraFin : null,
            Tipo = Evento.Tipo,
            UsuarioId = Evento.UsuarioId,
            EmpresaId = Evento.EmpresaId,
        };
       
        if (Evento.TareaDetalle is not null)
        {
            eventoguarda.TareaDetalle = new TareaDetalle()
            {
                Id = Evento.TareaDetalle.Id,
                Cantidad = Cantidad,
                Unidad = Unidad,
                Finalizada = Finalizada,
                EventoId = Evento.Id
            };
            eventoguarda.Color =  eventoguarda.TareaDetalle.Finalizada?"#FF0BE62F":"#FF0B83E6";
        }
        if (!ValidarModelo(eventoguarda))
        {
            await _navigator.ShowMessageDialogAsync(_localizer["Error"], _localizer[_mensajeError]);
            return;
        }
        try
        {
            var result = await _eventoApi.PutEventoAsync(eventoguarda);
            if (result is not null)
            {
                if(!(bool)TransientSettings.Get("Calendario"))
                    _messenger.Send(new Eventosaved(true));
                else
                    await _navigator.NavigateViewModelAsync<EventosMesViewModel>(this, qualifier: Qualifiers.ClearBackStack);
            }
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
    protected override void CargarPalabras()
    {
        Titulo_Loc = _localizer["EditarEvento"];
        Nombre_Loc = _localizer["Nombre"];
        Descripcion_Loc = _localizer["Descripcion"];
        Ubicacion_Loc = _localizer["Ubicacion"];
        Color_Loc = _localizer["Color"];
        Inicio_Loc = _localizer["Inicio"];
        Fin_Loc = _localizer["Fin"];
        Tipo_Loc = _localizer["Tipo"];
        Dia_Loc = _localizer["Dia"];
        Cantidad_Loc = _localizer["Cantidad"];
        Unidad_Loc = _localizer["Unidad"];
        Guardar_Loc = _localizer["Guardar"];
        Detalle_Loc = _localizer["Detalle"];
        Completado_Loc = _localizer["Completado"];
    }
}

public partial class AñadirEventoViewModel : ViewModelBase
{
#region Propiedades
    #region Apis
    private readonly EventoApi _eventoApi;
    private readonly UsuarioApi _usuarioApi;
    #endregion

    #region Localización
    public required string Nombre_Loc { get; set; }
    public required string Descripcion_Loc { get; set; }
    public required string Ubicacion_Loc { get; set; }
    public required string Color_Loc { get; set; }
    public required string Inicio_Loc { get; set; }
    public required string Fin_Loc { get; set; }
    public required string Tipo_Loc { get; set; }
    public required string Dia_Loc { get; set; }
    public required string Añadir_Loc { get; set; }
    public required string Cantidad_Loc { get; set; }
    public required string Unidad_Loc { get; set; }
    public required string Empleado_Loc { get; set; }
    public required string Titulo_Loc { get; set; }
    #endregion

    #region Campos
    public string Nombre { get => nombre; set { nombre = value; OnPropertyChanged(nameof(Nombre)); } }
    public string Descripcion { get => descripcion; set { descripcion = value; OnPropertyChanged(nameof(Descripcion)); } }
    public string Ubicacion { get => ubicacion; set { ubicacion = value; OnPropertyChanged(nameof(Ubicacion)); } }
    private DateTimeOffset _diaInicio;
    public DateTimeOffset DiaInicio
    {
        get => _diaInicio;
        set
        {
            if (_diaInicio != value)
            {
                _diaInicio = value;
                OnPropertyChanged(nameof(DiaInicio));
            }
        }
    }
    private int tipoSeleccionado = 0;
    private string nombre = null!;
    private string descripcion = null!;
    private string ubicacion = null!;
    private int cantidad = 0;
    private string unidad = null!;
    public TimeSpan HoraInicio { get; set; }
    public DateTimeOffset? DiaFin { get; set; } = null;
    public TimeSpan? HoraFin { get; set; } = null;
    public List<string> Tipos { get; set; } = null!;
    public int TipoSeleccionado { get => tipoSeleccionado; set { tipoSeleccionado = value; TareaVisibility = value == 0 ? Visibility.Visible : Visibility.Collapsed; OnPropertyChanged(nameof(TipoSeleccionado)); } }
    public int Cantidad { get => cantidad; set => cantidad = value; }
    public string Unidad { get => unidad; set => unidad = value; }
    public List<UsuarioNombre> Empleados { get; set; } = null!;
    public UsuarioNombre EmpleadoSeleccionado { get; set; } = null!;
    #endregion

    #region Comandos
    public ICommand AñadirEventoCommand { get; set; }
    #endregion

    #region Visibilidad
    private Visibility tareaVisibility;
    public Visibility TareaVisibility { get => tareaVisibility; set { tareaVisibility = value; OnPropertyChanged(nameof(TareaVisibility)); } }
    public Visibility EmpleadoVisibility { get; set; }
    #endregion

    private readonly int _usuarioId;
    public EventoDto Evento { get; set; } = null!;
#endregion
    public AñadirEventoViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo) : base(localizer, navigator, appInfo)
    {
        _eventoApi = new(Apiurl);
        _usuarioApi = new(Apiurl);
        _usuarioId = Usuario.Id;
        AñadirEventoCommand = new RelayCommand(Guardar);
        EmpleadoVisibility = Usuario.Tipo <2 ? Visibility.Visible : Visibility.Collapsed;
        if( EmpleadoVisibility == Visibility.Visible)
            CargarUsuarios();
    }

    public AñadirEventoViewModel (INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, EntityDateNumber datos): this ( navigator, localizer, appInfo)
    {
        DiaInicio = new DateTimeOffset(datos.date);
        _usuarioId = datos.number;
        HoraInicio = datos.date.TimeOfDay;
    }
#if __WASM__
    private async void CargarUsuarios()
    {
        try
        {
            var result = await _usuarioApi.GetNombreUsuariosEmpresa(Usuario.EmpresaId);
            TerminarCarga(result);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#else
    private void CargarUsuarios()
    {
        try
        {
            var result = _usuarioApi.GetNombreUsuariosEmpresa(Usuario.EmpresaId).GetAwaiter().GetResult();
            TerminarCarga(result);
        }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#endif
    private void TerminarCarga (string? result)
    {
        if (result is not null)
        Empleados = JsonSerializer.Deserialize(result, UsuarioNombreContext.Default.ListUsuarioNombre)!;
        EmpleadoSeleccionado = Empleados.FirstOrDefault(e => e.Id == _usuarioId) ?? Empleados.FirstOrDefault()!;
        OnPropertyChanged(nameof(Empleados));
        OnPropertyChanged(nameof(EmpleadoSeleccionado));
    }

    private async void Guardar()
    {
#if !WINAPPSDK_PACKAGED
        InputPane.GetForCurrentView().TryHide();
#endif
        try
        {
            if (TipoSeleccionado == 0 && (Unidad is null || Cantidad == 0))
            {
                var acciones = new[]
                {
                    new DialogAction(_localizer["Aceptar"].ToString()),
                    new DialogAction(_localizer["Cancelar"].ToString())
                };
                string texto = _localizer["DetallesTareaAlerta"];
                string[] parts = texto.Split('\\');
                var textoUnidad = Unidad ?? "nan";
                string textoseparado = parts[0] + "\n" + parts[1] +" "+ Cantidad + "\n" + parts[2] +" "+textoUnidad;
                string? resultado = await _navigator.ShowMessageDialogAsync<string>(this, title: _localizer["Error"], content: textoseparado, buttons: acciones);
                if (resultado != _localizer["Aceptar"].ToString())
                    return;
            }
            Evento = new EventoDto
            {
                Nombre = Nombre,
                Descripcion = Descripcion,
                Ubicacion = Ubicacion,
                Color = TipoSeleccionado ==0 ?"#FF0B83E6": "#FFFFFFFF",
                Inicio = DiaInicio.Date + HoraInicio,
                Fin = DiaFin.HasValue ? DiaInicio.Date + HoraInicio : null,
                Tipo = TipoSeleccionado,
                UsuarioId = EmpleadoSeleccionado is null ? _usuarioId : EmpleadoSeleccionado.Id,
                EmpresaId = Usuario.EmpresaId,
                TareaDetalle = TipoSeleccionado == 0 ? new TareaDetalleDto
                {
                    Cantidad = Cantidad,
                    Unidad = Unidad ?? "nan"
                } : null
            };
            if (!ValidarModelo(Evento))
            {
                await _navigator.ShowMessageDialogAsync(_localizer["Error"], _localizer[_mensajeError]);
                return;
            }
            var result = "";
            if (Evento.TareaDetalle is null)
                result = await _eventoApi.PostEventoAsync(Evento);
            else
                result = await _eventoApi.PostTareaAsync(Evento);
            if (string.IsNullOrEmpty(result))
                return;
            else
                await _navigator.NavigateViewModelAsync<EventosMesViewModel>(this, qualifier: Qualifiers.ClearBackStack);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    protected override void CargarPalabras()
    {
        Tipos = new() { _localizer["Tarea"], _localizer["Recordatorio"] };
        Nombre_Loc = _localizer["Nombre"];
        Descripcion_Loc = _localizer["Descripcion"];
        Ubicacion_Loc = _localizer["Ubicacion"];
        Color_Loc = _localizer["Color"];
        Inicio_Loc = _localizer["Inicio"];
        Fin_Loc = _localizer["Fin"];
        Tipo_Loc = _localizer["Tipo"];
        Añadir_Loc = _localizer["Añadir"];
        Dia_Loc = _localizer["Dia"];
        Cantidad_Loc = _localizer["Cantidad"];
        Unidad_Loc = _localizer["Unidad"];
        Empleado_Loc = _localizer["Empleado"];
        Titulo_Loc = _localizer["CrearEvento"];
    }
}
