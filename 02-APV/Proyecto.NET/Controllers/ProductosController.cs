using Microsoft.AspNetCore.Mvc;
using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;

namespace LowkeysDrops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductosController(IProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoResponseDto>>> Get()
        {
            var productos = await _service.GetAllAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoResponseDto>> Get(int id)
        {
            var producto = await _service.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound(new { status = 404, message = "Producto no encontrado." });
            }
            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoResponseDto>> Post([FromBody] ProductoCreateDto dto)
        {
            var producto = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = producto.IdProducto }, producto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductoUpdateDto dto)
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
        
        [HttpGet("{id}/resenas")]
        public async Task<ActionResult<IEnumerable<ResenaResponseDto>>> GetResenas(int id, [FromServices] IResenaService resenaService)
        {
            var resenas = await resenaService.GetByProductoIdAsync(id);
            return Ok(resenas);
        }
        
        [HttpPost("{id}/resenas")]
        public async Task<ActionResult<ResenaResponseDto>> PostResena(int id, [FromBody] ResenaCreateDto dto, [FromServices] IResenaService resenaService)
        {
            var resena = await resenaService.CreateAsync(id, dto);
            return Created(string.Empty, resena);
        }
    }
}
