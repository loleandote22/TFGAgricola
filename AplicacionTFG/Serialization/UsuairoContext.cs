using System.Text.Json.Serialization;

namespace AplicacionTFG.Serialization;
[JsonSerializable(typeof(Usuario))]
[JsonSerializable(typeof(Usuario[]))]
[JsonSerializable(typeof(IEnumerable<Usuario>))]
[JsonSerializable(typeof(IImmutableList<Usuario>))]
[JsonSerializable(typeof(ImmutableList<Usuario>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class UsuarioContext : JsonSerializerContext
{
}

