using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AplicacionTFG.Presentation.Eventos;
using AplicacionTFG.Presentation.Perfil;
using AplicacionTFG.Serialization;
using AplicacionTFG.Services;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Documents;

namespace AplicacionTFG.Presentation.Personal;
public class PersonaViewModel : ViewModelBase
{
    #region Localización
    public required string Perfil_Loc { get; set; }
    public required string Eventos_Loc { get; set; }
    #endregion

    #region Contextos
    public PerfilViewModel PerfilViewModel { get; set; }
    public EventosMesViewModel EventosViewModel { get; set; }
    #endregion

    #region Visibilidades
    private int indice = 0;
    public int Indice
    {
        get => indice; set
        {

            indice = value;
            VerPerfil = indice == 0 ? Visibility.Visible : Visibility.Collapsed;
            VerEventos = indice == 1 ? Visibility.Visible : Visibility.Collapsed;
            OnPropertyChanged(nameof(Indice));
        }
    }
    private Visibility vererfil;
    public Visibility VerPerfil { get => vererfil; set { vererfil = value; OnPropertyChanged(nameof(VerPerfil)); } }
    private Visibility verEventos;
    public Visibility VerEventos { get => verEventos; set { verEventos = value; OnPropertyChanged(nameof(VerEventos)); }}
    #endregion


    private readonly UsuarioApi _usuarioApi;
    private Usuario usuario = null!;

    public Usuario UsuarioBuscado
    {
        get => usuario;
        set
        {
            usuario = value;
            OnPropertyChanged(nameof(UsuarioBuscado));
        }
    }
    public PersonaViewModel(IMessenger messenger, INavigator navigator, IStringLocalizer localizer,  IOptions<AppConfig> appInfo, EntityNumber elemento)
        : base(localizer, navigator, appInfo)
    {
        _usuarioApi = new UsuarioApi(Apiurl);
        Indice = 0;
        CargarUsuario(elemento.number);
        PerfilViewModel = new PerfilViewModel(messenger, localizer,_localizationService, navigator, appInfo) { Usuario = UsuarioBuscado};
        EventosViewModel = new EventosMesViewModel(navigator, localizer, appInfo, UsuarioBuscado.Id);   
        PerfilViewModel.CargarCampos();
    }

    private void CargarUsuario(int id)
    {
        var result = _usuarioApi.GetUsuarioAsync(id).Result;
       if (result is not null)
                UsuarioBuscado = JsonSerializer.Deserialize(result, UsuarioContext.Default.Usuario)!;
        else
            _navigator.ShowMessageDialogAsync(this, title:"Error", content:_localizer["ErrorCargarUsuario"]);
    }

    protected override void CargarPalabras()
    {
        Perfil_Loc = _localizer["Perfil"];
        Eventos_Loc = _localizer["Eventos"];

    }
}
