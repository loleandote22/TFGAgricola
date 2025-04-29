using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AplicacionTFG.Models;
public class Empresa
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; }
    [JsonPropertyName("password")]
    public string Contrasena { get; set; }
}
public class EmpresaDto
{
    [JsonPropertyName("nombre")]
    [Required(ErrorMessage = "El nombre es obligatoria")]
    public required string Nombre { get; set; }
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [JsonPropertyName("password")]
    public required string Contrasena { get; set; }
}
