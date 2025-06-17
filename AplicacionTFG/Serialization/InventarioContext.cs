using System.Text.Json.Serialization;

namespace AplicacionTFG.Serialization;
[JsonSerializable(typeof(Inventario))]
[JsonSerializable(typeof(Inventario[]))]
[JsonSerializable(typeof(IEnumerable<Inventario>))]
[JsonSerializable(typeof(IImmutableList<Inventario>))]
[JsonSerializable(typeof(ImmutableList<Inventario>))]
[JsonSerializable(typeof(List<Inventario>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class InventarioContext : JsonSerializerContext
{
}
[JsonSerializable(typeof(InventarioConsulta))]
[JsonSerializable(typeof(InventarioConsulta[]))]
[JsonSerializable(typeof(IEnumerable<InventarioConsulta>))]
[JsonSerializable(typeof(IImmutableList<InventarioConsulta>))]
[JsonSerializable(typeof(ImmutableList<InventarioConsulta>))]
[JsonSerializable(typeof(List<InventarioConsulta>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class InventarioConsultaContext : JsonSerializerContext
{
}

[JsonSerializable(typeof(InventarioDto))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class InventarioDtoContext : JsonSerializerContext
{
}

[JsonSerializable(typeof(InventarioActualizaDto))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class InventarioActualizaContext : JsonSerializerContext
{
}
[JsonSerializable(typeof(InventarioChat))]
[JsonSerializable(typeof(InventarioChat[]))]
[JsonSerializable(typeof(IEnumerable<InventarioChat>))]
[JsonSerializable(typeof(IImmutableList<InventarioChat>))]
[JsonSerializable(typeof(ImmutableList<InventarioChat>))]
[JsonSerializable(typeof(List<InventarioChat>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class InventarioChatContext : JsonSerializerContext
{
}
[JsonSerializable(typeof(List<InventarioChatConsulta>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class InventarioChatConsultaContext : JsonSerializerContext
{
}
[JsonSerializable(typeof(List<InventarioEventoConsulta>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class InventarioEventoConsultaContext : JsonSerializerContext
{
}

