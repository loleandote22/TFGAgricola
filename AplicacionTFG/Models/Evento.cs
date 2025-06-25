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
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
    public required string Nombre { get; set; }
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
    public required string Color { get; set; } // Color del evento en formato hexadecimal (ejemplo: #FF5733)
    public required DateTime Inicio { get; set; }
    public DateTime? Fin { get; set; }
    [StringLength(250, ErrorMessage = "El nombre no puede tener más de 250 caracteres")]
    public string? Descripcion { get; set; }
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
    public string? Ubicacion { get; set; }
    public required int UsuarioId { get; set; }
    public required int EmpresaId { get; set; }
    public required int Tipo { get; set; } // 1: Evento, 2: Tarea, 3: Recordatorio
}

public class EventoDto
{

    [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
    public required string Nombre { get; set; }
    public required string Color { get; set; }
    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    public required DateTime Inicio { get; set; }
    public DateTime? Fin { get; set; }
    [StringLength(250, ErrorMessage = "El nombre no puede tener más de 250 caracteres")]
    public string? Descripcion { get; set; }
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
    public string? Ubicacion { get; set; }
    public required int UsuarioId { get; set; }
    public required int EmpresaId { get; set; }
    [Required(ErrorMessage = "El tipo de evento es obligatorio")]
    public required int Tipo { get; set; } // 1: Evento, 2: Tarea, 3: Recordatorio
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
    public int Tipo { get; set; }
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
