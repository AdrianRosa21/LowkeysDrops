using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using LowkeysDrops.API.Data;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Repositories {
    public class PedidoRepository : IPedidoRepository {
        private readonly LowkeysDropsDbContext _context;
        public PedidoRepository(LowkeysDropsDbContext context) { _context = context; }

        public async Task<int> CrearPedidoAsync(int idCliente, int idDireccion, string metodoPago) {
            var idPedidoParam = new SqlParameter {
                ParameterName = "@IdPedido",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            var p1 = new SqlParameter("@IdCliente", idCliente);
            var p2 = new SqlParameter("@IdDireccion", idDireccion);
            var p3 = new SqlParameter("@MetodoPago", metodoPago);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CrearPedido @IdCliente, @IdDireccion, @MetodoPago, @IdPedido OUTPUT",
                p1, p2, p3, idPedidoParam);

            return (int)idPedidoParam.Value;
        }

        public async Task AgregarProductoPedidoAsync(int idPedido, int idProducto, int cantidad) {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_AgregarProductoPedido @IdPedido, @IdProducto, @Cantidad",
                new SqlParameter("@IdPedido", idPedido),
                new SqlParameter("@IdProducto", idProducto),
                new SqlParameter("@Cantidad", cantidad));
        }

        public async Task VerificarPagoAnticipadoAsync(int idPedido, string referencia) {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_VerificarPagoAnticipado @IdPedido, @Referencia",
                new SqlParameter("@IdPedido", idPedido),
                new SqlParameter("@Referencia", referencia));
        }

        public async Task TomarPedidoAsync(int idPedido, int idRepartidor) {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_TomarPedido @IdPedido, @IdRepartidor",
                new SqlParameter("@IdPedido", idPedido),
                new SqlParameter("@IdRepartidor", idRepartidor));
        }

        public async Task MarcarEnCaminoAsync(int idPedido, int idRepartidor) {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_MarcarEnCamino @IdPedido, @IdRepartidor",
                new SqlParameter("@IdPedido", idPedido),
                new SqlParameter("@IdRepartidor", idRepartidor));
        }

        public async Task RegistrarEntregaAsync(int idPedido, int idRepartidor, string? fotoEntregaUrl, string? observacion) {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_RegistrarEntrega @IdPedido, @IdRepartidor, @FotoEntregaUrl, @Observacion",
                new SqlParameter("@IdPedido", idPedido),
                new SqlParameter("@IdRepartidor", idRepartidor),
                new SqlParameter("@FotoEntregaUrl", (object)fotoEntregaUrl ?? DBNull.Value),
                new SqlParameter("@Observacion", (object)observacion ?? DBNull.Value));
        }

        public async Task RegistrarEntregaFallidaAsync(int idPedido, int idRepartidor, string? observacion) {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_RegistrarEntregaFallida @IdPedido, @IdRepartidor, @Observacion",
                new SqlParameter("@IdPedido", idPedido),
                new SqlParameter("@IdRepartidor", idRepartidor),
                new SqlParameter("@Observacion", (object)observacion ?? DBNull.Value));
        }

        public async Task ConfirmarRecepcionAsync(int idPedido, int idCliente) {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_ConfirmarRecepcion @IdPedido, @IdCliente",
                new SqlParameter("@IdPedido", idPedido),
                new SqlParameter("@IdCliente", idCliente));
        }

        public async Task<Pedido?> GetByIdAsync(int id) {
            return await _context.Pedidos
                .Include(p => p.DetallePedidos)
                .Include(p => p.Pago)
                .Include(p => p.Entrega)
                .FirstOrDefaultAsync(p => p.IdPedido == id);
        }
    }
}
