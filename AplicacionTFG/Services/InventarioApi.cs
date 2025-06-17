using System.Text;
using System.Text.Json;
using AplicacionTFG.Serialization;

namespace AplicacionTFG.Services;

public class InventarioApi(string url) : WebApiBase(url)
{
    private readonly string urlbase = "api/Inventario/";

    #region Gets
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

    public Task<string?> GetInventarioEventosAsync(int id, int pagina)
    {
        var url = urlbase + "eventos/" + id + "/pagina/" + pagina;
        return GetAsync(url);
    }

    public Task<string?> GetInventarioChatsAsync(int id, int pagina)
    {
        var url = urlbase + "mensajes/" + id+ "/pagina/" + pagina;
        return GetAsync(url);
    }
    #endregion

    #region Posts

    public Task<string?> PostInventarioAsync(InventarioDto inventario)
    {;
        var content = new StringContent(JsonSerializer.Serialize(inventario, InventarioDtoContext.Default.InventarioDto), Encoding.UTF8, "application/json");
        return PostAsync(urlbase, content);
    }
    public Task<string?> PostComentarioInventario(InventarioChat inventarioChat)
    {
        var url = urlbase + "comentario";
        var content = new StringContent(JsonSerializer.Serialize(inventarioChat, InventarioChatContext.Default.InventarioChat), Encoding.UTF8, "application/json");
        return PostAsync(url, content);
    }
    #endregion

    #region Puts
    public Task<string?> PutInventarioAsync(InventarioActualizaDto inventario)
    {
        var url = urlbase + inventario.Id;
        var content = new StringContent(JsonSerializer.Serialize(inventario, InventarioActualizaContext.Default.InventarioActualizaDto), Encoding.UTF8, "application/json");
        return PutAsync(url, content);
    }
    #endregion

    #region Deletes
    public Task<string?> DeleteInventarioAsync(int id)
    {
        var url = urlbase + id;
        return DeleteAsync(url);
    }
    #endregion
}
