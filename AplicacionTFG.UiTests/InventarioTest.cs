using NUnit.Framework;

namespace AplicacionTFG.UiTests;
public partial class TestsAplicacion : TestBase
{
    [Test]
    public void InventarioCreacion()
    {
        Inventario_Navigation();
        App.WaitForElement("AñadirInventarioButton");
        App.Tap("AñadirInventarioButton");
        App.ClearText("CantidadInventario");
        App.EnterText("NombreInventario", "prueba123");
        App.EnterText("DescripcionInventario", "prueba123");
        App.EnterText("CantidadInventario", "10");
        App.ClearText("UnidadInventario");
        App.EnterText("UnidadInventario", "Unidades");
        App.WaitForElement("TipoInventario");
        App.Tap("TipoInventario");
        App.Tap(q => q.Text("Maquinaria"));
        App.Tap("GuardarInventario");
        App.WaitForElement("prueba123");
    }
    
}
