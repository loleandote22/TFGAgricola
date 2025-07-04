using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AplicacionTFG.Models;

public class Usuario
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("nombre")]
    [StringLength(50, ErrorMessage = "LongitudNombre")]
    public required string Nombre { get; set; }
    [StringLength(16,MinimumLength =8, ErrorMessage = "LongitudContraseña")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).+$", ErrorMessage = "ContraseñaLetraNumero")]
    [JsonPropertyName("password")]
    public required string Contrasena { get; set; }
    [JsonPropertyName("rol")]
    public required int Tipo { get; set; } // Dueño, Administrador, Empleado
    [JsonPropertyName("pregunta")]
    public required string Pregunta { get; set; }
    [JsonPropertyName("respuesta")]
    public required string Respuesta { get; set; }
    [JsonPropertyName("empresaId")]
    public int EmpresaId { get; set; }
    [JsonPropertyName("imagen")]
    public string? Imagen { get; set; }
}

public class UsuarioRegistroDto
{
    [Required(ErrorMessage = "NombreObligatorio")]
    [JsonPropertyName("nombre")]
    [StringLength(50, ErrorMessage = "LongitudNombre")]
    public required string Nombre { get; set; }
    [Required(ErrorMessage = "ContraseñaObligatoria")]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "LongitudContraseña")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).+$", ErrorMessage = "ContraseñaLetraNumero")]
    [JsonPropertyName("password")]
    public required string Contrasena { get; set; }
    [JsonPropertyName("rol")]
    [Required(ErrorMessage = "SeleccionaUsuario")]
    public int Tipo { get; set; } 
    [JsonPropertyName("pregunta")]
    [Required(ErrorMessage = "PreguntaRequerida")]
    public required string Pregunta { get; set; }
    [JsonPropertyName("respuesta")]
    [Required(ErrorMessage = "RespuestaRequerida")]
    public required string Respuesta { get; set; }
    [JsonPropertyName("empresaId")]
    public int? EmpresaId { get; set; }
}
public class UsuarioAcutliazarDto
{
    [Required(ErrorMessage = "NombreObligatorio")]
    [JsonPropertyName("nombre")]
    [StringLength(50, ErrorMessage = "LongitudNombre")]
    public required string Nombre { get; set; }
    [StringLength(16, MinimumLength = 8, ErrorMessage = "LongitudContraseña")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).+$", ErrorMessage = "ContraseñaLetraNumero")]
    [JsonPropertyName("password")]
    public string? Contrasena { get; set; }
    [JsonPropertyName("rol")]
    [Required(ErrorMessage = "SeleccionaUsuario")]
    public required int Tipo { get; set; }
    [JsonPropertyName("pregunta")]
    [Required(ErrorMessage = "PreguntaRequerida")]
    public required string Pregunta { get; set; }
    [JsonPropertyName("respuesta")]
    [Required(ErrorMessage = "RespuestaRequerida")]
    public required string Respuesta { get; set; }
    [JsonPropertyName("empresaId")]
    public int? EmpresaId { get; set; }
    [JsonPropertyName("imagen")]
    public string? Imagen { get; set; }
}

public class UsuarioDto
{
    [JsonPropertyName("nombre")]
    [Required(ErrorMessage = "NombreObligatorio")]
    [StringLength(50, ErrorMessage = "LongitudNombre")]
    public required string Nombre { get; set; }
    [JsonPropertyName("password")]
    [Required(ErrorMessage = "ContraseñaObligatoria")]
    public required string Contrasena { get; set; }
}

public class UsuarioRespuestaDto
{
    [JsonPropertyName("nombre")]
    [StringLength(50, ErrorMessage = "LongitudNombre")]
    public required string Nombre { get; set; }
    [JsonPropertyName("respuesta")]
    public required string Respuesta { get; set; }
}


public class  UsuarioEmpresa
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    [JsonPropertyName("rol")]
    public required int Tipo { get; set; }
    public string TipoNombre { get; set; } = string.Empty;
    public required string Imagen { get; set; }
}

public class UsuarioNombre
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
}
