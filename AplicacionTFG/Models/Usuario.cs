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
    public required string Nombre { get; set; }
    [MinLength(8, ErrorMessage = "La contraseña debe tener un mínimo de 8 caracteres")]
    [StringLength(16, ErrorMessage = "La contraseña debe tener un máximo de 16 caracteres")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).+$", ErrorMessage = "La contraseña debe contener al menos una letra y un número.")]
    [JsonPropertyName("password")]
    public required string Contrasena { get; set; }
    [JsonPropertyName("rol")]
    public required string Tipo { get; set; } // Dueño, Administrador, Empleado
    [JsonPropertyName("pregunta")]
    public required string Pregunta { get; set; }
    [JsonPropertyName("respuesta")]
    public required string Respuesta { get; set; }
    [JsonPropertyName("empresaId")]
    public int? EmpresaId { get; set; }
}

public class UsuarioRegistroDto
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [JsonPropertyName("nombre")]
    public required string Nombre { get; set; }
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener un mínimo de 8 caracteres")]
    [StringLength(16, ErrorMessage = "La contraseña debe tener un máximo de 16 caracteres")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).+$", ErrorMessage = "La contraseña debe contener al menos una letra y un número.")]
    [JsonPropertyName("password")]
    public required string Contrasena { get; set; }
    [JsonPropertyName("rol")]
    [Required(ErrorMessage = "Selecciona el tipo de usuario")]
    public required string Tipo { get; set; } // Dueño, Administrador, Empleado
    [JsonPropertyName("pregunta")]
    [Required(ErrorMessage = "Introduce una pregunta de seguridad")]
    public required string Pregunta { get; set; }
    [JsonPropertyName("respuesta")]
    [Required(ErrorMessage = "La pregunta de seguridad requiere respuesta")]
    public required string Respuesta { get; set; }
    [JsonPropertyName("empresaId")]
    public int? EmpresaId { get; set; }
}

public class UsuarioDto
{
    [JsonPropertyName("nombre")]
    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    public required string Nombre { get; set; }
    [JsonPropertyName("password")]
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public required string Contrasena { get; set; }
}

public class UsuarioRespuestaDto
{
    [JsonPropertyName("nombre")]
    public required string Nombre { get; set; }
    [JsonPropertyName("respuesta")]
    public required string Respuesta { get; set; }
}
