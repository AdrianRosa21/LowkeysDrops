using Microsoft.AspNetCore.Mvc;
using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;

namespace LowkeysDrops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropsController : ControllerBase
    {
        private readonly IDropService _service;
        public DropsController(IDropService service) { _service = service; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DropResponseDto>>> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<DropResponseDto>> Get(int id)
        {
            var drop = await _service.GetByIdAsync(id);
            if (drop == null) return NotFound(new { status = 404, message = "Drop no encontrado." });
            return Ok(drop);
        }

        [HttpPost]
        public async Task<ActionResult<DropResponseDto>> Post([FromBody] DropCreateDto dto)
        {
            var drop = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = drop.IdDrop }, drop);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] DropUpdateDto dto)
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
