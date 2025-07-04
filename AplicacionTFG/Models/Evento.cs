using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AplicacionTFG.Models;

public class Evento
{
    public int Id { get; set; }
    [StringLength(50, ErrorMessage = "LongitudNombre")]
    public required string Nombre { get; set; }
    [StringLength(50)]
    public required string Color { get; set; } // Color del evento en formato hexadecimal (ejemplo: #FF5733)
    public required DateTime Inicio { get; set; }
    public DateTime? Fin { get; set; }
    [StringLength(250, ErrorMessage = "LongitudDescripcion")]
    public string? Descripcion { get; set; }
    [StringLength(50, ErrorMessage = "LongitudUbicacion")]
    public string? Ubicacion { get; set; }
    public required int UsuarioId { get; set; }
    public required int EmpresaId { get; set; }
    public required int Tipo { get; set; }
    public TareaDetalle? TareaDetalle { get; set; }
}

public class TareaDetalle
{
    public int Id { get; set; }
    [Required(ErrorMessage = "CantidadRequerida")]
    public required double Cantidad { get; set; }
    [Required(ErrorMessage = "UnidadRequerida")]
    public required string Unidad { get; set; }
    public required int EventoId { get; set; }
    [JsonIgnore]
    public Evento Evento { get; set; }
    public required bool Finalizada { get; set; } = false; 
    public ICollection<TareaActualizacion>? Actualizaciones { get; set; }
}

public class EventoDto
{
    [StringLength(50, ErrorMessage = "LongitudNombre")]
    public required string Nombre { get; set; }
    public required string Color { get; set; }
    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    public required DateTime Inicio { get; set; }
    public DateTime? Fin { get; set; }
    [StringLength(250, ErrorMessage = "LongitudDescripcion")]
    public string? Descripcion { get; set; }
    [StringLength(50, ErrorMessage = "LongitudUbicacion")]
    public string? Ubicacion { get; set; }
    public required int UsuarioId { get; set; }
    public required int EmpresaId { get; set; }
    [Required(ErrorMessage = "TipoEventoRequerido")]
    public required int Tipo { get; set; }
    public TareaDetalleDto? TareaDetalle { get; set; } // Relación con la entidad TareaDetalle
}


public class TareaDetalleDto
{
    [Required(ErrorMessage = "CantidadRequerida")]
    public required double Cantidad { get; set; }
    [Required(ErrorMessage = "UnidadRequerida")]
    public required string Unidad { get; set; }
}



public class TareaActualizacion
{
    public int Id { get; set; }
    public required double Cantidad { get; set; }
    public required DateTime Fecha { get; set; }
    public string FechaTexto { get => Fecha.ToString("dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture); }
    public int TareaId { get; set; }
    [JsonIgnore]
    public TareaDetalle TareaDetalle { get; set; }
    public int UsuarioId { get; set; }
    [JsonIgnore]
    public Usuario Usuario { get; set; }
}

public class TareaActualizacionDto
{
    public required double Cantidad { get; set; }
    public required DateTime Fecha { get; set; }
    public int TareaDetalleId { get; set; }
    public int UsuarioId { get; set; }
}

public class EventoMes
{
    public int Id { get; set; }
    public string Nombre { get; set; }= string.Empty;
    public string Color { get; set; } = string.Empty; private string colorLetra = string.Empty;
    [JsonIgnore]
    public string ColorLetra
    {
        get
        {
            var color = ColorTranslator.FromHtml(Color);
            return 0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B < 128 ? "#FFFfff" : "#000000";
        }
        set => colorLetra = value;
    }
    public DateTime Inicio { get; set; }

    public DateTime? Fin { get; set; }
    public int Tipo { get; set; }
    public int UsuarioId { get; set; }
}

public class EventoDia
{
    public required int Id { get; set; }
    public required string Nombre { get; set; }
    public required string Color { get; set; }
    private string colorLetra = string.Empty;
    [JsonIgnore]
    public string ColorLetra
    {
        get
        {
            var color = ColorTranslator.FromHtml(Color);
            var R = color.R;
            return 0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B < 128 ? "#FFFFFF" : "#000000";
        }
        set => colorLetra = value;
    }
    public required DateTime Inicio { get; set; }
    public string InicioHora { get => Inicio.ToString("HH:mm", System.Globalization.CultureInfo.CurrentCulture); }
    public DateTime? Fin { get; set; }
    public string FinHora { get => Fin is not null ? Fin.Value.ToString("HH:mm", System.Globalization.CultureInfo.CurrentCulture) : ""; }
    public required string Ubicacion { get; set; }
    public required string Descripcion { get; set; }
}
