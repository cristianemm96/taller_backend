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
            var repuesto = await _context.Repuestos.FindAsync(repuestoId)?? throw new Exception("El repuesto no existe."); 
            var origenId = repuesto.CajonId;
            repuesto.CajonId = nuevoCajonId;
            var log = new Logs
            {
                UsuarioId = usuarioId,
                RepuestoId = repuestoId,
                AccionId = 3,
                Mensaje = $"Movido del Cajón {origenId} al {nuevoCajonId}",
                Fecha = DateTime.UtcNow
            };
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UtilizarRepuestoAsync(int repuestoId, int usuarioId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var repuesto = await _context.Repuestos.FindAsync(repuestoId) ?? throw new Exception("El repuesto no existe.");
            if (repuesto.Stock <= 0) throw new Exception("No hay stock disponible.");
            repuesto.Stock -= 1;
            int stockRestante = repuesto.Stock;
            var log = new Logs
            {
                UsuarioId = usuarioId,
                RepuestoId = repuestoId,
                AccionId = 4,
                Mensaje = $"Utilizado 1 unidad. Cantidad restante: {stockRestante}",
                Fecha = DateTime.UtcNow
            };
            _context.Logs.Add(log);
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
