using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstancieroData;
using EstancieroEntity;
using EstancieroResponse;

namespace EstancieroService
{
    public class JugadorService
    {
        private JugadorData JugadorData =new JugadorData();
        public ApiResponse<Jugador> AgregarJugador(Jugador jugador)
        {
            var response = new ApiResponse<Jugador>();
            try
            {
                // Validación básica
                if (jugador == null)
                {
                    response.Success = false;
                    response.Message = "El jugador no puede ser nulo.";
                    return response;
                }
                if (string.IsNullOrWhiteSpace(jugador.DNI.ToString()) || string.IsNullOrWhiteSpace(jugador.Nombre) || string.IsNullOrWhiteSpace(jugador.Email))
                {
                    response.Success = false;
                    response.Message = "DNI, Nombre y Email son obligatorios.";
                    return response;
                }

                // Verificar si el jugador ya existe por DNI
                var jugadores = JugadorData.GetAll();
                if (jugadores.Any(j => j.DNI == jugador.DNI))
                {
                    response.Success = false;
                    response.Message = "Ya existe un jugador con ese DNI.";
                    return response;
                }

                // Agregar jugador
                var nuevoJugadorData = new JugadorData.Jugador()
                {
                    DNI = jugador.DNI,
                    Nombre = jugador.Nombre,
                    Email = jugador.Email
                };
                var jugadorAgregadoData = JugadorData.EscribirJugador(nuevoJugadorData);

                // Map JugadorData.Jugador back to Jugador (EstancieroEntity)

                var jugadorAgregado = new Jugador(jugadorAgregadoData.DNI,jugadorAgregadoData.Nombre,jugadorAgregadoData.Email);

                response.Success = true;
                response.Message = "Jugador agregado correctamente.";
                response.Data = jugadorAgregado;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al agregar jugador: {ex.Message}";
            }
            return response;
        }
        public ApiResponse<List<Jugador>> ObtenerJugadores()
        {
            var response = new ApiResponse<List<Jugador>>();
            try
            {
                var jugadoresData = JugadorData.GetAll();
                var jugadores = jugadoresData
                    .Select(jd => new Jugador(jd.DNI, jd.Nombre, jd.Email))
                    .ToList();

                response.Success = true;
                response.Message = "Jugadores obtenidos correctamente.";
                response.Data = jugadores;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al obtener jugadores: {ex.Message}";
                response.Data = null;
            }
            return response;
        }
        public ApiResponse<Jugador> ObtenerJugadorPorId(string dni)
        {
            var response = new ApiResponse<Jugador>();
            try
            {
                if (string.IsNullOrWhiteSpace(dni))
                {
                    response.Success = false;
                    response.Message = "El DNI es obligatorio.";
                    return response;
                }

                var jugadoresData = JugadorData.GetAll();
                var jugadorData = jugadoresData.FirstOrDefault(jd => jd.DNI.ToString() == dni);

                if (jugadorData == null)
                {
                    response.Success = false;
                    response.Message = "No se encontró un jugador con ese DNI.";
                    response.Data = null;
                    return response;
                }

                var jugador = new Jugador(jugadorData.DNI, jugadorData.Nombre, jugadorData.Email);
                response.Success = true;
                response.Message = "Jugador obtenido correctamente.";
                response.Data = jugador;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al obtener jugador: {ex.Message}";
                response.Data = null;
            }
            return response;
        }
    }
}