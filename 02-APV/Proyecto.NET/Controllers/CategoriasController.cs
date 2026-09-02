using Microsoft.AspNetCore.Mvc;
using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;

namespace LowkeysDrops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _service;
        public CategoriasController(ICategoriaService service) { _service = service; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaResponseDto>>> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaResponseDto>> Get(int id)
        {
            var categoria = await _service.GetByIdAsync(id);
            if (categoria == null) return NotFound(new { status = 404, message = "Categoria no encontrada." });
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaResponseDto>> Post([FromBody] CategoriaCreateDto dto)
        {
            var categoria = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = categoria.IdCategoria }, categoria);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaUpdateDto dto)
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
