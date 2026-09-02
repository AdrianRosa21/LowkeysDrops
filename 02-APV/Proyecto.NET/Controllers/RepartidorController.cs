using Microsoft.AspNetCore.Mvc;
using LowkeysDrops.API.Interfaces;

namespace LowkeysDrops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepartidorController : ControllerBase
    {
        private readonly IConsultasService _service;
        public RepartidorController(IConsultasService service) { _service = service; }

        [HttpGet("pedidos-disponibles")]
        public async Task<ActionResult> GetPedidosDisponibles()
        {
            var pedidos = await _service.GetPedidosDisponiblesRepartidorAsync();
            return Ok(pedidos);
        }
    }
}
