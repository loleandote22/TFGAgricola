using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using AplicacionTFG.Serialization;

namespace AplicacionTFG.Services;

public class EventoApi(string url) : WebApiBase(url)
{
    private readonly string urlbase = "api/Eventos/";
    
    #region Gets
    public async Task<string?> GetEventosDiaEmpresaAsync(int idEmpresa, int dia, int mes, int anno)
    {
        var url = urlbase + $"empresa/{idEmpresa}/dia/{dia}/mes/{mes}/anno/{anno}";
        return await GetAsync(url);
    }
    public async Task<string?> GetEventosDiaUsuarioAsync(int idUsuario, int dia, int mes, int anno)
    {
        var url = urlbase + $"usuario/{idUsuario}/dia/{dia}/mes/{mes}/anno/{anno}";
        return await GetAsync(url);
    }
    public async Task<string?> GetEventosMesEmpresaAsync(int idEmpresa, int mes, int anno)
    {
        var url = urlbase + $"empresa/{idEmpresa}/mes/{mes}/anno/{anno}";
        return await GetAsync(url);
    }
    public async Task<string?> GetEventosMesUsuarioAsync(int idUsuario, int mes, int anno)
    {
        var url = urlbase + $"usuario/{idUsuario}/mes/{mes}/anno/{anno}";
        return await GetAsync(url);
    }

    public async Task<string?> GetEvento(int id)
    {
        var url = urlbase + id;
        return await GetAsync(url);
    }
    #endregion

    #region Posts
    public async Task<string?> PostEventoAsync(EventoDto evento)
    {
        var content = new StringContent(JsonSerializer.Serialize(evento, EventoDtoContext.Default.EventoDto), Encoding.UTF8, "application/json");
        return await PostAsync(urlbase, content);
    }
    #endregion

    #region Puts
    public async Task<string?> PutEventoAsync(Evento evento)
    {
        var url = urlbase + evento.Id;
        var content = new StringContent(JsonSerializer.Serialize(evento, EventoContext.Default.Evento), Encoding.UTF8, "application/json");
        return await PutAsync(url, content);
    }
    #endregion
    #region Deletes
    public async Task<string?> DeleteEventoAsync(int id)
    {
        var url = urlbase + id;
        return await DeleteAsync(url);
    }
    #endregion
}
