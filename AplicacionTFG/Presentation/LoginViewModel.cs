using System.Text.Json;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;
using Windows.UI.ViewManagement;
namespace AplicacionTFG.Presentation;
public partial class LoginViewModel: ViewModelBase
{
    #region Auxiliares
    private int indice;
    public int Indice { get => indice; set 
        {
            
            indice = value; 
            VerAcceso = indice == 0 ? Visibility.Visible : Visibility.Collapsed;
            VerRegistro = indice == 1 ? Visibility.Visible : Visibility.Collapsed;
            VerRecuperacion = indice == 2 ? Visibility.Visible : Visibility.Collapsed;
            LimpiarCampos(); 
            OnPropertyChanged(nameof(Indice)); } }
    #endregion
    #region Localización
    private string usuario_Loc = "";
    private string contraseña_Loc = "";
    private string iniciar_Loc = "";
    private string confirmarContra_Loc = "";
    private string tipo_Loc = "";
    private string pregunta_Loc = "";
    private string respuesta_Loc = "";
    private string nombreEmpresa_Loc = "";
    private string contraseñaEmpresa_Loc = "";
    private string registrar_Loc = "";
    private string restablecer_Loc = "";
    private string nuevaContra_Loc = "";
    private string recuperar_Loc = "";
    private string cambiarContra_Loc = "";
    private string cambiarUsuario_Loc = "";
    public required string Usuario_Loc { get => usuario_Loc; set { usuario_Loc = value; OnPropertyChanged(nameof(Usuario_Loc)); } }
    public required string Contraseña_Loc { get => contraseña_Loc; set { contraseña_Loc = value; OnPropertyChanged(nameof(Contraseña_Loc)); } }
    public required string Iniciar_Loc { get => iniciar_Loc; set { iniciar_Loc = value; OnPropertyChanged(nameof(Iniciar_Loc)); } }
    public required string ConfirmarContra_Loc { get => confirmarContra_Loc; set { confirmarContra_Loc = value; OnPropertyChanged(nameof(ConfirmarContra_Loc)); } }
    public required string Tipo_Loc { get => tipo_Loc; set { tipo_Loc = value; OnPropertyChanged(nameof(Tipo_Loc)); } }
    public required string Pregunta_Loc { get => pregunta_Loc; set { pregunta_Loc = value; OnPropertyChanged(nameof(Pregunta_Loc)); } }
    public required string Respuesta_Loc { get => respuesta_Loc; set { respuesta_Loc = value; OnPropertyChanged(nameof(Respuesta_Loc)); } }
    public required string NombreEmpresa_Loc { get => nombreEmpresa_Loc; set { nombreEmpresa_Loc = value; OnPropertyChanged(nameof(NombreEmpresa_Loc)); } }
    public required string ContraseñaEmpresa_Loc { get => contraseñaEmpresa_Loc; set { contraseñaEmpresa_Loc = value; OnPropertyChanged(nameof(ContraseñaEmpresa_Loc)); } }
    public required string Registrar_Loc { get => registrar_Loc; set { registrar_Loc = value; OnPropertyChanged(nameof(Registrar_Loc)); } }
    public required string Restablecer_Loc { get => restablecer_Loc; set { restablecer_Loc = value; OnPropertyChanged(nameof(Restablecer_Loc)); } }
    public required string NuevaContra_Loc { get => nuevaContra_Loc; set { nuevaContra_Loc = value; OnPropertyChanged(nameof(NuevaContra_Loc)); } }
    public required string Recuperar_Loc { get => recuperar_Loc; set { recuperar_Loc = value; OnPropertyChanged(nameof(Recuperar_Loc)); } }
    public required string CambiarContra_Loc { get => cambiarContra_Loc; set { cambiarContra_Loc = value; OnPropertyChanged(nameof(CambiarContra_Loc)); } }
    public required string CambiarUsuario_Loc { get => cambiarUsuario_Loc; set { cambiarUsuario_Loc = value; OnPropertyChanged(nameof(CambiarUsuario_Loc)); } }

    #endregion
    #region Commandos
    public ICommand CambiarAEmpresaCommand => new RelayCommand(ComprobarUsuario);
    public ICommand CambiarAUsuarioCommand => new RelayCommand(CambiarAUsuario);
    public ICommand PreguntarCommand => new RelayCommand(Preguntar);
    public ICommand ResponderCommand => new RelayCommand(Responder);
    public ICommand CambiarContraCommand => new RelayCommand(CambiarContra);
    public ICommand RegistrarCommand => new RelayCommand(Registrar);
    public ICommand LoginCommand => new RelayCommand(Login);
    #endregion
    #region Visibilidades
    public Visibility VerRegistroUsuario { get; set; } = Visibility.Visible;
    public Visibility VerRegistroEmpresa { get; set; } = Visibility.Collapsed;
    public Visibility VerRecuperacionUsuario { get; set; } = Visibility.Visible;
    public Visibility VerRecuperacionPregunta { get; set; } = Visibility.Collapsed;
    public Visibility VerRecuperacionContraseña { get; set; } = Visibility.Collapsed;
    public bool Funcional { get => funcional; set { funcional = value; OnPropertyChanged(nameof(Funcional)); } }

    private Visibility verAcceso= Visibility.Visible;
    private Visibility verRegistro = Visibility.Collapsed;
    private Visibility verRecuperacion = Visibility.Collapsed;
    public Visibility VerAcceso { get => verAcceso; set { verAcceso = value; OnPropertyChanged(nameof(VerAcceso)); }}
    public Visibility VerRegistro { get => verRegistro; set { verRegistro = value; OnPropertyChanged(nameof(VerRegistro)); }}
    public Visibility VerRecuperacion { get => verRecuperacion; set { verRecuperacion = value; OnPropertyChanged(nameof(VerRecuperacion)); }}
    #endregion
    #region Strings
    public string NombreUsuarioLogin { get; set; } = string.Empty;
    public string ContraLogin { get; set ; } = string.Empty;
    public string NombreUsuarioRegistro { get; set; } = string.Empty;
    public string ContraRegistro { get; set; } = string.Empty;
    public string ContraRegistroConfirm { get; set; } = string.Empty;
    public string PreguntaRegistro { get; set; } = string.Empty;
    public string RespuestaRegistro { get; set; } = string.Empty;
    public List<string> RolesRegistro { get; set; } = [ "Dueño", "Administrador", "Empleado" ]; 
    public string RolRegistro { get; set; } = string.Empty;
    public string NombreEmpresa { get; set; } = string.Empty;
    public string ContraEmpresa { get; set; } = string.Empty;
    public string NombeUsuarioRecuperar { get; set; } = string.Empty;
    public string PreguntaRecuperar { get; set; } = string.Empty;
    public string RespuestaRecuperar { get; set; } = string.Empty;
    public string ContraRecuperar { get; set; } = string.Empty;
    public string ContraRecuperarConfirm { get; set; } = string.Empty;
    #endregion


    private UsuarioRegistroDto? usuarioRegistro = null;
    private Usuario? usuario = null;
    private readonly UsuarioApi _usuarioApi;
    private readonly EmpresaApi _empresaApi;
    private bool funcional = true;

    public LoginViewModel(IStringLocalizer localizer,ILocalizationService localizationService, INavigator navigator, IOptions<AppConfig> appInfo) : base(localizer, navigator,appInfo, localizationService)
    {
        Indice =0;
        _usuarioApi = new(Apiurl);
        _empresaApi = new(Apiurl);
    }

#pragma warning disable CS8618 
    public LoginViewModel() { }
#pragma warning restore CS8618 

    private async void Preguntar()
    {
        Funcional = false;
        InputPane.GetForCurrentView().TryHide();
        try
        {
            var pregunta = await _usuarioApi.GetPregunta(NombeUsuarioRecuperar);
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
        InputPane.GetForCurrentView().TryHide();
        UsuarioRespuestaDto usuarioRespuestaDto = new()
        {
            Nombre = NombeUsuarioRecuperar,
            Respuesta = RespuestaRecuperar
        };
        var result = await _usuarioApi.PostRespuesta(usuarioRespuestaDto);
        if (result == null)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["RecuperarContraseña"], content: "La respuesta es incorrecta");
            Funcional = true;
            return;
        }
        usuario = JsonSerializer.Deserialize<Usuario>(result, UsuarioContext.Default.Usuario);
        VerRecuperacionPregunta = Visibility.Collapsed;
        VerRecuperacionContraseña = Visibility.Visible;
        OnPropertyChanged(nameof(VerRecuperacionPregunta));
        OnPropertyChanged(nameof(VerRecuperacionContraseña));
        Funcional = true;
    }

    private async void CambiarContra()
    {
        Funcional = false;
        InputPane.GetForCurrentView().TryHide();
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
        Usuario? usuarioDevuelto = JsonSerializer.Deserialize<Usuario>(result, UsuarioContext.Default.Usuario);
        if (usuarioDevuelto == null)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["RecuperarContraseña"], content: "Error al cambiar la contraseña");
            Funcional = true;
            return;
        }
        ResetearFormularios();
        localSettings.Values["Usuario"] = result;
        await _navigator.NavigateViewModelAsync<MainViewModel>(this, data: usuarioDevuelto);
        Funcional = true;
    }

    private async void ComprobarUsuario()
    {
        usuarioRegistro = new UsuarioRegistroDto()
        {
            Nombre = NombreUsuarioRegistro,
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

        var opciones = new JsonSerializerOptions
        {
          
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
        var resultado = Convert.ToBoolean(await _usuarioApi.GetUsuarioNombreAsync(NombreUsuarioRegistro));
        if (resultado )
        {
            await _navigator.ShowMessageDialogAsync(this, title: "Registrar usuario", content: "El nombre de usuario ya existe");
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
        InputPane.GetForCurrentView().TryHide();
        EmpresaDto empresa = new()
        {
            Nombre = NombreEmpresa,
            Contrasena = ContraEmpresa
        };
        if(!ValidarModelo(empresa) )
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
            Empresa? empresaDevuelta = JsonSerializer.Deserialize<Empresa>(result, EmpresaContext.Default.Empresa);
            usuarioRegistro.EmpresaId = empresaDevuelta!.Id;
            result = await _usuarioApi.PostUsuarioAsync(usuarioRegistro);
            if (result is not null)
            {
                await _navigator.ShowMessageDialogAsync(this, title: "Registrar usuario", content: "Usuario registrado correctamente");
                Usuario? usuarioDevuelto = JsonSerializer.Deserialize<Usuario>(result, UsuarioContext.Default.Usuario);
                localSettings.Values["Usuario"] = result;
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
        InputPane.GetForCurrentView().TryHide();

        if (string.IsNullOrEmpty(NombreUsuarioLogin) || string.IsNullOrEmpty(ContraLogin))
            await _navigator.ShowMessageDialogAsync(this, title: "Login", content: "Por favor, introduce un nombre de usuario y una contraseña.");
        else
        {
            UsuarioDto usuario = new()
            {
                Nombre = NombreUsuarioLogin,
                Contrasena = ContraLogin
            };
            try
            {
                var result = await _usuarioApi.PostLogin(usuario);
                if (result is not null)
                {
                    Usuario usuarioDevuelto = JsonSerializer.Deserialize(result, UsuarioContext.Default.Usuario)!;
                    ResetearFormularios();
                    localSettings.Values["Usuario"] = result;
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

    protected override void CargarPalabras()
    {
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
        NuevaContra_Loc = _localizer["Nueva"] + " " + Contraseña_Loc;
        CambiarContra_Loc = _localizer["Cambiar"] + " " + Contraseña_Loc;
        Recuperar_Loc = _localizer["RecuperarContraseña"];
        CambiarUsuario_Loc = _localizer["CambiarUsuario"];
    }

    private void LimpiarCampos()
    {
        NombreUsuarioLogin = string.Empty;
        ContraLogin = string.Empty;
        NombreUsuarioRegistro = string.Empty;
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
        OnPropertyChanged(nameof(NombreUsuarioLogin));
        OnPropertyChanged(nameof(ContraLogin));
        OnPropertyChanged(nameof(NombreUsuarioRegistro));
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
