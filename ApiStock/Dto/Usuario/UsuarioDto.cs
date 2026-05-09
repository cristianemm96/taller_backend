namespace ApiStock.Dto.Usuario;

public class UsuarioDto
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono {get;set;} = string.Empty;
    public string? UrlFoto { get; set; }
    public string NombreRol { get; set; } = string.Empty; 
    public bool Activo { get; set; }
}