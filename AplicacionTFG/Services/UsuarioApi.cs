using System.Text;
using System.Text.Json;
using AplicacionTFG.Serialization;

namespace AplicacionTFG.Services;
public class UsuarioApi(string url) : WebApiBase(url)
{
    private readonly string urlbase = "api/Usuarios/";
    public async Task<string?> GetUsuarioAsync(int id)
    {
        var url = urlbase + id;
        return await GetAsync(url);
    }
    public async Task<string?> PostUsuarioAsync(UsuarioRegistroDto usuario)
    {
        var url = urlbase + "register";
        var content = new StringContent(JsonSerializer.Serialize<UsuarioRegistroDto>(usuario, UsuarioRegistroDtoContext.Default.UsuarioRegistroDto), Encoding.UTF8, "application/json");
        return await PostAsync(url, content);
    }
    public async Task<string?> PutUsuarioAsync(Usuario usuario)
    {
        var url = urlbase + usuario.Id;
        var content = new StringContent(JsonSerializer.Serialize<Usuario>(usuario, UsuarioContext.Default.Usuario), Encoding.UTF8, "application/json");
        return await PutAsync(url, content);
    }
    public async Task<string?> DeleteUsuarioAsync(int id)
    {
        var url = urlbase + id;
        return await DeleteAsync(url);
    }
    public async Task<string?> Login(UsuarioDto usuario)
    {
        var url = urlbase + "login";
        return await PostAsync(url, new StringContent(JsonSerializer.Serialize<UsuarioDto>(usuario, UsuarioDtoContext.Default.UsuarioDto), Encoding.UTF8, "application/json"));
    }
    public async Task<string?> Preguntar(string nombre)
    {
        var url = urlbase + "pregunta/" + nombre;
        return await GetAsync(url);
    }
    public async Task<string?> Responder(UsuarioRespuestaDto usuario)
    {
        var url = urlbase + "responder";
        return await PostAsync(url, new StringContent(JsonSerializer.Serialize<UsuarioRespuestaDto>(usuario, UsuarioRespuestaDtoContext.Default.UsuarioRespuestaDto), Encoding.UTF8, "application/json"));
    }
}
