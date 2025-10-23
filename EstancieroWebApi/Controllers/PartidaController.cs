using Microsoft.AspNetCore.Mvc;
using EstancieroService;
using EstancieroRequest;
using EstancieroResponse;

namespace EstancieroWebAppi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PartidaController : ControllerBase
    {
        private readonly PartidaService _svc;

        [HttpPost("/CrearPartida")]
        public IActionResult CrearPartida([FromBody] CrearPartida request)
        {
            var resultado = _svc.CrearPartida(request);
            if (!resultado.Success)
            {
                return BadRequest(resultado);
            }
            return CreatedAtAction(nameof(CrearPartida), new { numeroPartida = resultado.Data.NumeroPartida }, resultado);
        }

        [HttpGet("/BuscarPartida/{id}")]
        public IActionResult BuscarPartidaId([FromRoute] int id)
        {
            var request = new BuscarPartida { NumeroPartida = id };
            var resultado = _svc.BuscarPartidaId(request);
            if (!resultado.Success)
            {
                return BadRequest(resultado);
            }
            return Ok(resultado);
        }
    }
}