using Microsoft.AspNetCore.Mvc;
using LowkeysDrops.API.Interfaces;

namespace LowkeysDrops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConsultasService _service;
        public AdminController(IConsultasService service) { _service = service; }

        [HttpGet("pedidos")]
        public async Task<ActionResult> GetPedidos()
        {
            var pedidos = await _service.GetPedidosResumenAsync();
            return Ok(pedidos);
        }
        
        [HttpGet("ventas")]
        public async Task<ActionResult> GetVentas()
        {
            var ventas = await _service.GetVentasEntregadasAsync();
            return Ok(ventas);
        }
        
        [HttpGet("auditoria")]
        public async Task<ActionResult> GetAuditoria()
        {
            var auditoria = await _service.GetAuditoriaRecienteAsync();
            return Ok(auditoria);
        }
    }
}
