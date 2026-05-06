public interface IStockService
{
    Task MoverRepuestoAsync(int repuestoId, int nuevoCajonId, int usuarioId);
    Task UtilizarRepuestoAsync(int repuestoId, int usuarioId);
}