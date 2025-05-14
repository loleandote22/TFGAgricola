namespace AplicacionTFG.Presentation;

public partial class MainViewModel : ViewModelBase
{
    private INavigator _navigator;
    private Usuario _usuario;
    public ICommand GoToSecond { get; }
    //public MainViewModel(
    //    IStringLocalizer localizer,
    //    IOptions<AppConfig> appInfo,
    //    INavigator navigator)
    //{
    //    _navigator = navigator;
    //    Title = "Main";
    //    Title += $" - {localizer["ApplicationName"]}";
    //    Title += $" - {appInfo?.Value?.Environment}";
    //    GoToSecond = new AsyncRelayCommand(GoToSecondView);
    //}
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public MainViewModel() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public MainViewModel(IStringLocalizer localizer, INavigator navigator, Usuario usuario)
    {
        _usuario = usuario;
        _navigator = navigator;
        Title = localizer["ApplicationName"];
        GoToSecond = new AsyncRelayCommand(GoToSecondView);
    }
    public string? Title { get; }

    private async Task GoToSecondView()
    {
        await _navigator.NavigateViewModelAsync<SecondViewModel>(this, data: new Entity(Title!));
    }
}
