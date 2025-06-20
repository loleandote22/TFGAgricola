using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionTFG.Presentation.Eventos;
public class EventosMesViewModel : ViewModelBase
{
    #region Localización
    public string Calendario_Loc { get; set; }
    #endregion
    public required string Titulo_Loc { get; set; }
    public List<Dia> Dias { get; set; } = new List<Dia>();
    public EventosMesViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo) : base(localizer, navigator, appInfo)
    {
        CargarEventos();
    }
    private void CargarEventos()
    {
        // Aquí se cargarían los eventos desde una fuente de datos, como una API o base de datos.
        // Por ahora, se deja vacío para que puedas implementar la lógica más adelante.
    }

    protected override void CargarPalabras()
    {
        Calendario_Loc = _localizer["Calendario"];
    }
}

public struct Evento
{
    public string Nombre { get; set; }
    public string Hora { get; set; }
}

public struct Dia
{
    public int Numero { get; set; }
    public List<Evento> Eventos { get; set; }
}

