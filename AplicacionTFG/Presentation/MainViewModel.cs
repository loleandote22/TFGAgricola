namespace AplicacionTFG.Presentation; 
using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.Messaging;

public partial class MainViewModel : ViewModelBase
{

    public Visibility VerPersonal { get; set; } = Visibility.Collapsed;
    #region Localización
    private string inicio_Loc;
    private string inventario_Loc;
    private string personal_Loc;
    private string salir_Loc;
    private string perfil_Loc;
    public string Calendario_Loc { get; set; }
    public string Inicio_Loc { get => inicio_Loc; set { inicio_Loc = value; OnPropertyChanged(nameof(Inicio_Loc)); }}
    public string Inventario_Loc { get => inventario_Loc; set {inventario_Loc = value; OnPropertyChanged(nameof(Inventario_Loc));}}
    public string Personal_Loc { get => personal_Loc; set {personal_Loc = value; OnPropertyChanged(nameof(Personal_Loc));}}
    public string Salir_Loc { get => salir_Loc; set => salir_Loc = value; }
    public string Perfil_Loc { get => perfil_Loc; set => perfil_Loc = value; }
    #endregion

#pragma warning disable CS8618
    public MainViewModel(IMessenger messenger, IStringLocalizer localizer, ILocalizationService localizationService, INavigator navigator, IOptions<AppConfig> appInfo): base(localizer, navigator, appInfo, localizationService)
    {
        messenger.Register<Languagechanged>(this, (r,msg) => { IdiomaSeleccionado = msg.idioma; CargarPalabras(); });
        Title = localizer["ApplicationName"];
        VerPersonal = Usuario.Tipo == "Dueño" || Usuario.Tipo == "Administrador" ? Visibility.Visible : Visibility.Collapsed;
    }
#pragma warning restore CS8618
    public string? Title { get; }

    protected override void CargarPalabras()
    {
        Calendario_Loc = _localizer["Calendario"];
        Inicio_Loc = _localizer["Inicio"];
        Inventario_Loc = _localizer["Inventario"];
        Personal_Loc = _localizer["Personal"];
        Salir_Loc = _localizer["Salir"];
        Perfil_Loc = _localizer["Perfil"];
    }
}
public record Languagechanged(Idioma idioma);
