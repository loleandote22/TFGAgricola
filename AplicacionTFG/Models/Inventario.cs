using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AplicacionTFG.Models;
    #region Base

    public class Inventario
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        [Required(ErrorMessage = "NombreObligatorio")]
        [StringLength(50, ErrorMessage = "LongitudNombre")]
        public required string Nombre { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "TipoObligatorio")]
        [JsonPropertyName("tipo")]
        public required string Tipo { get; set; }
        [Required(ErrorMessage = "DescripcionObligatoria")]
        [JsonPropertyName("descripcion")]
        [StringLength(200, ErrorMessage = "LongitudDescripcion200")]
        public required string Descripcion { get; set; }
        [Required(ErrorMessage = "CantidadRequerida")]
        [JsonPropertyName("cantidad")]
        public required int Cantidad { get; set; }
        [JsonPropertyName("empresaId")]
        public required int EmpresaId { get; set; }
        [JsonPropertyName("inventarioEventos")]
        public List<InventarioEvento>? InventarioEventos { get; set; }
        [JsonPropertyName("inventarioChats")]
        public List<InventarioChat>? InventarioChats { get; set; }
    }

    public class InventarioEvento
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("tipo")]
        public required int Tipo { get; set; }
        [JsonPropertyName("fecha")]
        public required DateTime Fecha { get; set; }
        [JsonPropertyName("cantidad")]
        public required int Cantidad { get; set; }
        [JsonPropertyName("inventarioId")]
        public required int InventarioId { get; set; }
        [JsonPropertyName("usuarioNombre")]
        public required string UsuarioNombre { get; set; }
    }

    public class InventarioChat
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [StringLength(250, ErrorMessage = "LongitudMensaje")]
        [JsonPropertyName("mensaje")]
        public required string Mensaje { get; set; }
        [JsonPropertyName("fecha")]
        public required DateTime Fecha { get; set; }
        [JsonPropertyName("inventarioId")]
        public required int InventarioId { get; set; }
        [JsonPropertyName("usuarioId")]
        public int? UsuarioId { get; set; }
    }

    public class InventarioConsulta
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        public required string Nombre { get; set; }
        [JsonPropertyName("descripcion")]
        public required string Descripcion { get; set; }
        [JsonIgnore]
        private string? _descripcionCorta;
        [JsonIgnore]
        public string DescripcionCorta
        {
            get
            {
                if (!string.IsNullOrEmpty(_descripcionCorta))
                    return _descripcionCorta;
                if (Descripcion == null)
                    return string.Empty;
                if (Descripcion.Length > 75)
                    _descripcionCorta = Descripcion.Substring(0, 75);
                return Descripcion.Length > 75 ? _descripcionCorta!.Substring(0,_descripcionCorta.LastIndexOf(" "))+"..." : Descripcion;
            }
            set
            {
                _descripcionCorta = value;
            }
        }
        [JsonPropertyName("tipo")]
        public required int Tipo { get; set; }
        [JsonPropertyName("cantidad")]
        public required int Cantidad { get; set; }
        [JsonPropertyName("empresaId")]
        public required int EmpresaId { get; set; }
        [JsonPropertyName("unidad")]
        public required string Unidad { get; set; }
    }


    public class InventarioEventoConsulta
    {
        public required int Tipo { get; set; }

        [JsonIgnore]
        public string TipoNombre { get; set; }
        public required DateTime Fecha { get; set; }
        public required int Cantidad { get; set; }
        [JsonPropertyName("usuarioNombre")]
        public string? UsuarioNombre { get; set; }
    }

    public class InventarioChatConsulta
    {
        public required string Mensaje { get; set; }
        public required DateTime Fecha { get; set; }
        public string? UsuarioNombre { get; set; }
    }

    #endregion

    #region DTOs

    public class InventarioDto
    {
        [JsonPropertyName("nombre")]
        [Required(ErrorMessage = "NombreObligatorio")]
        [StringLength(50, ErrorMessage = "LongitudNombre")]
        public required string Nombre { get; set; }
        [Required(ErrorMessage = "TipoObligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "TipoObligatorio")]
        public required int Tipo { get; set; }
        [JsonPropertyName("descripcion")]
        public required string Descripcion { get; set; }
        [Required(ErrorMessage ="CantidadRequerida")]
        public required int Cantidad { get; set; }
        public required int EmpresaId { get; set; }
        [JsonPropertyName("unidad")]
        [Required(ErrorMessage = "UnidadRequerida")]
        [StringLength(50, ErrorMessage = "LongitudUnidad")]
        public required string Unidad { get; set; }
}

    public class InventarioActualizaDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        [Required(ErrorMessage = "NombreObligatorio")]
        [StringLength(50, ErrorMessage = "LongitudNombre")]
        public required string Nombre { get; set; }
        [Required(ErrorMessage = "TipoObligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "TipoObligatorio")]
        [JsonPropertyName("tipo")]
        public required int Tipo { get; set; }
        [JsonPropertyName("descripcion")]
        public required string Descripcion { get; set; }
        [JsonPropertyName("cantidad")]
        [Required(ErrorMessage = "CantidadRequerida")]
        public required int Cantidad { get; set; }
        [JsonPropertyName("usuarioId")]
        public required int UsuarioId { get; set; }

        [JsonPropertyName("unidad")]
        [Required(ErrorMessage = "UnidadRequerida")]
        [StringLength(50, ErrorMessage = "LongitudUnidad")]
        public required string Unidad { get; set; }
    }

    public class InventarioChatDto
    {
        [StringLength(250, ErrorMessage = "LongitudMensaje")]
        public required string Mensaje { get; set; }
        public required int InventarioId { get; set; }
        public int? UsuarioId { get; set; }
    }

#endregion
