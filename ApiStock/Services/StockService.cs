using ApiStock.Models;
namespace ApiStock.Services;

public class StockService : IStockService
{
    private readonly StockContext _context;
    public StockService(StockContext context) => _context = context;
    public async Task MoverRepuestoAsync(int repuestoId, int nuevoCajonId, int usuarioId)
    {
        //Realiza movimiento de repuestos entre cajones, generando el log de la acción
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var repuesto = await _context.Repuestos.FindAsync(repuestoId) ?? throw new Exception("El repuesto no existe.");
            var origenId = repuesto.CajonId;
            repuesto.CajonId = nuevoCajonId;
            //var log = new Logs
            // {
            //   UsuarioId = usuarioId,
            //  RepuestoId = repuestoId,
            //  AccionId = 3,
            //  Mensaje = $"Movido del Cajón {origenId} al {nuevoCajonId}",
            //  Fecha = DateTime.UtcNow
            // };
            //_context.Logs.Add(log);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task AjustarStockDirectoAsync(int repuestoId, int cantidad, int usuarioId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var repuesto = await _context.Repuestos.FindAsync(repuestoId)
            ?? throw new Exception("El repuesto no existe.");
            int nuevoStockFisico = repuesto.StockFisico + cantidad;
            if (nuevoStockFisico < 0)
                throw new Exception("No podés retirar más unidades de las que existen físicamente.");
            if (cantidad < 0 && repuesto.StockDisponible < Math.Abs(cantidad))
                throw new Exception("No hay suficiente stock disponible (las unidades restantes están comprometidas en reservas).");
            if (nuevoStockFisico > 6)
                throw new Exception($"Capacidad excedida. El cajón aloja hasta 6 unidades. Espacio libre actual: {6 - repuesto.StockFisico}");
            repuesto.StockFisico = nuevoStockFisico;
            string tipoOperacion = cantidad > 0 ? "Abastecido" : "Utilizado";
            int accionId = cantidad > 0 ? 6 : 4; 
           /*var log = new Logs <- Hacer al completar Logs
            {
                UsuarioId = usuarioId,
                RepuestoId = repuestoId,
                AccionId = 4,
                Mensaje = $"{tipoOperacion} {Math.Abs(cantidad)} unidad(es). Stock resultante en estante: {repuesto.StockFisico}",
                Fecha = DateTime.UtcNow
            };
            _context.Logs.Add(log);*/
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
