using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;
using Windows.Devices.Input;

namespace AplicacionTFG.Presentation.Eventos;
public class EventosMesViewModel : ViewModelBase
{
    #region Auxiliar
    private DateOnly _fechaBusqueda;
    private readonly int _idUsuario = 0;
    private readonly string _tipoUsuario = "Empleado";
    private int diaSeleccionado;
    public int DiaSeleccionado { get => diaSeleccionado; set { diaSeleccionado = value; VerDia(value); }}
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
        _navigator.NavigateViewModelAsync<AñadirEventoViewModel>(this, qualifier: Qualifiers.ClearBackStack);
    });
    #endregion


    public List<string> Tipos { get; set; } = null!;

    private int tipoSeleccionado = 0;
    public int TipoSeleccionado { get => tipoSeleccionado; set { tipoSeleccionado = value; ActualizarEventos(); }}

    private readonly EventoApi _eventoApi;
    private List<EventoMes> _eventos = null!;
    // 42 dias para mostrar eventos
    public List<Dia> Dias { get; set; } = null!;
    public EventosMesViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo) : base(localizer, navigator, appInfo)
    {
        _tipoUsuario = Usuario.Tipo;
        _idUsuario = Usuario.Id;
        _eventoApi = new EventoApi(Apiurl);
        _fechaBusqueda = DateOnly.FromDateTime(DateTime.Now);
        CargarEventos( _fechaBusqueda.Month, _fechaBusqueda.Year);
    }

    public EventosMesViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, int idUsuario) : base(localizer, navigator, appInfo)
    {
        _idUsuario = idUsuario;
        _eventoApi = new EventoApi(Apiurl);
        _fechaBusqueda = DateOnly.FromDateTime(DateTime.Now);
        CargarEventos(_fechaBusqueda.Month, _fechaBusqueda.Year);
    }
#if __WASM__
    private async void CargarEventos( int mes, int año)
    {
        Fecha_Loc = _fechaBusqueda.ToString("MMMM - yyyy", System.Globalization.CultureInfo.CurrentCulture);
        if (_tipoUsuario == "Empleado")
        {
            var result =await _eventoApi.GetEventosMesUsuarioAsync(_idUsuario, mes , año);
            TerminarCarga(result);
        }
        else{
            var result =await _eventoApi.GetEventosMesEmpresaAsync(Usuario.EmpresaId, mes , año);
            TerminarCarga(result);
        }
    }
#else
    private void CargarEventos(int mes, int año)
    {
        Fecha_Loc = _fechaBusqueda.ToString("MMMM - yyyy", System.Globalization.CultureInfo.CurrentCulture);
        if(_tipoUsuario == "Empleado")
        { 
            var result = _eventoApi.GetEventosMesUsuarioAsync(_idUsuario, mes, año).GetAwaiter().GetResult();
            TerminarCarga(result);
        }
        else
        {
            var result = _eventoApi.GetEventosMesEmpresaAsync(Usuario.EmpresaId, mes, año).GetAwaiter().GetResult();
            TerminarCarga(result);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mes"></param>
    /// <param name="año"></param>
    private async void CargarEventosComando(int mes, int año)
    {
        Fecha_Loc = _fechaBusqueda.ToString("MMMM - yyyy", System.Globalization.CultureInfo.CurrentCulture);
        if (_tipoUsuario == "Empleado")
        {
            var result = await _eventoApi.GetEventosMesUsuarioAsync(_idUsuario, mes, año);
            TerminarCarga(result);
        }
        else
        {
            var result = await _eventoApi.GetEventosMesEmpresaAsync(Usuario.EmpresaId, mes, año);
            TerminarCarga(result);
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
        _eventos = JsonSerializer.Deserialize(result, EventoMesContext.Default.ListEventoMes)!;
        
        ActualizarEventos();
    }

    private void ActualizarEventos()
    {
        var diaUno = new DateOnly(_fechaBusqueda.Year, _fechaBusqueda.Month, 1);
        var diasDelMes = DateTime.DaysInMonth(_fechaBusqueda.Year, _fechaBusqueda.Month);
        var diaInicio = diaUno.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)diaUno.DayOfWeek - 1;
        var DiasTemporal = new List<Dia>();
        for (int i = 0; i < diaInicio; i++)
            DiasTemporal.Add(new Dia());
        for (int i = 1; i <= diasDelMes; i++)
        {
            DiasTemporal.Add(new () { Numero = i, Eventos = _eventos
                .Where(e => e.Inicio.Day == i && (e.Tipo == TipoSeleccionado||TipoSeleccionado==0))
                .OrderBy(e => e.Inicio).ToList() });
        }
        Dias = DiasTemporal;
        OnPropertyChanged(nameof(Dias));
    }


    private void VerDia(int dia)
    {
        if (dia > 0)
        {
            var fecha = new DateOnly(_fechaBusqueda.Year, _fechaBusqueda.Month, dia);
            _navigator.NavigateViewModelAsync<EventosDiaViewModel>(this, qualifier: Qualifiers.ClearBackStack, data: new EntityDate(fecha));
        }
    }

    protected override void CargarPalabras()
    {
        Tipos = new (){ _localizer["Todos"], _localizer["Evento"], _localizer["Tarea"], _localizer["Recordatorio"] };
        var lunes = new DateOnly(2023, 1, 2); // Primer lunes de 2023
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
    private readonly DateOnly _fechaBusqueda;
    private readonly string _tipoUsuario = "Empleado";
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

   

    public string FechaSeleccionada { get; set; }

    #region Eventos
    public List<EventoDia> Eventos { get; set; } = null!;

    private EventoDia eventoSeleccionado = null!;
    public EventoDia EventoSeleccionado { get => eventoSeleccionado; set { eventoSeleccionado = value; VerEvento(); } }
    #endregion

    #region Comandos
    public RelayCommand AñadirEventoCommand => new(() =>
    {
        _navigator.NavigateViewModelAsync<AñadirEventoViewModel>(this, qualifier: Qualifiers.ClearBackStack);
    });
    #endregion

    private readonly EventoApi _eventoApi;
    public EventosDiaViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, EntityDate dia) : base(localizer, navigator, appInfo)
    {
        _fechaBusqueda = dia.date;
        _tipoUsuario = Usuario.Tipo;
        FechaSeleccionada = _fechaBusqueda.ToString("dddd dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
        _eventoApi = new EventoApi(Apiurl);
        CargarEventos();
    }
    public EventosDiaViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, DateOnly dia, int idUsuario) : base(localizer, navigator, appInfo)
    {
        _idUsuario = idUsuario;
        _fechaBusqueda = dia;
        FechaSeleccionada = _fechaBusqueda.ToString("dddd dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
        _eventoApi = new EventoApi(Apiurl);
        CargarEventos();
    }
#if __WASM__
     private async void CargarEventos()
    {
        Fecha_Loc = _fechaBusqueda.ToString("MMMM - yyyy", System.Globalization.CultureInfo.CurrentCulture);

      if (_tipoUsuario == "Empleado")
        {
            var result = await _eventoApi.GetEventosDiaUsuarioAsync(_idUsuario, _fechaBusqueda.Day, _fechaBusqueda.Month, _fechaBusqueda.Year);
            if (result is not null)
                Eventos = JsonSerializer.Deserialize(result, EventoDiaContext.Default.ListEventoDia)!;
        }
        else
        {
            var result = await _eventoApi.GetEventosDiaEmpresaAsync(Usuario.EmpresaId, _fechaBusqueda.Day, _fechaBusqueda.Month, _fechaBusqueda.Year);
            if (result is not null)
                Eventos = JsonSerializer.Deserialize(result, EventoDiaContext.Default.ListEventoDia)!;
        }
        //TerminarCarga(result, _fechaBusqueda.Day, _fechaBusqueda.Month, _fechaBusqueda.Year);
    }
#else
    private void CargarEventos()
    {
        Fecha_Loc = _fechaBusqueda.ToString("MMMM - yyyy", System.Globalization.CultureInfo.CurrentCulture);
      if(_tipoUsuario == "Empleado")
        {
            var result = _eventoApi.GetEventosDiaUsuarioAsync(_idUsuario, _fechaBusqueda.Day, _fechaBusqueda.Month, _fechaBusqueda.Year).GetAwaiter().GetResult();
            if (result is not null)
                Eventos = JsonSerializer.Deserialize(result, EventoDiaContext.Default.ListEventoDia)!;
        }
        else
        {
            var result = _eventoApi.GetEventosDiaEmpresaAsync(Usuario.EmpresaId, _fechaBusqueda.Day, _fechaBusqueda.Month, _fechaBusqueda.Year).GetAwaiter().GetResult();
            if (result is not null)
                Eventos = JsonSerializer.Deserialize(result, EventoDiaContext.Default.ListEventoDia)!;
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
    private readonly EventoApi _eventoApi;
    public Evento Evento { get; set; } = null!;
    public EventoViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo, EntityNumber id): base(localizer, navigator, appInfo)
    {
        _eventoApi = new(Apiurl);
        CargarEvento(id.number);
    }
    private void CargarEvento(int id)
    {
        var result = _eventoApi.GetEvento(id).GetAwaiter().GetResult();
        if (result is null)
            return;
        Evento = JsonSerializer.Deserialize(result, EventoContext.Default.Evento)!;
        Console.WriteLine($"Evento cargado: {Evento.Nombre}");
    }
    protected override void CargarPalabras()
    {
       
    }
}

public class AñadirEventoViewModel : ViewModelBase
{

    #region Localización
    public required string Nombre_Loc { get; set; }
    public required string Descripcion_Loc { get; set; }
    public required string Ubicacion_Loc { get; set; }
    public required string Color_Loc { get; set; }
    public required string Inicio_Loc { get; set; }
    public required string Fin_Loc { get; set; }
    public required string Tipo_Loc { get; set; }

    public required string Añadir_Loc { get; set; }
    #endregion
    #region Campos
    #endregion
    private readonly EventoApi _eventoApi;
    public EventoDto Evento { get; set; } = null!;
    public AñadirEventoViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo) : base(localizer, navigator, appInfo)
    {
        _eventoApi = new(Apiurl);
    }
    public async Task<bool> AñadirEventoAsync()
    {
        var result = await _eventoApi.PostEventoAsync(Evento);
        return !string.IsNullOrEmpty(result);
    }
    protected override void CargarPalabras()
    {
        Nombre_Loc = _localizer["Nombre"];
        Descripcion_Loc = _localizer["Descripcion"];
        Ubicacion_Loc = _localizer["Ubicacion"];
        Color_Loc = _localizer["Color"];
        Inicio_Loc = _localizer["Inicio"];
        Fin_Loc = _localizer["Fin"];
        Tipo_Loc = _localizer["Tipo"];
        Añadir_Loc = _localizer["Añadir"];
    }
}
