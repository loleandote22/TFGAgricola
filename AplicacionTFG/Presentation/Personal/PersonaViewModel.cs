using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AplicacionTFG.Presentation.Perfil;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;

namespace AplicacionTFG.Presentation.Personal;
public class PersonaViewModel : ViewModelBase
{
    #region Localización
    public required string Perfil_Loc { get; set; }
    public required string Tareas_Loc { get; set; }

    public PerfilViewModel PerfilViewModel { get; set; }
    #endregion
    private readonly UsuarioApi _usuarioApi;
    private Usuario usuario;
    public Usuario Usuario
    {
        get => usuario;
        set
        {
            usuario = value;
            OnPropertyChanged(nameof(Usuario));
        }
    }
    public PersonaViewModel(INavigator navigator, IStringLocalizer localizer,  IOptions<AppConfig> appInfo, EntityNumber elemento)
        : base(localizer, navigator, appInfo)
    {
        _usuarioApi = new UsuarioApi(Apiurl);
        
        CargarUsuario(elemento.number);
        PerfilViewModel = new PerfilViewModel(localizer,_localizationService, navigator, appInfo) { Usuario = Usuario};
        PerfilViewModel.CargarCampos();
    }

    private void CargarUsuario(int id)
    {
        var result = _usuarioApi.GetUsuarioAsync(id).Result;
       if (result is not null)
                Usuario = JsonSerializer.Deserialize(result, UsuarioContext.Default.Usuario)!;
        else
            _navigator.ShowMessageDialogAsync(this, title:"Error", content:_localizer["ErrorCargarUsuario"]);
    }

    protected override void CargarPalabras()
    {
        Perfil_Loc = _localizer["Perfil"];
        Tareas_Loc = _localizer["Tareas"];

    }
}
