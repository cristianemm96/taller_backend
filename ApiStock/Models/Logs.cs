using System.ComponentModel.DataAnnotations;

namespace ApiStock.Models;
public class Logs
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(200)]
    public string Mensaje { get; set; } = string.Empty;
    [Required]
    public int UsuarioId { get; set; }
    public virtual Usuario Usuario { get; set; }
    [Required]
    public DateTime Fecha { get; set; }
    [Required]
    public int AccionId { get; set; }
    public virtual Accion AccionDetalle { get; set; }
    public int? RepuestoId { get; set; }
    public virtual Repuesto Repuesto { get; set; }
}