using EstancieroEntity;
using EstancieroResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EstancieroService;

namespace EstancieroWebApi.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class JugadorController : ControllerBase
    {
        private JugadorService JugadorService = new JugadorService();

        //[HttpPost]
        //public IActionResult AgregarJugador([FromBody] Jugador nuevoJugador)
        //{
        //    if (nuevoJugador == null || string.IsNullOrWhiteSpace(nuevoJugador.DNI.ToString()) || string.IsNullOrWhiteSpace(nuevoJugador.Nombre) || string.IsNullOrWhiteSpace(nuevoJugador.Email))
        //    {
        //        return BadRequest("Datos de jugador inválidos.");
        //    }
        //    var response = JugadorService.AgregarJugador(nuevoJugador);
        //    if (!response.Success)
        //    {
        //        return BadRequest(response.Message); 
        //    }

        //    return Ok("Jugador agregado correctamente.");
        //}
        [HttpGet]
        public IActionResult ObtenerListadoJugadores()
        {
            var response = JugadorService.ObtenerJugadores();
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }
        //[HttpGet("{dni}")]
        //public IActionResult ObtenerJugadorId(string dni)
        //{
        //    if (string.IsNullOrWhiteSpace(dni))
        //    {
        //        return BadRequest("DNI inválido.");
        //    }
        //    var response = JugadorService.ObtenerJugadorPorId(dni);
        //    if (!response.Success)
        //    {
        //        return NotFound(response.Message);
        //    }
        //    return Ok(response.Data);
        //}
    }
}
