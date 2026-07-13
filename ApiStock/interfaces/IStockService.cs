public interface IStockService
{
    Task MoverRepuestoAsync(int repuestoId, int nuevoCajonId, int usuarioId);
    Task AjustarStockDirectoAsync(int repuestoId, int cantidad, int usuarioId);
}