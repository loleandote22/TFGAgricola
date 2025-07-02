using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;

namespace AplicacionTFG.Presentation;
public abstract class ViewModelBase : ObservableObject
{
    protected ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
    #region Validaciones
    protected string _mensajeError = "Las contraseñas no coinciden";
    /// <summary>
    /// Valida un modelo utilizando Data Annotations.
    /// </summary>
    /// <param name="modelo"></param>
    /// <returns> True si los campos son válidos
    /// False si los campos no son válidos
    /// </returns>
    public bool ValidarModelo(object modelo)
    {
        List<ValidationResult> validationResults = new ();
#pragma warning disable IL2026
        var context = new ValidationContext(modelo);
        bool isValid = Validator.TryValidateObject(modelo, context, validationResults, true);
#pragma warning restore IL2026
        _mensajeError = validationResults.Count > 0 ? validationResults[0].ErrorMessage! : string.Empty;
        return isValid;
    }
    #endregion

    #region Localización
    public List<Idioma> Idiomas { get; } = new()
    {
        new Idioma("Español", "es"),
        new Idioma("English","en"),
        new Idioma("Français", "fr"),
        new Idioma("Português", "pt"),
    };

    private Idioma idiomaSeleccionado;


    public Idioma IdiomaSeleccionado
    {
        get => idiomaSeleccionado;
        set
        {
            idiomaSeleccionado = value;
            if (_localizationService is not null && value.Simbolo is not null)
            {
                _localizationService.SetCurrentCultureAsync(new CultureInfo(value.Simbolo + "-" + value.Simbolo.ToUpper().Replace("PT", "BR")));
                CultureInfo.CurrentCulture = new CultureInfo(value.Simbolo + "-" + value.Simbolo.ToUpper().Replace("PT", "BR"));
            }
            CargarPalabras();
            OnPropertyChanged(nameof(IdiomaSeleccionado));
        }
    }

    

    protected abstract void CargarPalabras();

    protected readonly ILocalizationService _localizationService;

    protected readonly IStringLocalizer _localizer;
    #endregion  
    protected string Apiurl;
    protected readonly INavigator _navigator;
    protected readonly IOptions<AppConfig> _appInfo;
    private Usuario _usuario;
    public Usuario Usuario { get => _usuario; set => _usuario = value; }
#pragma warning disable CS8618
    public ViewModelBase() { }

    public ViewModelBase(IStringLocalizer localizer, INavigator navigator, IOptions<AppConfig> appInfo)
    {
        _localizer = localizer;
        _navigator = navigator;
        _appInfo = appInfo;
        Usuario = (Usuario)TransientSettings.Get("Usuario");
        Apiurl = appInfo.Value.ApiUrl ?? throw new ArgumentNullException(nameof(appInfo.Value.ApiUrl), "La URL de la API no puede ser nula.");
        CargarPalabras();
    }
#pragma warning restore CS8618

    public ViewModelBase(IStringLocalizer localizer, INavigator navigator, IOptions<AppConfig> appInfo, ILocalizationService? localizationService = null) : this(localizer, navigator, appInfo) 
    {
        if (localizationService is not null)
        {
            _localizationService = localizationService;
            IdiomaSeleccionado = Idiomas.FirstOrDefault(x => x.Simbolo == _localizationService.CurrentCulture.Name[..2]);
        }
    }
}

public struct Idioma
{
    public string Lengua { get; set; }
    public string Simbolo { get; set; }
    public string Bandera { get; set; }

    public Idioma(string lengua, string simbolo)
    {
        Simbolo = simbolo;
        Lengua = lengua;
        Bandera = "/Assets/flags/" + simbolo.Replace("-", "_").Replace("en", "gb") + ".png";
    }
}
