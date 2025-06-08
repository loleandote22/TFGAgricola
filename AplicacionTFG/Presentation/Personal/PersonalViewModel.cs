using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionTFG.Presentation.Personal;
public class PersonalViewModel: ViewModelBase
{
    protected override void CargarPalabras()
    {
        // Cargar palabras en el idioma seleccionado
        // Ejemplo: _localizationService.SetCurrentCultureAsync(new CultureInfo(IdiomaSeleccionado.Simbolo));
    }
}
