using EstancieroData;
using EstancieroEntity;
using EstancieroRequest;
using EstancieroResponse;
using Newtonsoft.Json;

namespace EstancieroService
{
    public class PartidaService
    {
        private readonly PartidasData _partidaData;
        private readonly PartidaDetalleData _partidaDetalleData;
        private readonly JugadorData _jugadorData;
        private readonly TableroData _tableroData;
        public PartidaService()
        {
            _partidaData = new PartidasData();
            _partidaDetalleData = new PartidaDetalleData();
            _jugadorData = new JugadorData();
            _tableroData = new TableroData();
        }
        public ApiResponse<PartidaResponse> CrearPartida(CrearPartida request)
        {
            var response = new ApiResponse<PartidaResponse>();
            if (request.Dnis.Count < 2 || request.Dnis.Count > 4)
            {
                response.Success = false;
                response.Message = "La partida debe tener entre 2 y 4 jugadores";
                return response;
            }
            foreach (var dni in request.Dnis)
            {
                var jugador = _jugadorData.GetAll().FirstOrDefault(j => j.DNI.ToString() == dni);
                if (jugador == null)
                {
                    response.Success = false;
                    response.Message = $"El jugador con DNI {dni} no existe";
                    return response;
                }
            }

            var partida = new Partida
            {
                NumeroPartida = GenerarNumeroPartida(),
                FechaInicio = DateTime.Now,
                Estado = 0,
                TurnoActual = 1,
                ConfiguracionTurnos = new List<ConfiguracionTurno>(),
                Tablero = CargarTablero(),
                Jugadores = new List<JugadorEnPartida>()
            };

            for (int i = 0; i < request.Dnis.Count; i++)
            {
                partida.ConfiguracionTurnos.Add(new ConfiguracionTurno
                {
                    NumeroTurno = i + 1,
                    DniJugador = int.Parse(request.Dnis[i])
                });
            }

            foreach (var dni in request.Dnis)
            {
                partida.Jugadores.Add(new JugadorEnPartida
                {
                    NumeroPartida = partida.NumeroPartida,
                    DniJugador = int.Parse(dni),
                    PosicionActual = 0,
                    DineroDisponible = 5000000,
                    Estado = 0, // EnJuego
                    HistorialMovimientos = new List<Movimiento>()
                });
            }
            _partidaData.WritePartida(partida);

            response.Success = true;
            response.Message = "Partida creada exitosamente";
            response.Data = MapearPartida(partida);

            return response;
        }
        public ApiResponse<LanzarDadoResponse> LanzarDado(LanzarDado request)
        {
            var response = new ApiResponse<LanzarDadoResponse>();

            var partida = _partidaData.GetAll().FirstOrDefault(p => p.NumeroPartida == request.NumeroPartida);
            if (partida == null)
            {
                response.Success = false;
                response.Message = "Partida no encontrada";
                return response;
            }

            var jugador = partida.Jugadores.FirstOrDefault(j => j.DniJugador == request.DniJugador);
            if (jugador == null)
            {
                response.Success = false;
                response.Message = "Jugador no encontrado en la partida";
                return response;
            }

            Random random = new Random();
            int valorDado = random.Next(1, 7);

            int nuevaPosicion = jugador.PosicionActual + valorDado;
            if (nuevaPosicion > partida.Tablero.Count)
            {
                nuevaPosicion = nuevaPosicion - partida.Tablero.Count;
                jugador.DineroDisponible += 100000;
            }
            jugador.PosicionActual = nuevaPosicion;
            var casillero = partida.Tablero.FirstOrDefault(c => c.NroCasillero == nuevaPosicion);
            if (casillero != null)
            {
                AplicarReglasCasillero(partida, jugador, casillero);
            }
            _partidaData.WritePartida(partida);
            response.Success = true;
            response.Message = "Jugador movido exitosamente";
            response.Data = new LanzarDadoResponse
            {
                DniJugador = request.DniJugador,
                ValorDado = valorDado,
                PosicionNueva = nuevaPosicion,
                DineroDisponible = jugador.DineroDisponible
            };
            return response;
        }
        private void AplicarReglasCasillero(Partida partida, JugadorEnPartida jugador, CasilleroTablero casillero)
        {
            switch (casillero.TipoCasillero)
            {
                case 1:
                    if (casillero.DniPropietario == null)
                    {
                        if (jugador.DineroDisponible >= (double)casillero.PrecioCompra)
                        {
                            jugador.DineroDisponible -= (double)casillero.PrecioCompra;
                            casillero.DniPropietario = jugador.DniJugador.ToString();
                        }
                    }
                    else if (casillero.DniPropietario != jugador.DniJugador.ToString())
                    {
                        double alquiler = (double)casillero.PrecioAlquiler;
                        if (jugador.DineroDisponible >= alquiler)
                        {
                            jugador.DineroDisponible -= alquiler;
                            var propietario = partida.Jugadores.FirstOrDefault(j => j.DniJugador.ToString() == casillero.DniPropietario);
                            if (propietario != null)
                            {
                                propietario.DineroDisponible += alquiler;
                            }
                        }
                    }
                    break;
                case 2:
                    jugador.DineroDisponible -= (double)casillero.MontoSancion;
                    break;
                case 3:
                    jugador.DineroDisponible += (double)casillero.MontoSancion;
                    break;
            }
        }
        private int GenerarNumeroPartida()
        {
            var partidas = _partidaData.GetAll();
            return partidas.Count == 0 ? 1 : partidas.Max(p => p.NumeroPartida) + 1;
        }
        private List<CasilleroTablero> CargarTablero()
        {
            return new List<CasilleroTablero>();
        }
        private PartidaResponse MapearPartida(Partida partida)
        {
            return new PartidaResponse
            {
                NroPartida = partida.NumeroPartida,
                Estado = (EstadoPartida)partida.Estado,
                TurnoActual = partida.TurnoActual,
                DniJugadorTurno = partida.ConfiguracionTurnos.FirstOrDefault(t => t.NumeroTurno == partida.TurnoActual)?.DniJugador,
                DniGanador = partida.DniGanador,
                MotivoVictoria = partida.MotivoVictoria,
                Jugadores = partida.Jugadores.Select(j => new JugadorEnPartidaResponse
                {
                    DniJugador = j.DniJugador,
                    PosicionActual = j.PosicionActual,
                    DineroDisponible = j.DineroDisponible,
                    Estado = (EstadoJugador)j.Estado
                }).ToList()
            };
        }
    }
}