using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AplicacionTFG.Models;
public class Empresa
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("nombre")]
    public required string Nombre { get; set; }
    [JsonPropertyName("password")]
    public required string Contrasena { get; set; }
}
public class EmpresaDto
{
    [JsonPropertyName("nombre")]
    [Required(ErrorMessage = "NombreObligatorio")]
    public required string Nombre { get; set; }
    [Required(ErrorMessage = "ContraseñaObligatoria")]
    [JsonPropertyName("password")]
    public required string Contrasena { get; set; }
}
