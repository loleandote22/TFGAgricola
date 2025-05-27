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
[JsonSerializable(typeof(UsuarioDto))]
[JsonSerializable(typeof(UsuarioDto[]))]
[JsonSerializable(typeof(IEnumerable<UsuarioDto>))]
[JsonSerializable(typeof(IImmutableList<UsuarioDto>))]
[JsonSerializable(typeof(ImmutableList<UsuarioDto>))]
[JsonSerializable(typeof(List<UsuarioDto>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class UsuarioDtoContext : JsonSerializerContext
{
}

[JsonSerializable(typeof(UsuarioRegistroDto))]
[JsonSerializable(typeof(UsuarioRegistroDto[]))]
[JsonSerializable(typeof(IEnumerable<UsuarioRegistroDto>))]
[JsonSerializable(typeof(IImmutableList<UsuarioRegistroDto>))]
[JsonSerializable(typeof(ImmutableList<UsuarioRegistroDto>))]
[JsonSerializable(typeof(List<UsuarioRegistroDto>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class UsuarioRegistroDtoContext : JsonSerializerContext
{
    
}

[JsonSerializable(typeof(UsuarioRespuestaDto))]
[JsonSerializable(typeof(UsuarioRespuestaDto[]))]
[JsonSerializable(typeof(IEnumerable<UsuarioRespuestaDto>))]
[JsonSerializable(typeof(IImmutableList<UsuarioRespuestaDto>))]
[JsonSerializable(typeof(ImmutableList<UsuarioRespuestaDto>))]
[JsonSerializable(typeof(List<UsuarioRespuestaDto>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class UsuarioRespuestaDtoContext : JsonSerializerContext
{
}
