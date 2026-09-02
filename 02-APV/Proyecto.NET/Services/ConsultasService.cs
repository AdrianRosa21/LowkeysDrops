using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Services {
    public class ConsultasService : IConsultasService {
        private readonly IConsultasRepository _repository;
        public ConsultasService(IConsultasRepository repository) { _repository = repository; }
        
        public async Task<IEnumerable<VwCatalogoDisponible>> GetCatalogoDisponibleAsync() { return await _repository.GetCatalogoDisponibleAsync(); }
        public async Task<IEnumerable<VwPedidosResuman>> GetPedidosResumenAsync() { return await _repository.GetPedidosResumenAsync(); }
        public async Task<IEnumerable<VwPedidosDisponiblesRepartidor>> GetPedidosDisponiblesRepartidorAsync() { return await _repository.GetPedidosDisponiblesRepartidorAsync(); }
        public async Task<IEnumerable<VwVentasEntregada>> GetVentasEntregadasAsync() { return await _repository.GetVentasEntregadasAsync(); }
        public async Task<IEnumerable<VwAuditoriaReciente>> GetAuditoriaRecienteAsync() { return await _repository.GetAuditoriaRecienteAsync(); }
    }
}
