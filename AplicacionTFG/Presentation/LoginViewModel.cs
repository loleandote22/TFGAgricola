using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using AplicacionTFG.Services;
using Uno.Extensions;
using Windows.UI.Popups;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AplicacionTFG.Presentation;
public class LoginViewModel: ObservableObject//ViewModelBase
{
    private INavigator _navigator;

    public ICommand RecuperarCommand { get; }
    public ICommand RegistrarCommand { get; }
    public ICommand LoginCommand { get; }
    public string UserNameLogin { get => userNameLogin; set { userNameLogin = value;
            } }
    public string PasswordLogin { get => passwordLogin; set { passwordLogin = value;
            } }
    public string UserNameRegister { get; set; }
    public string PasswordRegister { get; set; }
    public string PasswordRegisterConfirm { get; set; }
    public string UserNameRecuperar { get; set; }
    public Usuario? Usuario { get; set; }
    public int Indice { get => indice; set { indice = value; LimpiarCampos(); OnPropertyChanged("Indice"); } }

    private UsuarioApi _usuarioApi = new UsuarioApi();
    private string userNameLogin;
    private string passwordLogin;
    private int indice;

    public LoginViewModel(INavigator navigator)
    {
        _navigator = navigator;
        userNameLogin = string.Empty;
        passwordLogin = string.Empty;
        UserNameRegister = string.Empty;
        PasswordRegister = string.Empty;
        PasswordRegisterConfirm = string.Empty;
        UserNameRecuperar = string.Empty;
        RecuperarCommand = new RelayCommand(Recuperar);
        RegistrarCommand = new RelayCommand(Registrar);
        LoginCommand = new RelayCommand(Login);
    }

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public LoginViewModel() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

    private void Recuperar()
    {
        _navigator.ShowMessageDialogAsync(this,title:"Recuperar contraseña", content:"Funcionalidad no implementada");
    }
    private void Registrar()
    {

        _navigator.ShowMessageDialogAsync(this, title: "Registrar usuario", content: "Funcionalidad no implementada");
    }
    private async void Login()
    {
        //_navigator.ShowMessageDialogAsync(this, title: "Login", content: "Llegué hasta aquí.");
        if (string.IsNullOrEmpty(UserNameLogin) || string.IsNullOrEmpty(PasswordLogin))
        {
            _navigator.ShowMessageDialogAsync(this, title: "Login", content: "Por favor, introduce un nombre de usuario y una contraseña.");
            return;
        }
        else
        {
            UsuarioDto usuario = new UsuarioDto()
            {
                Nombre = UserNameLogin,
                Contrasena = PasswordLogin
            };
            using StringContent content = new("{\"nombre\":\"" + UserNameLogin + "\",\"password\":\"" + PasswordLogin + "\"}", Encoding.UTF8, "application/json");
            var resultado = await _usuarioApi.GetUsuarioAsync(2);
            var result = await _usuarioApi.Login(content);
            if (result != null)
            {
                await _navigator.NavigateViewModelAsync<MainViewModel>(this);
                //_navigator.ShowMessageDialogAsync(this, title: "Login", content: "Login correcto");
                //return;
            }
            else
            {
                _navigator.ShowMessageDialogAsync(this, title: "Login", content: "Error en el login");
                return;
            }
        }
        //    _navigator.ShowMessageDialogAsync(this, title: "Login", content: "Funcionalidad no implementada");
    }
    private void LimpiarCampos()
    {

        UserNameLogin = string.Empty;
        PasswordLogin = string.Empty;
        UserNameRegister = string.Empty;
        PasswordRegister = string.Empty;
        PasswordRegisterConfirm = string.Empty;
        UserNameRecuperar = string.Empty;
        OnPropertyChanged(nameof(userNameLogin));
        OnPropertyChanged(nameof(passwordLogin));
        OnPropertyChanged(nameof(UserNameRegister));
        OnPropertyChanged(nameof(PasswordRegister));
        OnPropertyChanged(nameof(PasswordRegisterConfirm));
        OnPropertyChanged(nameof(UserNameRecuperar));
    }

}
