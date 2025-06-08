using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionTFG.Presentation;

public class InicioViewModel :ViewModelBase
{
    private readonly Dictionary<string, DateOnly> estaciones = new()
    {
        { "Invierno", new DateOnly(2000, 3, 20) },
        { "Primavera", new DateOnly(2000, 6, 21) },
        { "Verano", new DateOnly(2000, 9, 23) },
        { "Otoño", new DateOnly(2000, 12, 21) }
    };

    private string imagenFondo;

    public string ImagenFondo { get => imagenFondo; set { imagenFondo = value; }}
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public InicioViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo): base(localizer, navigator, appInfo)
    {
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
        OnPropertyChanged(nameof(ImagenFondo));
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    protected override void CargarPalabras()
    {

    }
}
