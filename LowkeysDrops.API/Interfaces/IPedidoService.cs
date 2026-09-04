using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Models.Entities;
namespace LowkeysDrops.API.Interfaces {
    public interface IPedidoService {
        Task<int> CrearPedidoAsync(PedidoCreateDto dto);
        Task AgregarProductoPedidoAsync(int idPedido, AgregarProductoPedidoDto dto);
        Task VerificarPagoAnticipadoAsync(int idPedido, VerificarPagoDto dto);
        Task TomarPedidoAsync(int idPedido, TomarPedidoDto dto);
        Task MarcarEnCaminoAsync(int idPedido, MarcarEnCaminoDto dto);
        Task RegistrarEntregaAsync(int idPedido, RegistrarEntregaDto dto);
        Task RegistrarEntregaFallidaAsync(int idPedido, RegistrarEntregaFallidaDto dto);
        Task ConfirmarRecepcionAsync(int idPedido, ConfirmarRecepcionDto dto);
        Task<Pedido?> GetByIdAsync(int id);
    }
}
