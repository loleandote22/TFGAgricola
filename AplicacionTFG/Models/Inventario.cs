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
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        public required string Nombre { get; set; }
        [MaxLength(50)]
        public required string Tipo { get; set; }
        [MaxLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres")]
        public required string Descripcion { get; set; }
        public required int Cantidad { get; set; }
        public required int EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
        public List<InventarioEvento>? InventarioEventos { get; set; }
        public List<InventarioChat>? InventarioChats { get; set; }
    }

    public class InventarioEvento
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "El tipo no puede tener más de 50 caracteres")]
        public required string Tipo { get; set; }
        public required DateTime Fecha { get; set; }
        public required int Cantidad { get; set; }
        public required int InventarioId { get; set; }
        public required Inventario Inventario { get; set; }
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }

    public class InventarioChat
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [MaxLength(250, ErrorMessage = "El mensaje no puede tener más de 250 caracteres")]
        public required string Mensaje { get; set; }
        public required DateTime Fecha { get; set; }
        public required int InventarioId { get; set; }
        public Inventario? Inventario { get; set; }
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }

    #endregion

    #region Consulta

    public class InventarioConsulta
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        public required string Nombre { get; set; }
        public required string Tipo { get; set; }
        public required int Cantidad { get; set; }
    }

    public class InventarioConsultaCompleto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        public required string Nombre { get; set; }
        public required string Tipo { get; set; }
        public required string Descripcion { get; set; }
        public required int Cantidad { get; set; }
        public required int EmpresaId { get; set; }
        public List<InventarioEventoConsulta>? InventarioEventos { get; set; } = new List<InventarioEventoConsulta>();
        public List<InventarioChatConsulta>? InventarioChats { get; set; } = new List<InventarioChatConsulta>();
    }

    public class InventarioEventoConsulta
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        public required string Tipo { get; set; }
        public required DateTime Fecha { get; set; }
        public required int Cantidad { get; set; }
        public required int InventarioId { get; set; }
        public int? UsuarioId { get; set; }
    }

    public class InventarioChatConsulta
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        public required string Mensaje { get; set; }
        public required DateTime Fecha { get; set; }
        public required int InventarioId { get; set; }
        public int? UsuarioId { get; set; }
    }

    #endregion

    #region DTOs

    public class InventarioDto
    {
        [JsonPropertyName("nombre")]
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        public required string Nombre { get; set; }
        [MaxLength(50)]
        public required string Tipo { get; set; }
        [MaxLength(300, ErrorMessage = "El nombre no puede tener más de 300 caracteres")]
        public required string Descripcion { get; set; }
        public required int Cantidad { get; set; }
        public required int EmpresaId { get; set; }
    }

    public class InventarioActualizaDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        public required string Nombre { get; set; }
        [MaxLength(50)]
        public required string Tipo { get; set; }
        [MaxLength(300, ErrorMessage = "El nombre no puede tener más de 300 caracteres")]
        public required string Descripcion { get; set; }
        public required int Cantidad { get; set; }
        public required int UsuarioId { get; set; }
    }

    public class InventarioChatDto
    {
        [MaxLength(250, ErrorMessage = "El mensaje no puede tener más de 250 caracteres")]
        public required string Mensaje { get; set; }
        public required int InventarioId { get; set; }
        public int? UsuarioId { get; set; }
    }
    #endregion
