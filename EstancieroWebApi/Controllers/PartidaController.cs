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
        [HttpPut("/PausarPartida/{id}")]
        public IActionResult PausarPartida([FromRoute] int id)
        {
            var request = new CambiarEstadoPartida { NumeroPartida = id };
            var resultado = _svc.PausarPartida(request);
            if (resultado.Success)
            {
                return Ok(resultado);
            }
            return BadRequest(resultado);
        }
        [HttpPut("/ReanudarPartida/{id}")]
        public IActionResult ReanudarPartida([FromRoute] int id)
        {
            var request = new CambiarEstadoPartida { NumeroPartida = id };
            var resultado = _svc.ReanudarPartida(request);
            if (resultado.Success)
            {
                return Ok(resultado);
            }
            return BadRequest(resultado);
        }
        [HttpPut("/SuspenderPartida/{id}")]
        public IActionResult SuspenderPartida([FromRoute] int id)
        {
            var request = new CambiarEstadoPartida { NumeroPartida = id };
            var resultado = _svc.SuspenderPartida(request);
            if (resultado.Success)
            {
                return Ok(resultado);
            }
            return BadRequest(resultado);
        }
        [HttpPut("/LanzarDado/{numeroPartida}/{dniJugador}")]
        public IActionResult LanzarDado([FromRoute] int numeroPartida, [FromRoute] int dniJugador)
        {
            var request = new LanzarDado
            {
                NumeroPartida = numeroPartida,
                DniJugador = dniJugador
            };
            var resultado = _svc.LanzarDado(request);
            if (resultado.Success)
            {
                return Ok(resultado);
            }
            return BadRequest(resultado);
        }
    }
}
