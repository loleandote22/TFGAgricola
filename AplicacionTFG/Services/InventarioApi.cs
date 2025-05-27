using System.Text;
using System.Text.Json;

namespace AplicacionTFG.Services;

public class InventarioApi(string url) : WebApiBase(url)
{
    private readonly string urlbase = "api/Inventario/";
    public Task<string?> GetInventariosAsync(int id)
    {
        var url = urlbase + "empresa/" + id;
        return GetAsync(url);
    }

    public Task<string?> GetInventarioAsync(int id)
    {
        var url = urlbase + id;
        return GetAsync(url);
    }

    public Task<string?> PostInventarioAsync(Inventario inventario)
    {;
        var content = new StringContent(JsonSerializer.Serialize<Inventario>(inventario), Encoding.UTF8, "application/json");
        return PostAsync(urlbase, content);
    }

    public Task<string?> PutInventarioAsync(InventarioActualizaDto inventario)
    {
        var url = urlbase + inventario.Id;
        var content = new StringContent(JsonSerializer.Serialize<InventarioActualizaDto>(inventario), Encoding.UTF8, "application/json");
        return PutAsync(url, content);
    }

    public Task<string?> DeleteInventarioAsync(int id)
    {
        var url = urlbase + id;
        return DeleteAsync(url);
    }

    public Task<string?> PostComentarioInventario(InventarioChat inventarioChat)
    {
        var url = urlbase + "comentario";
        var content = new StringContent(JsonSerializer.Serialize<InventarioChat>(inventarioChat), Encoding.UTF8, "application/json");
        return PostAsync(url, content);
    }
}
