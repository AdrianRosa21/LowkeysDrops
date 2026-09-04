using Microsoft.AspNetCore.Mvc;
using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;

namespace LowkeysDrops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _service;
        private readonly IConsultasService _consultasService;
        
        public PedidosController(IPedidoService service, IConsultasService consultasService) { 
            _service = service; 
            _consultasService = consultasService;
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult> GetByCliente(int idCliente)
        {
            var pedidos = await _consultasService.GetPedidosClienteAsync(idCliente);
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var pedido = await _service.GetByIdAsync(id);
            if (pedido == null) return NotFound(new { status = 404, message = "Pedido no encontrado." });

            var response = new
            {
                pedido.IdPedido,
                pedido.IdCliente,
                pedido.IdDireccion,
                pedido.FechaPedido,
                pedido.Subtotal,
                pedido.CostoEnvio,
                pedido.Total,
                pedido.Estado,
                Detalles = pedido.DetallePedidos.Select(d => new
                {
                    d.IdDetalle,
                    d.IdProducto,
                    d.Cantidad,
                    d.PrecioUnitario
                }),
                Pago = pedido.Pago != null ? new
                {
                    pedido.Pago.IdPago,
                    pedido.Pago.Metodo,
                    pedido.Pago.Estado,
                    pedido.Pago.Referencia,
                    pedido.Pago.FechaPago
                } : null,
                Entrega = pedido.Entrega != null ? new
                {
                    pedido.Entrega.IdEntrega,
                    pedido.Entrega.IdRepartidor,
                    pedido.Entrega.Estado,
                    pedido.Entrega.FechaTomado,
                    pedido.Entrega.FechaEntrega
                } : null
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PedidoCreateDto dto)
        {
            var idPedido = await _service.CrearPedidoAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = idPedido }, new { status = 201, message = "Pedido creado.", idPedido });
        }

        [HttpPost("{id}/productos")]
        public async Task<ActionResult> AgregarProducto(int id, [FromBody] AgregarProductoPedidoDto dto)
        {
            await _service.AgregarProductoPedidoAsync(id, dto);
            return Ok(new { status = 200, message = "Producto agregado exitosamente." });
        }

        [HttpPut("{id}/pago/verificar")]
        public async Task<ActionResult> VerificarPago(int id, [FromBody] VerificarPagoDto dto)
        {
            await _service.VerificarPagoAnticipadoAsync(id, dto);
            return Ok(new { status = 200, message = "Pago verificado exitosamente." });
        }

        [HttpPost("{id}/tomar")]
        public async Task<ActionResult> TomarPedido(int id, [FromBody] TomarPedidoDto dto)
        {
            await _service.TomarPedidoAsync(id, dto);
            return Ok(new { status = 200, message = "Pedido tomado exitosamente." });
        }

        [HttpPut("{id}/en-camino")]
        public async Task<ActionResult> EnCamino(int id, [FromBody] MarcarEnCaminoDto dto)
        {
            await _service.MarcarEnCaminoAsync(id, dto);
            return Ok(new { status = 200, message = "Pedido marcado en camino." });
        }

        [HttpPut("{id}/entrega")]
        public async Task<ActionResult> RegistrarEntrega(int id, [FromBody] RegistrarEntregaDto dto)
        {
            await _service.RegistrarEntregaAsync(id, dto);
            return Ok(new { status = 200, message = "Entrega registrada exitosamente." });
        }

        [HttpPut("{id}/entrega-fallida")]
        public async Task<ActionResult> RegistrarEntregaFallida(int id, [FromBody] RegistrarEntregaFallidaDto dto)
        {
            await _service.RegistrarEntregaFallidaAsync(id, dto);
            return Ok(new { status = 200, message = "Entrega fallida registrada." });
        }

        [HttpPut("{id}/confirmar-recepcion")]
        public async Task<ActionResult> ConfirmarRecepcion(int id, [FromBody] ConfirmarRecepcionDto dto)
        {
            await _service.ConfirmarRecepcionAsync(id, dto);
            return Ok(new { status = 200, message = "Recepción confirmada exitosamente." });
        }
    }
}
