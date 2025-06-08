using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace AplicacionTFG.Droid;
[Activity(
    MainLauncher = true,
    ConfigurationChanges = global::Uno.UI.ActivityHelper.AllConfigChanges,
    WindowSoftInputMode = SoftInput.AdjustNothing | SoftInput.StateHidden,
    ScreenOrientation = ScreenOrientation.Portrait
)]
public class MainActivity : Microsoft.UI.Xaml.ApplicationActivity
{
}
