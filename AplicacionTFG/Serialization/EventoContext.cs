using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AplicacionTFG.Serialization;

    [JsonSerializable(typeof(Evento))]
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    public partial class EventoContext : JsonSerializerContext
    {
    }

    [JsonSerializable(typeof(TareaDetalle))]
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    public partial class TareaDetalleContext : JsonSerializerContext
    {
    }
    [JsonSerializable(typeof(EventoDto))]
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    public partial class EventoDtoContext : JsonSerializerContext
    {
    }
    [JsonSerializable(typeof(EventoMes))]
    [JsonSerializable(typeof(EventoMes[]))]
    [JsonSerializable(typeof(IEnumerable<EventoMes>))]
    [JsonSerializable(typeof(List<EventoMes>))]
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    public partial class EventoMesContext : JsonSerializerContext
    {
    }
    [JsonSerializable(typeof(EventoDia))]
    [JsonSerializable(typeof(List<EventoDia>))]
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    public partial class EventoDiaContext : JsonSerializerContext
    {
    }

    [JsonSerializable(typeof(TareaActualizacionDto))]
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    public partial class TareaActualizacionDtoContext : JsonSerializerContext
    {
    }
    [JsonSerializable(typeof(TareaActualizacion))]
    [JsonSerializable(typeof(TareaActualizacion[]))]
    [JsonSerializable(typeof(IEnumerable<TareaActualizacion>))]
    [JsonSerializable(typeof(List<TareaActualizacion>))]
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    public partial class TareaActualizacionContext : JsonSerializerContext
    {
    }
