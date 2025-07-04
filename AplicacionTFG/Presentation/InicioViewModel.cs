using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AplicacionTFG.Presentation.Eventos;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;

namespace AplicacionTFG.Presentation;

public class InicioViewModel :ViewModelBase
{
    #region Auxiliar
    public string Color { get; } = "#80E8E8E8";
    #endregion
    #region Localización
    public required string EventosHoy_Loc { get; set; }
    public required string TareasPendientes_Loc { get; set; }
    public required string Bienvenida_Loc { get; set; }
    public required string NoHayEventos_Loc { get; set; }
    #endregion

    private readonly Dictionary<string, DateOnly> estaciones = new()
    {
        { "Invierno", new DateOnly(2000, 3, 20) },
        { "Primavera", new DateOnly(2000, 6, 21) },
        { "Verano", new DateOnly(2000, 9, 23) },
        { "Otoño", new DateOnly(2000, 12, 21) }
    };
    #region Eventos
    private List<EventoDia> eventos = new List<EventoDia>();
    public List<EventoDia> Eventos { get => eventos; set { eventos = value; OnPropertyChanged(nameof(Eventos)); OnPropertyChanged(nameof(VerNoHayEventos)); }}

    private List<EventoDia> tareasPendientes;
    public List<EventoDia> TareasPendientes { get => tareasPendientes; set { tareasPendientes = value; OnPropertyChanged(nameof(TareasPendientes)); OnPropertyChanged(nameof(VerNoHayTareasPendientes)); }}
    private readonly EventoApi _eventoApi;

    private EventoDia eventoSeleccionado;
    public EventoDia EventoSeleccionado { get => eventoSeleccionado; set { eventoSeleccionado = value; VerEvento(); } }
    public Visibility VerNoHayEventos { get => Eventos.Count == 0 ? Visibility.Visible : Visibility.Collapsed; }
    public Visibility VerNoHayTareasPendientes { get; set; } = Visibility.Visible;
    #endregion
    private string imagenFondo;


    public string ImagenFondo { get => imagenFondo; set { imagenFondo = value; }}


#pragma warning disable CS8618
    public InicioViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo): base(localizer, navigator, appInfo)
    {
        _eventoApi = new EventoApi(Apiurl);
        Bienvenida_Loc = _localizer["Hola"] + ", " + Usuario.Nombre;
        ImagenFondo = "/Assets/Images/Inicio/Inicio";
        DateOnly fecha =new (2000, DateTime.Now.Month, DateTime.Now.Day);
        if (fecha  <= estaciones["Invierno"])
        {
            ImagenFondo += "Invierno.png";
        }
        else if (fecha <= estaciones["Primavera"])
        {
            ImagenFondo += "Primavera.png";
        }
        else if (fecha <= estaciones["Verano"])
        {
            ImagenFondo += "Verano.png";
        }
        else if (fecha < estaciones["Otoño"])
        {
            ImagenFondo += "Otoño.png";
        }

        TransientSettings.Set("Calendario", false);
        OnPropertyChanged(nameof(ImagenFondo));
        CargarEventos();
        CargarTareasPendientes();
    }

#if __WASM__
     private async void CargarEventos()
    {
        try
        {
            var hoy = DateTime.Now;
            var result = await _eventoApi.GetEventosDiaUsuarioAsync(Usuario.Id, hoy.Day, hoy.Month, hoy.Year);
            if (result is not null)
                Eventos = JsonSerializer.Deserialize(result, EventoDiaContext.Default.ListEventoDia)!;
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    private async  void CargarTareasPendientes()
    {
        try
        {
            var result = await _eventoApi.GetTareasPendientes(Usuario.Id);
            if (result is not null)
                TareasPendientes = JsonSerializer.Deserialize(result, EventoDiaContext.Default.ListEventoDia)!;
            VerNoHayTareasPendientes = TareasPendientes is null || TareasPendientes.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            OnPropertyChanged(nameof(VerNoHayTareasPendientes));
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
            var hoy = DateTime.Now;
            var result = _eventoApi.GetEventosDiaUsuarioAsync(Usuario.Id, hoy.Day, hoy.Month, hoy.Year).GetAwaiter().GetResult();
            if (result is not null)
                Eventos = JsonSerializer.Deserialize(result, EventoDiaContext.Default.ListEventoDia)!;
        }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    private void CargarTareasPendientes()
    {
        try
        {
            var result = _eventoApi.GetTareasPendientes(Usuario.Id).GetAwaiter().GetResult();
            if (result is not null)
                TareasPendientes = JsonSerializer.Deserialize(result, EventoDiaContext.Default.ListEventoDia)!;
            VerNoHayTareasPendientes = TareasPendientes is null || TareasPendientes.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            OnPropertyChanged(nameof(VerNoHayTareasPendientes));
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


#pragma warning restore CS8618
    protected override void CargarPalabras()
    {
        EventosHoy_Loc = _localizer["EventosHoy"];
        TareasPendientes_Loc = _localizer["TareasPendientes"];
        Bienvenida_Loc = _localizer["Hola"] + ", " + Usuario.Nombre;
        NoHayEventos_Loc = _localizer["NoElementos"];
    }
}
