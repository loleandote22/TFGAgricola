using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionTFG.Presentation;

public class InicioViewModel :ViewModelBase
{
    private readonly Dictionary<string, DateOnly> estaciones = new Dictionary<string, DateOnly>()
    {
        { "Invierno", new DateOnly(2000, 3, 20) },
        { "Primavera", new DateOnly(2000, 6, 21) },
        { "Verano", new DateOnly(2000, 9, 23) },
        { "Otoño", new DateOnly(2000, 12, 21) }
    };

    private readonly INavigator _navigator;
    private string imagenFondo;

    public string ImagenFondo { get => imagenFondo; set { imagenFondo = value; }}
    public InicioViewModel(INavigator navigator)
    {
        _navigator = navigator;
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
    protected override void CargarPalabras()
    {
        //throw new NotImplementedException();
    }
}
