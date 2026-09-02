using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Services {
    public class PedidoService : IPedidoService {
        private readonly IPedidoRepository _repository;
        public PedidoService(IPedidoRepository repository) { _repository = repository; }

        public async Task<int> CrearPedidoAsync(PedidoCreateDto dto) {
            return await _repository.CrearPedidoAsync(dto.IdCliente, dto.IdDireccion, dto.MetodoPago);
        }

        public async Task AgregarProductoPedidoAsync(int idPedido, AgregarProductoPedidoDto dto) {
            await _repository.AgregarProductoPedidoAsync(idPedido, dto.IdProducto, dto.Cantidad);
        }

        public async Task VerificarPagoAnticipadoAsync(int idPedido, VerificarPagoDto dto) {
            await _repository.VerificarPagoAnticipadoAsync(idPedido, dto.Referencia);
        }

        public async Task TomarPedidoAsync(int idPedido, TomarPedidoDto dto) {
            await _repository.TomarPedidoAsync(idPedido, dto.IdRepartidor);
        }

        public async Task MarcarEnCaminoAsync(int idPedido, MarcarEnCaminoDto dto) {
            await _repository.MarcarEnCaminoAsync(idPedido, dto.IdRepartidor);
        }

        public async Task RegistrarEntregaAsync(int idPedido, RegistrarEntregaDto dto) {
            await _repository.RegistrarEntregaAsync(idPedido, dto.IdRepartidor, dto.FotoEntregaUrl, dto.Observacion);
        }

        public async Task RegistrarEntregaFallidaAsync(int idPedido, RegistrarEntregaFallidaDto dto) {
            await _repository.RegistrarEntregaFallidaAsync(idPedido, dto.IdRepartidor, dto.Observacion);
        }

        public async Task ConfirmarRecepcionAsync(int idPedido, ConfirmarRecepcionDto dto) {
            await _repository.ConfirmarRecepcionAsync(idPedido, dto.IdCliente);
        }

        public async Task<Pedido?> GetByIdAsync(int id) {
            return await _repository.GetByIdAsync(id);
        }
    }
}
