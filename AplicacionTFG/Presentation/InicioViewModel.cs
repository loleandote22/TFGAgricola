using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionTFG.Presentation;

public class InicioViewModel
{
    private readonly INavigator _navigator;
    public InicioViewModel(INavigator navigator)
    {
        _navigator = navigator;
    }
}
