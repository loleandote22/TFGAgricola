using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AplicacionTFG.Presentation.Personal;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;

namespace AplicacionTFG.Presentation.Perfil;
public class PerfilViewModel: ViewModelBase
{

    #region Auxiliar
    private string directorioImagenes = "/Assets/UserIcons/";
    private List<string> imagenes = new();
    public List<string> Imagenes { get => imagenes; set => imagenes = value; }
    private bool funcional;
    public bool Funcional { get => funcional; set { funcional = value; OnPropertyChanged(nameof(Funcional)); }}

    public Idioma IdiomaPerfil { get => IdiomaSeleccionado; set { IdiomaSeleccionado = value; _messenger.Send(new Languagechanged(value)); }}

    private readonly IMessenger _messenger;

    #endregion

    #region Visibilidad
    private Visibility verRoles = Visibility.Collapsed;
    public Visibility VerRoles { get => verRoles; set => verRoles = value; }

    private Visibility verIdiomas;
    public Visibility VerIdiomas { get => verIdiomas; set => verIdiomas = value; }

    private Visibility verPeligro;
    public Visibility VerPeligro { get => verPeligro; set => verPeligro = value; }
    #endregion

    #region Localización
    private string nombre_Loc = "";
    private string tipo_Loc = "";
    private string contraseña_Loc = "";
    private string confirmarContraseña_Loc = "";
    private string guardar_Loc = "";
    private string pregunta_Loc = "";
    private string respuesta_Loc = "";
    private string aceptar_Loc = "";
    private string cancelar_Loc = "";
    private string zonaPeligrosa_Loc = "";

    private string eliminar_Loc = "";
    public string Nombre_Loc { get => nombre_Loc; set { nombre_Loc = value; OnPropertyChanged(nameof(Nombre_Loc)); } }
    public string Tipo_Loc { get => tipo_Loc; set { tipo_Loc = value; OnPropertyChanged(nameof(Tipo_Loc)); } }

    public string Contraseña_Loc { get => contraseña_Loc; set { contraseña_Loc = value; OnPropertyChanged(nameof(Contraseña_Loc)); }}
    public string ConfirmarContraseña_Loc { get => confirmarContraseña_Loc; set { confirmarContraseña_Loc = value; OnPropertyChanged(nameof(ConfirmarContraseña_Loc)); }}
    public string Guardar_Loc { get => guardar_Loc; set { guardar_Loc = value; OnPropertyChanged(nameof(Guardar_Loc)); }}
    public string Pregunta_Loc { get => pregunta_Loc; set { pregunta_Loc = value; OnPropertyChanged(nameof(Pregunta_Loc)); }}
    public string Respuesta_Loc { get => respuesta_Loc; set { respuesta_Loc = value; OnPropertyChanged(nameof(Respuesta_Loc)); } }
    public string Aceptar_Loc { get => aceptar_Loc; set { aceptar_Loc = value; OnPropertyChanged(nameof(Aceptar_Loc)); }}
    public string Cancelar_Loc { get => cancelar_Loc; set { cancelar_Loc = value; OnPropertyChanged(nameof(Cancelar_Loc)); }}
    public string ZonaPeligrosa_Loc { get => zonaPeligrosa_Loc; set { zonaPeligrosa_Loc = value; OnPropertyChanged(nameof(ZonaPeligrosa_Loc)); }}
    public string Eliminar_Loc { get => eliminar_Loc; set { eliminar_Loc = value; OnPropertyChanged(nameof(Eliminar_Loc)); }}
    #endregion

    #region Campos
    private string nombre = "";
    private string contraseña = "";
    private string confirmarContraseña = "";
    private string pregunta = "";
    private string respuesta = "";
    private string imagenSeleccionada = "";
    private string imagenPerfil = "";
    private int rolSeleccionado = -1;

    public List<string> Roles { get; set; }
    public int RolSeleccionado { get => rolSeleccionado; set { Funcional = true; rolSeleccionado = value; }}
    public string Nombre { get => nombre; set { Funcional = true; nombre = value; OnPropertyChanged(nameof(Nombre)); } }
    public string Contraseña { get => contraseña; set { Funcional = true; contraseña = value; OnPropertyChanged(nameof(Contraseña)); } }
    public string ConfirmarContraseña { get => confirmarContraseña; set { confirmarContraseña = value; OnPropertyChanged(nameof(ConfirmarContraseña)); } }
    public string Respuesta { get => respuesta; set { Funcional = true; respuesta = value; OnPropertyChanged(nameof(Respuesta)); } }
    public string Pregunta { get => pregunta; set { Funcional = true; pregunta = value; OnPropertyChanged(nameof(Pregunta)); } }
    public string ImagenSeleccionada { get => imagenSeleccionada; set { Funcional = true; imagenSeleccionada = value; OnPropertyChanged(nameof(ImagenSeleccionada)); } }
    public string ImagenPerfil { get => imagenPerfil; set { Funcional = true; imagenPerfil = value; OnPropertyChanged(nameof(ImagenPerfil)); } }
    #endregion

    #region Comandos
    public ICommand GuardarCommand { get ; set ; }
    public IAsyncRelayCommand<XamlRoot> SeleccionarImagenCommand => new AsyncRelayCommand<XamlRoot>(async (xamlRoot) =>
    {
        var dialog = new ImagenesContentDialog()
        {
            DataContext = this
        };

        dialog.XamlRoot = xamlRoot;
        ContentDialogResult resultado = await dialog.ShowAsync();
        if (resultado == ContentDialogResult.Primary)
        {
            ImagenPerfil = ImagenSeleccionada;
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["ImagenActualizada"], content: _localizer["NoOlvidesGuardar"]);
        }

    });

    public IAsyncRelayCommand<XamlRoot> EliminarPerfilCommand => new AsyncRelayCommand<XamlRoot>(async (xamlRoot) =>
    {
        ContentDialog dialog = new ContentDialog
        {
            Title = _localizer["Eliminar"],
            Content = _localizer["EliminarIrreversible"],
            PrimaryButtonText = _localizer["Aceptar"],
            SecondaryButtonText = _localizer["Cancelar"],
            XamlRoot = xamlRoot
        };
        ContentDialogResult resultado = await dialog.ShowAsync();
        if (resultado == ContentDialogResult.Primary)
        {
            var result = await _usuarioApi.DeleteUsuarioAsync(Usuario.Id);
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["PerfilEliminado"], content: _localizer["PerfilEliminadoCorrectamente"]);
            await _navigator.NavigateBackAsync(this);
        }
    });



    #endregion

    private readonly UsuarioApi _usuarioApi;

    public Usuario UsuarioActualizar { get; set; }

    public PerfilViewModel(IMessenger messenger, IStringLocalizer localizer, ILocalizationService localizationService, INavigator navigator, IOptions<AppConfig> appInfo):base(localizer, navigator, appInfo, localizationService)
    {
        List<string> RolesTemp = [_localizer["Dueno"], _localizer["Administrador"], _localizer["Empleado"]];
        UsuarioActualizar = Usuario;
        Roles = RolesTemp.GetRange(Usuario.Tipo, 3-Usuario.Tipo);
        _messenger = messenger;
        IdiomaPerfil = IdiomaSeleccionado;
        if (_localizationService is not null)
        {
            CargarCampos();
            VerIdiomas = Visibility.Visible;
        }
        else
            VerIdiomas = Visibility.Collapsed;
        _usuarioApi = new UsuarioApi(Apiurl);
        GuardarCommand = new RelayCommand(Guardar);
    }

    public void CargarCampos()
    {
        ImagenPerfil = directorioImagenes + UsuarioActualizar.Imagen;
        ImagenSeleccionada = ImagenPerfil;
        Imagenes = _appInfo.Value.Icons!;
        Nombre = UsuarioActualizar.Nombre;
        RolSeleccionado = UsuarioActualizar.Tipo-Usuario.Tipo;
        VerRoles = Usuario.Tipo < 2 && UsuarioActualizar.Tipo >= Usuario.Tipo ? Visibility.Visible : Visibility.Collapsed;
        Pregunta = UsuarioActualizar.Pregunta;
        Funcional = false;
    }

    private async void Guardar()
    {
#if !WINAPPSDK_PACKAGED
        InputPane.GetForCurrentView().TryHide();
#endif
        UsuarioAcutliazarDto usuarioActualizado = new UsuarioAcutliazarDto
        {
            Nombre = Nombre,
            Tipo = RolSeleccionado+Usuario.Tipo,
            Imagen =ImagenPerfil.Substring(ImagenPerfil.LastIndexOf('/')+1),
            Contrasena = Contraseña.Length>0 ? Contraseña: "default123",
            Pregunta = Pregunta.Length>0 ? Pregunta : UsuarioActualizar.Pregunta,
            Respuesta = Respuesta.Length>0 ? Respuesta : UsuarioActualizar.Respuesta,
            EmpresaId = UsuarioActualizar.EmpresaId
        };
        if (Contraseña != ConfirmarContraseña)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer[_mensajeError]);
            return;
        }
        if (!ValidarModelo(usuarioActualizado))
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer[_mensajeError]);
            return;
        }
        try
        {
            var result = await _usuarioApi.PutUsuarioAsync(Usuario.Id, usuarioActualizado);
            if (result is not null)
            {
                localSettings.Values["Usuario"] = result;
            }
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Perfil"], content: _localizer["ExitoGuardado"]);

            TransientSettings.Set("Usuario", Usuario);
            if (_localizationService is not null)
                Funcional = false;
            else
                await _navigator.NavigateViewModelAsync<PersonalViewModel>(this);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }

    protected override void CargarPalabras()
    {
        Nombre_Loc = _localizer["Nombre"];
        Tipo_Loc = _localizer["Tipo"];
        Contraseña_Loc = _localizer["Contraseña"];
        ConfirmarContraseña_Loc = _localizer["ConfirmarContraseña"];
        Guardar_Loc = _localizer["Guardar"];
        Pregunta_Loc = _localizer["Pregunta"];
        Respuesta_Loc = _localizer["Respuesta"];
        Aceptar_Loc = _localizer["Aceptar"];
        Cancelar_Loc = _localizer["Cancelar"];
        ZonaPeligrosa_Loc = _localizer["ZonaPeligrosa"];
        Eliminar_Loc = _localizer["Eliminar"];
    }
}
