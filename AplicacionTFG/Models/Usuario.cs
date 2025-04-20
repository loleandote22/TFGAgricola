using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AplicacionTFG.Models
{
    public class Usuario
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [JsonPropertyName("apellido")]
        public string Apellido { get; set; }
        [JsonPropertyName("correo")]
        public string Correo { get; set; }
        [JsonPropertyName("telefono")]
        public string? Telefono { get; set; }
        [JsonPropertyName("contrasena")]
        public string Contrasena { get; set; }
    }
    public class UsuarioDto
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [JsonPropertyName("contrasena")]
        public string Contrasena { get; set; }
    }
}
