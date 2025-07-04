using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;

namespace AplicacionTFG.Presentation.Personal;
public class PersonalViewModel: ViewModelBase
{

    private string directorioImagenes = "/Assets/UserIcons/";

    #region Localización
    public required string Titulo_Loc { get; set; }
    #endregion

    public List<UsuarioEmpresa> Usuarios { get; set; } = null!;
    public UsuarioEmpresa? UsuarioSeleccionado { get => usuarioSeleccionado; set { VerUsuario(value!); usuarioSeleccionado = value; }}
    private readonly UsuarioApi _usuarioApi;
    private UsuarioEmpresa? usuarioSeleccionado;

    public PersonalViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo) : base(localizer, navigator, appInfo)
    {
        _usuarioApi = new UsuarioApi(Apiurl);
        CargarUsuarios();
    }

#if __WASM__
    private async void CargarUsuarios()
    {
        try
        {
            var result =await _usuarioApi.GetUsuariosEmpresa(Usuario.EmpresaId);
            TerminarCarga(result);
        }
        catch (HttpRequestException)
        {
            await _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#else
    private void CargarUsuarios()
    {
        try
        {
            var result = _usuarioApi.GetUsuariosEmpresa(Usuario.EmpresaId).GetAwaiter().GetResult();
            TerminarCarga(result);
        }
        catch (HttpRequestException)
        {
            _navigator.ShowMessageDialogAsync(this, title: _localizer["Error"], content: _localizer["ErrorConexion"]);
        }
    }
#endif
    private void TerminarCarga(string? resultado)
    {
        List<string> Tipos = [_localizer["Dueno"], _localizer["Administrador"], _localizer["Empleado"]];
        if (string.IsNullOrEmpty(resultado))
            return;
        var deserializedResult = JsonSerializer.Deserialize(resultado, UsuarioEmpresaContext.Default.ListUsuarioEmpresa);
        
        Usuarios = deserializedResult!.Select(usuario => { usuario.Imagen = directorioImagenes + usuario.Imagen; usuario.TipoNombre = Tipos[usuario.Tipo];  return usuario; }).ToList();
        OnPropertyChanged(nameof(Usuarios));
    }

    public void VerUsuario(UsuarioEmpresa usuario)
    {
       _navigator.NavigateViewModelAsync<PersonaViewModel>(this, qualifier: Qualifiers.ClearBackStack,new EntityNumber(usuario.Id));
    }
    protected override void CargarPalabras()
    {
        Titulo_Loc = _localizer["Personal"];
    }
}
