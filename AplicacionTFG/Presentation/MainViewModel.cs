using Uno.Extensions.Navigation.Regions;
namespace AplicacionTFG.Presentation;

public partial class MainViewModel : ViewModelBase
{
    private readonly INavigator _navigator;
    private readonly Usuario _usuario;

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public MainViewModel() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public MainViewModel(IStringLocalizer localizer, INavigator navigator, IOptions<AppConfig> appInfo, Usuario usuario)
    {
        _usuario = usuario;
        _navigator = navigator;
        Title = localizer["ApplicationName"];
        Console.WriteLine($"Usuario: {_usuario.Nombre}");
        Console.WriteLine($"Titulo: {Title}");
        //GoToSecond = new AsyncRelayCommand(GoToSecondView);
    }
    public string? Title { get; }

    private async Task GoToSecondView()
    {
       // await _navigator.NavigateBackAsync(this);
        await _navigator.NavigateViewModelAsync<SecondViewModel>(this, data: new Entity(Title!));
    }
}
