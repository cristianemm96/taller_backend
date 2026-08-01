namespace ApiStock.Dto.Orden;

public class OrdenMecanicoDto
    {
        public int Id { get; set; }
        public string DescripcionTrabajo { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int MecanicoId { get; set; }
        public string NombreMecanico { get; set; } = string.Empty;
        public int CantidadDetalles { get; set; } 
    }