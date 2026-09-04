using LowkeysDrops.API.Models.Entities;
namespace LowkeysDrops.API.Interfaces {
    public interface IConsultasRepository {
        Task<IEnumerable<VwCatalogoDisponible>> GetCatalogoDisponibleAsync();
        Task<IEnumerable<VwPedidosResuman>> GetPedidosResumenAsync();
        Task<IEnumerable<VwPedidosResuman>> GetPedidosClienteAsync(int idCliente);
        Task<IEnumerable<VwPedidosDisponiblesRepartidor>> GetPedidosDisponiblesRepartidorAsync();
        Task<IEnumerable<VwVentasEntregada>> GetVentasEntregadasAsync();
        Task<IEnumerable<VwAuditoriaReciente>> GetAuditoriaRecienteAsync();
    }
}
