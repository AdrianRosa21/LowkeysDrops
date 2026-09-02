using LowkeysDrops.API.Models.Entities;
namespace LowkeysDrops.API.Interfaces {
    public interface IConsultasService {
        Task<IEnumerable<VwCatalogoDisponible>> GetCatalogoDisponibleAsync();
        Task<IEnumerable<VwPedidosResuman>> GetPedidosResumenAsync();
        Task<IEnumerable<VwPedidosDisponiblesRepartidor>> GetPedidosDisponiblesRepartidorAsync();
        Task<IEnumerable<VwVentasEntregada>> GetVentasEntregadasAsync();
        Task<IEnumerable<VwAuditoriaReciente>> GetAuditoriaRecienteAsync();
    }
}
