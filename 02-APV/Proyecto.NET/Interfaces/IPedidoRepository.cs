using LowkeysDrops.API.Models.Entities;
namespace LowkeysDrops.API.Interfaces {
    public interface IPedidoRepository {
        Task<int> CrearPedidoAsync(int idCliente, int idDireccion, string metodoPago);
        Task AgregarProductoPedidoAsync(int idPedido, int idProducto, int cantidad);
        Task VerificarPagoAnticipadoAsync(int idPedido, string referencia);
        Task TomarPedidoAsync(int idPedido, int idRepartidor);
        Task MarcarEnCaminoAsync(int idPedido, int idRepartidor);
        Task RegistrarEntregaAsync(int idPedido, int idRepartidor, string? fotoEntregaUrl, string? observacion);
        Task RegistrarEntregaFallidaAsync(int idPedido, int idRepartidor, string? observacion);
        Task ConfirmarRecepcionAsync(int idPedido, int idCliente);
        Task<Pedido?> GetByIdAsync(int id);
    }
}
