using System.Text;
using System.Text.Json;
using AplicacionTFG.Serialization;

namespace AplicacionTFG.Services;
public class EmpresaApi(string url) : WebApiBase(url)
{
    private string urlbase = "api/Empresas/";

    #region Gets
    public async Task<string?> GetEmpresaAsync(int id)
    {
        var url = urlbase + id;
        return await GetAsync(url);
    }
    #endregion

    #region Posts
    public async Task<string?> PostEmpresaAsync(EmpresaDto empresa)
    {
        var url = urlbase + "register";
        var content = new StringContent(JsonSerializer.Serialize<EmpresaDto>(empresa, EmpresaDtoContext.Default.EmpresaDto), Encoding.UTF8, "application/json");
        return await PostAsync(url, content);
    }
    public async Task<string?> Login(EmpresaDto empresa)
    {
        var url = urlbase + "login";
        return await PostAsync(url, new StringContent(JsonSerializer.Serialize<EmpresaDto>(empresa, EmpresaDtoContext.Default.EmpresaDto), Encoding.UTF8, "application/json"));
    }
    #endregion
}
