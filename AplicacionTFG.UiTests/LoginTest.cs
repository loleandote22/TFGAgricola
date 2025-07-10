using NUnit.Framework;
using Uno.UITest.Helpers.Queries;
using Query = System.Func<Uno.UITest.IAppQuery, Uno.UITest.IAppQuery>;

namespace AplicacionTFG.UiTests;
[TestFixture]
public partial class TestsAplicacion: TestBase
{
    [Test]
    public void LoginTest_ValidCredentials()
    {
        App.WaitForElement("LoginButton");
        App.EnterText("LoginName", "prueba123");
        App.EnterText("LoginPassw", "prueba123");
        App.Tap("LoginButton");
        App.WaitForElement("HomePage"); 
        Assert.That(App.Query("HomePage").Any());
    }

    [Test]
    public void LoginTest_InvalidCredentials()
    {
        App.WaitForElement("LoginButton");
        App.EnterText("LoginName", "wronguser");
        App.EnterText("LoginPassw", "wrongpassword");
        Query loginButtonQuery = q => q.Marked("LoginButton");
        App.Tap("LoginButton");
        Query contentQuery = q => q.Marked("Content");
        App.WaitForElement("Content");
        Assert.That(App.Query(q => contentQuery(q).GetDependencyPropertyValue("Content").Value<string>()).First().Equals("Nombre o contraseña incorrectos"));
    }

    [Test]
    public void LoginTest_EmptyCredentials()
    {
        App.WaitForElement("LoginButton");
        App.Tap("LoginButton");
        Query contentQuery = q => q.Marked("Content");
        Assert.That(App.Query(q => contentQuery(q).GetDependencyPropertyValue("Content").Value<string>()).First().Equals("Por favor, introduce un nombre de usuario y una contraseña"));
    }
}
