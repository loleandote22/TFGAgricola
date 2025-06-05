namespace AplicacionTFG.Presentation; 
using System.Text;
using System.Text.Json;

public partial class MainViewModel : ViewModelBase
{
    private Usuario usuarioLocal;
    #region Localización
    private string inicio_Loc;
    private string inventario_Loc;
    private string personal_Loc;
    public string Inicio_Loc { get => inicio_Loc; set { inicio_Loc = value; OnPropertyChanged(nameof(Inicio_Loc)); }}
    public string Inventario_Loc { get => inventario_Loc; set {inventario_Loc = value; OnPropertyChanged(nameof(Inventario_Loc));}}
    public string Personal_Loc { get => personal_Loc; set {personal_Loc = value; OnPropertyChanged(nameof(Personal_Loc));}}
    #endregion
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public MainViewModel() { }
    public MainViewModel(IStringLocalizer localizer, ILocalizationService localizationService, INavigator navigator, IOptions<AppConfig> appInfo): base(localizer, navigator, appInfo, localizationService)
    {
        Title = localizer["ApplicationName"];
        Console.WriteLine($"Usuario: {Usuario.Nombre}");
        Console.WriteLine($"Titulo: {Title}");
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public string? Title { get; }
    public Usuario UsuarioLocal { get => usuarioLocal; set => usuarioLocal = value; }

    protected override void CargarPalabras()
    {
        Inicio_Loc = _localizer["Inicio"];
        Inventario_Loc = _localizer["Inventario"];
        Personal_Loc = _localizer["Personal"];
    }
}
