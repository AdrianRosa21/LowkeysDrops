using Microsoft.AspNetCore.Mvc;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace LowkeysDrops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly IConsultasService _service;
        private readonly IUsuarioService _usuarioService;
        
        public AdminController(IConsultasService service, IUsuarioService usuarioService) 
        { 
            _service = service; 
            _usuarioService = usuarioService;
        }

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

        [HttpPost("repartidores")]
        public async Task<IActionResult> CrearRepartidor([FromBody] RegistroRepartidorDto dto)
        {
            var repartidor = await _usuarioService.CrearRepartidorAsync(dto);
            return StatusCode(201, repartidor);
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            return Ok(await _usuarioService.ObtenerTodosAsync());
        }

        [HttpGet("usuarios/{id}")]
        public async Task<IActionResult> ObtenerUsuario(int id)
        {
            return Ok(await _usuarioService.ObtenerPorIdAsync(id));
        }

        [HttpPut("usuarios/{id}/estado")]
        public async Task<IActionResult> CambiarEstadoUsuario(int id, [FromBody] CambiarEstadoUsuarioDto dto)
        {
            await _usuarioService.CambiarEstadoAsync(id, dto);
            return NoContent();
        }
    }
}
