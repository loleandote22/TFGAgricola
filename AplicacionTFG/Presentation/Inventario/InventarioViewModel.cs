namespace AplicacionTFG.Presentation.Inventario;
public class InventarioViewModel : ViewModelBase
{
    private Models.Inventario? inventarioSeleccionado;
    public List<Models.Inventario>? Inventarios { get; set; }
    public Models.Inventario? InventarioSeleccionado 
    { 
        get => inventarioSeleccionado; 
        set 
        {
            if (value is null) return; // No hay cambio
            inventarioSeleccionado = value; 
            ElementoControlVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(InventarioSeleccionado)); 
        } 
    }

    public Visibility ElementoControlVisibility
    {
        get => elementoControlVisibility; 
        set 
        { 
            elementoControlVisibility = value;
            InventarioSeleccionado = null; // Reset the selected item when visibility changes
            OnPropertyChanged(nameof(ElementoControlVisibility)); 
        } 
    }
    public ICommand VolverCommand => new RelayCommand(Volver);

   
    private Visibility elementoControlVisibility;
    private readonly INavigator _navigator;
    public InventarioViewModel(INavigator navigator)
    {
        _navigator = navigator;
        var algo = _navigator.Route;
        CargarInventario();
        ElementoControlVisibility = Visibility.Collapsed;
    }

    private void CargarInventario()
    {
        Inventarios = new () {
            new Models.Inventario() {Id=0, Nombre="Nombre", Tipo="Tipo", Descripcion="Descripcion",  Cantidad=1, EmpresaId=1 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 },
            new Models.Inventario(){Id=1, Nombre="asdf", Tipo="asdf", Descripcion="asdf",  Cantidad=21, EmpresaId=21 }
        };
        OnPropertyChanged(nameof(Inventarios));
    }

    
    private void Volver()
    {
        ElementoControlVisibility = Visibility.Collapsed;
    }
}
