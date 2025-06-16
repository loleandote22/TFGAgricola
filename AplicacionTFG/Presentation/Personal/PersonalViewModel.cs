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

    public List<UsuarioEmpresa> Usuarios { get; set; }
    public UsuarioEmpresa? UsuarioSeleccionado { get => usuarioSeleccionado; set { VerUsuario(value); usuarioSeleccionado = value; }}
    private readonly UsuarioApi _usuarioApi;
    private UsuarioEmpresa? usuarioSeleccionado;

    public PersonalViewModel(INavigator navigator, IStringLocalizer localizer, IOptions<AppConfig> appInfo) : base(localizer, navigator, appInfo)
    {
        _usuarioApi = new UsuarioApi(Apiurl);
        var result = _usuarioApi.GetUsuariosEmpresa(Usuario.EmpresaId.GetValueOrDefault()).GetAwaiter().GetResult();
        CargarUsuarios();
    }

#if __WASM__
    private async void CargarUsuarios()
    {
        var result =await _usuarioApi.GetUsuariosEmpresa(Usuario.EmpresaId.GetValueOrDefault());
        TerminarCarga(result);
    }
#else
    private void CargarUsuarios()
    {
        var result = _usuarioApi.GetUsuariosEmpresa(Usuario.EmpresaId.GetValueOrDefault()).GetAwaiter().GetResult();
        TerminarCarga(result);
    }
#endif
    private void TerminarCarga(string? resultado)
    {
        if (string.IsNullOrEmpty(resultado))
            return;
        var deserializedResult = JsonSerializer.Deserialize(resultado, UsuarioEmpresaContext.Default.ListUsuarioEmpresa);
        
        Usuarios = deserializedResult!.Select(usuario => { usuario.Imagen = directorioImagenes + usuario.Imagen; return usuario; }).ToList();
        OnPropertyChanged(nameof(Usuarios));
    }

    public void VerUsuario(UsuarioEmpresa usuario)
    {
        Console.WriteLine(usuario.id);
       // _navigator.NavigateViewModelAsync
    }
    protected override void CargarPalabras()
    {
        Titulo_Loc = _localizer["Personal"];
    }
}
