using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AplicacionTFG.Services;
public class UsuarioApi : WebApiBase
{
    private string urlbase = "api/Usuarios/";
    public async Task<string> GetUsuarioAsync(int id)
    {
        var url = urlbase + id;
        return await GetAsync(url);
    }
    public async Task<string> PostUsuarioAsync(string jsonContent)
    {
        var url = urlbase;
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        return await PostAsync(url, content);
    }
    public async Task<string> PutUsuarioAsync(int id, string jsonContent)
    {
        var url = urlbase + id;
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        return await PutAsync(url, content);
    }
    public async Task<string> DeleteUsuarioAsync(int id)
    {
        var url = urlbase + id;
        return await DeleteAsync(url);
    }
    public async Task<string> Login(StringContent jsonContent)
    {
        var url = urlbase+"login";
        return await PostAsync(url, jsonContent);
        
    }
}
