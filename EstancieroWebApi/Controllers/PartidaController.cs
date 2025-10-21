using Microsoft.AspNetCore.Mvc;
using EstancieroService;
using EstancieroRequest;
using EstancieroResponse;

namespace EstancieroWebAppi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidaController : ControllerBase
    {
        private readonly PartidaService _svc = new();

        [HttpPost("crear")]
        public IActionResult CrearPartida([FromBody] CrearPartida request)
        {
            var resultado = _svc.CrearPartida(request);
            if (!resultado.Success) return BadRequest(resultado);
            return Ok(resultado);
        }

        [HttpPost("lanzarDado")]
        public IActionResult LanzarDado([FromBody] LanzarDado request)
        {
            var resultado = _svc.LanzarDado(request);
            if (!resultado.Success) return BadRequest(resultado);
            return Ok(resultado);
        }
    }
}