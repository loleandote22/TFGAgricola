using System.Text.Json.Serialization;

namespace AplicacionTFG.Serialization;
[JsonSerializable(typeof(Empresa))]
[JsonSerializable(typeof(Empresa[]))]
[JsonSerializable(typeof(IEnumerable<Empresa>))]
[JsonSerializable(typeof(IImmutableList<Empresa>))]
[JsonSerializable(typeof(ImmutableList<Empresa>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class EmpresaContext : JsonSerializerContext
{
}

[JsonSerializable(typeof(EmpresaDto))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class EmpresaDtoContext : JsonSerializerContext
{
}
