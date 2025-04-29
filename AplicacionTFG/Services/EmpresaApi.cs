using System.Text;
using System.Text.Json;

namespace AplicacionTFG.Services;
public class EmpresaApi: WebApiBase
{
    private string urlbase = "api/Empresas/";
    public async Task<string> GetEmpresaAsync(int id)
    {
        var url = urlbase + id;
        return await GetAsync(url);
    }
    public async Task<string> PostEmpresaAsync(EmpresaDto empresa)
    {
        var url = urlbase + "register";
        var content = new StringContent(JsonSerializer.Serialize<EmpresaDto>(empresa), Encoding.UTF8, "application/json");
        return await PostAsync(url, content);
    }
    public async Task<string> Login(EmpresaDto empresa)
    {
        var url = urlbase + "login";
        return await PostAsync(url, new StringContent(JsonSerializer.Serialize<EmpresaDto>(empresa), Encoding.UTF8, "application/json"));
    }
}

