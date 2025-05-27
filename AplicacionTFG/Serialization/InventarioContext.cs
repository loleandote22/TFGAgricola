using System.Text.Json.Serialization;

namespace AplicacionTFG.Serialization;

public class InventarioContext
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
