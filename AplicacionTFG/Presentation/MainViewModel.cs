namespace AplicacionTFG.Presentation;

public partial class MainViewModel : ObservableObject
{
    private INavigator _navigator;
    private Usuario _usuario;
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
    public MainViewModel(INavigator navigator, Usuario usuario)
    {
        _usuario = usuario;
        _navigator = navigator;
        Title = "Main";
        GoToSecond = new AsyncRelayCommand(GoToSecondView);
    }
    public string? Title { get; }

    public ICommand GoToSecond { get; }

    private async Task GoToSecondView()
    {
        await _navigator.NavigateViewModelAsync<SecondViewModel>(this, data: new Entity(Title!));
    }

}
