using NUnit.Framework;
using Uno.UITest.Helpers.Queries;
using System.Linq;
using Query = System.Func<Uno.UITest.IAppQuery, Uno.UITest.IAppQuery>;

namespace AplicacionTFG.UiTests;
public class LoginTest: TestBase
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
       // App.Tap(loginButtonQuery);
        App.WaitForElement("Content");
        Query contentQuery = q => q.Marked("Content");
        Assert.That(App.Query(q => contentQuery(q).GetDependencyPropertyValue("Content").Value<string>()).First().Equals("Error en el login")); // Check if the login button is disabled after failed login
        App.PressEnter();
        Assert.That(App.Query(q => loginButtonQuery(q).GetDependencyPropertyValue("IsEnabled").Value<bool>()).First()); // Check if still on login page 
    }
    [Test]
    public void LoginTest_EmptyCredentials()
    {
        App.WaitForElement("LoginButton");
        App.Tap("LoginButton"); // Attempt to login without entering credentials
        // Optionally, you can check for an error message or validation feedback
        Query contentQuery = q => q.Marked("Content");
        Assert.That(App.Query(q => contentQuery(q).GetDependencyPropertyValue("Content").Value<string>()).First().Equals("Error en el login")); // Assuming there's an error message element
    }
}
