using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;

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
        var context = new ValidationContext(modelo);
        bool isValid = Validator.TryValidateObject(modelo, context, validationResults, true);

        // Limpiar errores previos
        _mensajeError = validationResults.Count > 0 ? validationResults[0].ErrorMessage : string.Empty;

        // Agregar nuevos errores si los hay
        //foreach (var validationResult in validationResults)
        //{
        //    Errores.Add(validationResult.ErrorMessage);
        //}

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
            _localizationService.SetCurrentCultureAsync(new CultureInfo(value.Simbolo + "-" + value.Simbolo.ToUpper().Replace("PT", "BR")));
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
    private Usuario _usuario;
    public Usuario Usuario { get => _usuario; set => _usuario = value; }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

    public ViewModelBase() { }

    public ViewModelBase(IStringLocalizer localizer, INavigator navigator, IOptions<AppConfig> appInfo)
    {
        _localizer = localizer;
        _navigator = navigator;
        string usuario = (string)localSettings.Values["Usuario"];
        if (usuario is not null)
            Usuario = JsonSerializer.Deserialize<Usuario>(usuario);
        Apiurl = appInfo.Value.ApiUrl ?? throw new ArgumentNullException(nameof(appInfo.Value.ApiUrl), "La URL de la API no puede ser nula.");
        CargarPalabras();
    }

    //public ViewModelBase() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

    public ViewModelBase(IStringLocalizer localizer, INavigator navigator, IOptions<AppConfig> appInfo, ILocalizationService? localizationService = null) :this(localizer, navigator, appInfo) 
    {
        if (localizationService is not null)
            _localizationService = localizationService;
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


