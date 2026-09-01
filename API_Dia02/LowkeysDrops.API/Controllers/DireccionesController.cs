using Microsoft.AspNetCore.Mvc;
using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;

namespace LowkeysDrops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionesController : ControllerBase
    {
        private readonly IDireccionService _service;
        public DireccionesController(IDireccionService service) { _service = service; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DireccionResponseDto>>> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<DireccionResponseDto>> Get(int id)
        {
            var direccion = await _service.GetByIdAsync(id);
            if (direccion == null) return NotFound(new { status = 404, message = "Direccion no encontrada." });
            return Ok(direccion);
        }

        [HttpPost]
        public async Task<ActionResult<DireccionResponseDto>> Post([FromBody] DireccionCreateDto dto)
        {
            var direccion = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = direccion.IdDireccion }, direccion);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] DireccionUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
