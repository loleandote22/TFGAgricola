using NUnit.Framework;
namespace AplicacionTFG.UiTests;
[TestFixture]
public partial class TestsAplicacion : TestBase
{
    [Test]
    public void Inventario_Navigation()
    {
        LoginTest_ValidCredentials();
        App.WaitForElement("InventarioNav");
        App.Tap("InventarioNav");
        App.WaitForElement("NoHayInventario");
    }

    [Test]
    public void Personal_Navigation()
    {
        LoginTest_ValidCredentials();
        App.WaitForElement("PersonalNav");
        App.Tap("PersonalNav");
        App.WaitForElement("prueba123");
    }

    [Test]
    public void Inicio_Navigation()
    {
        LoginTest_ValidCredentials();
        App.WaitForElement("InicioNav");
        App.Tap("InicioNav");
    }
}
