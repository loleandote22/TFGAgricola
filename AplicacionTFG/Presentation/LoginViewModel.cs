using System.Text.Json;
using AplicacionTFG.Services;
using System.Globalization;
// TODO: Controlar excepción no API connectada
namespace AplicacionTFG.Presentation;
public class LoginViewModel: ViewModelBase
{
    #region Auxiliares
    private readonly INavigator _navigator;
    private readonly IStringLocalizer _localizer;
    private readonly ILocalizationService _localizationService;
    private int indice;
    public int Indice { get => indice; set { indice = value; LimpiarCampos(); OnPropertyChanged(nameof(Indice)); } }
    #endregion
    #region Localización
    public string Usuario_Loc { get; set; }
    public string Contraseña_Loc { get; set; }
    public string Iniciar_Loc { get; set; }
    public string ConfirmarContra_Loc { get; set; }
    public string Tipo_Loc { get; set; }
    public string Pregunta_Loc { get; set; }
    public string Respuesta_Loc { get; set; }
    public string NombreEmpresa_Loc { get; set; }
    public string ContraseñaEmpresa_Loc { get; set; }
    public string Registrar_Loc { get; set; }
    public string Restablecer_Loc { get; set; }
    public string NuevaContra_Loc { get; set; }
    public string Recuperar_Loc { get; set; }
    public string CambiarContra_Loc { get; set; }
    public string CambiarUsuario_Loc { get; set; }

    #endregion
    #region Commandos
    public ICommand CambiarAEmpresaCommand { get; }
    public ICommand CambiarAUsuarioCommand { get; }
    public ICommand PreguntarCommand { get; }
    public ICommand ResponderCommand { get; }
    public ICommand CambiarContraCommand { get; }
    public ICommand RegistrarCommand { get; }
    public ICommand LoginCommand { get; }
    #endregion
    #region Visibilidades
    public Visibility VerRegistroUsuario { get; set; } = Visibility.Visible;
    public Visibility VerRegistroEmpresa { get; set; } = Visibility.Collapsed;
    public Visibility VerRecuperacionUsuario { get; set; } = Visibility.Visible;
    public Visibility VerRecuperacionPregunta { get; set; } = Visibility.Collapsed;
    public Visibility VerRecuperacionContraseña { get; set; } = Visibility.Collapsed;
    #endregion
    #region Strings
    public string NombeUsuarioLogin { get; set; } = string.Empty;
    public string ContraLogin { get; set ; } = string.Empty;
    public string NombeUsuarioRegistro { get; set; } = string.Empty;
    public string ContraRegistro { get; set; } = string.Empty;
    public string ContraRegistroConfirm { get; set; } = string.Empty;
    public string PreguntaRegistro { get; set; } = string.Empty;
    public string RespuestaRegistro { get; set; } = string.Empty;
    public List<string> RolesRegistro { get; set; } = [ "Dueño", "Administrador", "Empleado" ]; // Cambiar a un array de strings
    public string RolRegistro { get; set; } = string.Empty;
    public string NombreEmpresa { get; set; } = string.Empty;
    public string ContraEmpresa { get; set; } = string.Empty;
    public string NombeUsuarioRecuperar { get; set; } = string.Empty;
    public string PreguntaRecuperar { get; set; } = string.Empty;
    public string RespuestaRecuperar { get; set; } = string.Empty;
    public string ContraRecuperar { get; set; } = string.Empty;
    public string ContraRecuperarConfirm { get; set; } = string.Empty;
    #endregion
   
    public bool Funcional { get => funcional; set { funcional = value; OnPropertyChanged(nameof(Funcional)); } }
    public Usuario? Usuario { get; set; }
    private UsuarioRegistroDto? usuarioRegistro = null;
    private Usuario? usuario = null;
    private readonly UsuarioApi _usuarioApi = new();
    private readonly EmpresaApi _empresaApi = new();
    private bool funcional = true;

    public LoginViewModel(IStringLocalizer localizer,ILocalizationService localizationService, IOptions<AppConfig> appInfo, INavigator navigator)
    {
        Indice =0;
        _localizer = localizer;
        _localizationService = localizationService;
        IdiomaSeleccionado = Idiomas.FirstOrDefault(x => x.Simbolo == _localizationService.CurrentCulture.Name);
        //_localizationService.SetCurrentCultureAsync(new CultureInfo("fr-FR"));
        Usuario_Loc = _localizer["Usuario"];
        Contraseña_Loc = _localizer["Contraseña"];
        Iniciar_Loc = _localizer["IniciarSesion"];
        ConfirmarContra_Loc = _localizer["ConfirmarContraseña"];
        Tipo_Loc = _localizer["Tipo"];
        Pregunta_Loc = _localizer["Pregunta"];
        Respuesta_Loc = _localizer["Respuesta"];
        NombreEmpresa_Loc = _localizer["NombreEmpresa"];
        ContraseñaEmpresa_Loc = _localizer["ContraseñaEmpresa"];
        Registrar_Loc = _localizer["Registrar"];
        Restablecer_Loc = _localizer["Restablecer"];
        NuevaContra_Loc = _localizer["Nueva"]+ " " + Contraseña_Loc;
        CambiarContra_Loc = _localizer["Cambiar"] + " " + Contraseña_Loc;
        Recuperar_Loc = _localizer["RecuperarContraseña"];
        CambiarUsuario_Loc = _localizer["CambiarUsuario"];
        _navigator = navigator;
        Funcional = true;
        CambiarAEmpresaCommand = new RelayCommand(ComprobarUsuario);
        CambiarAUsuarioCommand = new RelayCommand(CambiarAUsuario);
        PreguntarCommand = new RelayCommand(Preguntar);
        ResponderCommand = new RelayCommand(Responder);
        CambiarContraCommand = new RelayCommand(CambiarContra);
        RegistrarCommand = new RelayCommand(Registrar);
        LoginCommand = new RelayCommand(Login);
    }

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public LoginViewModel() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

    private async void Preguntar()
    {
        Funcional = false;
        //await _navigator.ShowMessageDialogAsync(this,title:"Recuperar contraseña", content:"Fase 1 no implementada");
        try
        {
            var pregunta = await _usuarioApi.Preguntar(NombeUsuarioRecuperar);
            if (pregunta == null)
            {
                await _navigator.ShowMessageDialogAsync(this, title: _localizer["RecuperarContraseña"], content: "El usuario no existe");
                Funcional = true;
                return;
            }
            
            PreguntaRecuperar ="¿" + pregunta + "?";
            OnPropertyChanged(nameof(PreguntaRecuperar));
            VerRecuperacionUsuario = Visibility.Collapsed;
            VerRecuperacionPregunta = Visibility.Visible;
            OnPropertyChanged(nameof(VerRecuperacionUsuario));
            OnPropertyChanged(nameof(VerRecuperacionPregunta));
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["RecuperarContraseña"], content: "Error de conexión con el servidor");
        }
        finally
        {
            Funcional = true;
        }
    }

    private async void Responder()
    {
        Funcional = false;
        UsuarioRespuestaDto usuarioRespuestaDto = new()
        {
            Nombre = NombeUsuarioRecuperar,
            Respuesta = RespuestaRecuperar
        };
        var result = await _usuarioApi.Responder(usuarioRespuestaDto);
        if (result == null)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["RecuperarContraseña"], content: "La respuesta es incorrecta");
            Funcional = true;
            return;
        }
        usuario = JsonSerializer.Deserialize<Usuario>(result);
        VerRecuperacionPregunta = Visibility.Collapsed;
        VerRecuperacionContraseña = Visibility.Visible;
        OnPropertyChanged(nameof(VerRecuperacionPregunta));
        OnPropertyChanged(nameof(VerRecuperacionContraseña));
        Funcional = true;
    }

    private async void CambiarContra()
    {
        Funcional = false;
        if (ContraRecuperar != ContraRecuperarConfirm)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["RecuperarContraseña"], content: "Las contraseñas no coinciden");
            Funcional = true;
            return;
        }
        usuario!.Contrasena = ContraRecuperar;
        var result = await _usuarioApi.PutUsuarioAsync(usuario);
        if (result == null)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["RecuperarContraseña"], content: "Error al cambiar la contraseña");
            Funcional = true;
            return;
        }
        Usuario? usuarioDevuelto = JsonSerializer.Deserialize<Usuario>(result);
        if (usuarioDevuelto == null)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["RecuperarContraseña"], content: "Error al cambiar la contraseña");
            Funcional = true;
            return;
        }
        ResetearFormularios();
        await _navigator.NavigateViewModelAsync<MainViewModel>(this, data: usuarioDevuelto);
        Funcional = true;
    }

    private async void ComprobarUsuario()
    {
        usuarioRegistro = new UsuarioRegistroDto()
        {
            Nombre = NombeUsuarioRegistro,
            Contrasena = ContraRegistro,
            Pregunta = PreguntaRegistro,
            Respuesta = RespuestaRegistro,
            Tipo = RolRegistro
        };
        _mensajeError = "Las contraseñas no coinciden";
        if (ContraRegistro != ContraRegistroConfirm || ValidarModelo(usuarioRegistro) == false)
        {
            await _navigator.ShowMessageDialogAsync(this, title: "Registrar usuario", content: _mensajeError);
            return;
        }
        if (RolRegistro == "Dueño")
            await _navigator.ShowMessageDialogAsync(this, title: "Registrar usuario", content: "A continuación crea tu empresa.");
        else
            await _navigator.ShowMessageDialogAsync(this, title: "Registrar usuario", content: "A continuación únete a tu equipo.");
        VerRegistroUsuario = Visibility.Collapsed;
        VerRegistroEmpresa = Visibility.Visible;
        OnPropertyChanged(nameof(VerRegistroUsuario));
        OnPropertyChanged(nameof(VerRegistroEmpresa));
    }
    
    private void CambiarAUsuario()
    {
        VerRegistroUsuario = Visibility.Visible;
        VerRegistroEmpresa = Visibility.Collapsed;
        OnPropertyChanged(nameof(VerRegistroUsuario));
        OnPropertyChanged(nameof(VerRegistroEmpresa));
    }

    private async void Registrar()
    {
        Funcional = false;
        EmpresaDto empresa = new()
        {
            Nombre = NombreEmpresa,
            Contrasena = ContraEmpresa
        };
        if(ValidarModelo(empresa) == false)
        {
            await _navigator.ShowMessageDialogAsync(this, title: "Registrar empresa", content: _mensajeError);
            return;
        }
        string? result;
        try
        {
            if (usuarioRegistro!.Tipo == "Dueño")
                result = await _empresaApi.PostEmpresaAsync(empresa);
            else
                result = await _empresaApi.Login(empresa);
            if (result is null)
            {
                Funcional = true;
                await _navigator.ShowMessageDialogAsync(this, title: "Registrar empresa", content: "Error al registrar la empresa");
                return;
            }
            Empresa? empresaDevuelta = JsonSerializer.Deserialize<Empresa>(result);
            usuarioRegistro.EmpresaId = empresaDevuelta!.Id;
            result = await _usuarioApi.PostUsuarioAsync(usuarioRegistro);
            if (result is not null)
            {
                await _navigator.ShowMessageDialogAsync(this, title: "Registrar usuario", content: "Usuario registrado correctamente");
                Usuario? usuarioDevuelto = JsonSerializer.Deserialize<Usuario>(result);
                ResetearFormularios();
                await _navigator.NavigateViewModelAsync<MainViewModel>(this, data: usuarioDevuelto);
            }
            else
            {
                await _navigator.ShowMessageDialogAsync(this, title: "Registrar usuario", content: "Error al registrar el usuario");
                VerRegistroUsuario = Visibility.Collapsed;
                VerRegistroEmpresa = Visibility.Visible;
                OnPropertyChanged(nameof(VerRegistroUsuario));
                OnPropertyChanged(nameof(VerRegistroEmpresa));
            }
            Funcional = true;
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: "Registrar empresa", content: "Error de conexión con el servidor");
            Funcional = true;
            return;
        }
    }
    
    private async void Login()
    {
        Funcional = false;
        if (string.IsNullOrEmpty(NombeUsuarioLogin) || string.IsNullOrEmpty(ContraLogin))
            await _navigator.ShowMessageDialogAsync(this, title: "Login", content: "Por favor, introduce un nombre de usuario y una contraseña.");
        else
        {
            UsuarioDto usuario = new()
            {
                Nombre = NombeUsuarioLogin,
                Contrasena = ContraLogin
            };
            try
            {
                var result = await _usuarioApi.Login(usuario);
                if (result is not null)
                {
                    Usuario? usuarioDevuelto = JsonSerializer.Deserialize<Usuario>(result);
                    ResetearFormularios();
                    await _navigator.NavigateViewModelAsync<MainViewModel>(this, data: usuarioDevuelto);
                }
                else
                    await _navigator.ShowMessageDialogAsync(this, title: "Login", content: "Error en el login");
            }
            catch (HttpRequestException)
            {
                await _navigator.ShowMessageDialogAsync(this, title: "Login", content: "Error de conexión con el servidor");
                Funcional = true;
                return;
            }
        }
        Funcional = true;
    }
    
    private void LimpiarCampos()
    {
        NombeUsuarioLogin = string.Empty;
        ContraLogin = string.Empty;
        NombeUsuarioRegistro = string.Empty;
        ContraRegistro = string.Empty;
        RolRegistro = string.Empty;
        PreguntaRegistro = string.Empty;
        RespuestaRegistro = string.Empty;
        ContraRegistroConfirm = string.Empty;
        NombeUsuarioRecuperar = string.Empty;
        VerRegistroUsuario = Visibility.Visible;
        VerRegistroEmpresa = Visibility.Collapsed;
        NombreEmpresa = string.Empty;
        ContraEmpresa = string.Empty;
        OnPropertyChanged(nameof(NombeUsuarioLogin));
        OnPropertyChanged(nameof(ContraLogin));
        OnPropertyChanged(nameof(NombeUsuarioRegistro));
        OnPropertyChanged(nameof(ContraRegistro));
        OnPropertyChanged(nameof(RolRegistro));
        OnPropertyChanged(nameof(PreguntaRegistro));
        OnPropertyChanged(nameof(RespuestaRegistro));
        OnPropertyChanged(nameof(ContraRegistroConfirm));
        OnPropertyChanged(nameof(NombeUsuarioRecuperar));
        OnPropertyChanged(nameof(VerRegistroUsuario));
        OnPropertyChanged(nameof(VerRegistroEmpresa));
        OnPropertyChanged(nameof(NombreEmpresa));
        OnPropertyChanged(nameof(ContraEmpresa));
    }  
    
    private void ResetearFormularios()
    {
        Indice = 0;
        LimpiarCampos();
    }
}
