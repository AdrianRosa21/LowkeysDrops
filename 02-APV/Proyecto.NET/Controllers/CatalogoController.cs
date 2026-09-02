using Microsoft.AspNetCore.Mvc;
using LowkeysDrops.API.Interfaces;

namespace LowkeysDrops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoController : ControllerBase
    {
        private readonly IConsultasService _service;
        public CatalogoController(IConsultasService service) { _service = service; }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var catalogo = await _service.GetCatalogoDisponibleAsync();
            return Ok(catalogo);
        }
    }
}
