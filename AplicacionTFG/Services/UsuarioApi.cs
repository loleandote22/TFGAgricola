using System.Text;
using System.Text.Json;
using AplicacionTFG.Serialization;

namespace AplicacionTFG.Services;
public class UsuarioApi(string url) : WebApiBase(url)
{
    private readonly string urlbase = "api/Usuarios/";

    #region Gets
    public async Task<string?> GetUsuarioAsync(int id)
    {
        var url = urlbase + id;
        return await GetAsync(url);
    }

    public async Task<string?> GetUsuarioNombreAsync(string nombre)
    {
        var url = urlbase + "nombre/"+nombre;
        return await GetAsync(url);
    }

    public async Task<string?> GetPregunta(string nombre)
    {
        var url = urlbase + "pregunta/" + nombre;
        return await GetAsync(url);
    }

    public async Task<string?> GetUsuariosEmpresa(int idEmpresa)
    {
        var url = urlbase + "empresa/" + idEmpresa;
        return await GetAsync(url);
    }

    public async Task<string?> GetNombreUsuariosEmpresa(int idEmpresa)
    {

        var url = urlbase + "corto/empresa/" + idEmpresa;
        return await GetAsync(url);
    }

    public async Task<string?> GetExisteNombreUsuarioAsync(string nombre)
    {
        var url = urlbase + "nombre/"+nombre;
        return await GetAsync(url);
    }
    #endregion

    #region Posts
    public async Task<string?> PostUsuarioAsync(UsuarioRegistroDto usuario)
    {
        var url = urlbase + "register";
        var content = new StringContent(JsonSerializer.Serialize<UsuarioRegistroDto>(usuario, UsuarioRegistroDtoContext.Default.UsuarioRegistroDto), Encoding.UTF8, "application/json");
        return await PostAsync(url, content);
    }
    public async Task<string?> PostLogin(UsuarioDto usuario)
    {
        var url = urlbase + "login";
        return await PostAsync(url, new StringContent(JsonSerializer.Serialize<UsuarioDto>(usuario, UsuarioDtoContext.Default.UsuarioDto), Encoding.UTF8, "application/json"));
    }
    public async Task<string?> PostRespuesta(UsuarioRespuestaDto usuario)
    {
        var url = urlbase + "responder";
        return await PostAsync(url, new StringContent(JsonSerializer.Serialize<UsuarioRespuestaDto>(usuario, UsuarioRespuestaDtoContext.Default.UsuarioRespuestaDto), Encoding.UTF8, "application/json"));
    }
    public async Task<string?> PutUsuarioAsync(int id, UsuarioAcutliazarDto usuario)
    {
        var url = urlbase + id;
        var content = new StringContent(JsonSerializer.Serialize<UsuarioAcutliazarDto>(usuario, UsuarioActualizarDtoContext.Default.UsuarioAcutliazarDto), Encoding.UTF8, "application/json");
        return await PutAsync(url, content);
    }
    #endregion

    #region Puts
    public async Task<string?> PutUsuarioAsync(Usuario usuario)
    {
        var url = urlbase + usuario.Id;
        var content = new StringContent(JsonSerializer.Serialize<Usuario>(usuario, UsuarioContext.Default.Usuario), Encoding.UTF8, "application/json");
        return await PutAsync(url, content);
    }
    #endregion

    #region Deletes
    public async Task<string?> DeleteUsuarioAsync(int id)
    {
        var url = urlbase + id;
        return await DeleteAsync(url);
    }
    #endregion
}
