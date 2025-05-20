using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionTFG.Presentation.Inventario;
public class ElementoViewModel : ViewModelBase
{
    public Models.Inventario? Elemento { get; set; }
    public ICommand VolverCommand => new RelayCommand(volver);
    private readonly INavigator _navigator;
    public ElementoViewModel(INavigator navigator, Models.Inventario? elemento)
    {
        _navigator = navigator;
        var algo = _navigator.Route;
        Elemento = elemento;
    }
    private async void volver()
    {
        await _navigator.NavigateBackAsync(this);
    }
}
