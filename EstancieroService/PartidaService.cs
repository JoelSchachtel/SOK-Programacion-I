using EstancieroData;
using EstancieroEntity;
using EstancieroRequest;
using EstancieroResponse;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System.Runtime.CompilerServices;
using System.Transactions;

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
            foreach (var dni in request.Dnis)
            {
                var jugador = _jugadorData.GetAll().FirstOrDefault(j => j.DNI == dni);
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
                TurnoActual = 0,
                ConfiguracionTurnos = new List<ConfiguracionTurno>(),
                Tablero = CargarTablero(),
                Jugadores = new List<JugadorEnPartida>()
            };

            foreach (var dni in request.Dnis)
            {
                partida.Jugadores.Add(new JugadorEnPartida
                {
                    NumeroPartida = partida.NumeroPartida,
                    DniJugador = dni,
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
        public ApiResponse<PartidaResponse> BuscarPartidaId(BuscarPartida request)
        {
            var response = new ApiResponse<PartidaResponse>();
            var partida = _partidaData.GetAll().FirstOrDefault(p => p.NumeroPartida == request.NumeroPartida);
            if (partida == null)
            {
                response.Success = false;
                response.Message = "Partida no encontrada";
                return response;
            }
            response.Success = true;
            response.Message = "Partida encontrada exitosamente";
            response.Data = MapearPartida(partida);
            return response;
        }
        public ApiResponse<PartidaResponse> PausarPartida(CambiarEstadoPartida request)
        {
            var response = new ApiResponse<PartidaResponse>();
            var partida = _partidaData.GetAll().FirstOrDefault(p => p.NumeroPartida == request.NumeroPartida);
            if (partida == null)
            {
                response.Success = false;
                response.Message = "Partida no encontrada";
                return response;
            }
            if (partida.Estado == (int)EstadoPartida.EnJuego)
            {
                partida.Estado = (int)EstadoPartida.Pausada;
                _partidaData.WritePartida(partida);
                response.Success = true;
                response.Message = "Partida pausada exitosamente";
                return response;
            }
            response.Success = false;
            response.Message = "No se puede pausar la partida en su estado actual";
            return response;

            //Falta configurar función de suspender el dado
        }
        public ApiResponse<PartidaResponse> ReanudarPartida(CambiarEstadoPartida request)
        {
            var response = new ApiResponse<PartidaResponse>();
            var partida = _partidaData.GetAll().FirstOrDefault(p => p.NumeroPartida == request.NumeroPartida);
            if (partida == null)
            {
                response.Success = false;
                response.Message = "Partida no encontrada";
                return response;
            }
            if (partida.Estado == (int)EstadoPartida.Pausada)
            {
                partida.Estado = (int)EstadoPartida.EnJuego;
                _partidaData.WritePartida(partida);
                response.Success = true;
                response.Message = "Partida reanudada exitosamente";
                response.Data = MapearPartida(partida);
                return response;
            }
            response.Success = false;
            response.Message = "No se puede reanudar la partida en su estado actual";
            return response;

            //Falta función para habilitar el dado
            //Falta verificar si esta suspsendida, no se puede reanudar
        }
        public ApiResponse<PartidaResponse> SuspenderPartida(CambiarEstadoPartida request)
        {
            var response = new ApiResponse<PartidaResponse>();
            var partida = _partidaData.GetAll().FirstOrDefault(p => p.NumeroPartida == request.NumeroPartida);
            if (partida == null)
            {
                response.Success = false;
                response.Message = "Partida no encontrada";
                return response;
            }
            partida.Estado = (int)EstadoPartida.Suspendida;
            _partidaData.WritePartida(partida);
            response.Success = true;
            response.Message = "Partida suspendida exitosamente";
            response.Data = MapearPartida(partida);
            return response;

            //Falta función para deshabilitar funciones de la partida
            // Falta función para devolver ganadores hasta el momento
        }
        public ApiResponse<TurnoActualResponse> ConsultarTurnoActual(int nroPartida)
        {
            ApiResponse<TurnoActualResponse> response = new ApiResponse<TurnoActualResponse>();
            Partida partida = _partidaData.GetAll().FirstOrDefault(p => p.NumeroPartida == nroPartida);
            if (partida == null)
            {
                response.Success = false;
                response.Message = "Partida no encontrada";
                return response;
            }
            int? dniTurnoConfig = partida.ConfiguracionTurnos?.FirstOrDefault(t => t.NumeroTurno == partida.TurnoActual)?.DniJugador;

            int jugadorIndex;
            int dniTurno;

            if (dniTurnoConfig.HasValue)
            {
                dniTurno = dniTurnoConfig.Value;
                jugadorIndex = partida.Jugadores.FindIndex(j => j.DniJugador == dniTurno);
                if (jugadorIndex < 0)
                {
                    // Fallback si el DNI configurado no está en la lista
                    jugadorIndex = 0;
                    dniTurno = partida.Jugadores[jugadorIndex].DniJugador;
                }
            }
            else
            {
                // Alternancia por paridad: 1->jugador 1 (índice 0), 2->jugador 2 (índice 1), etc.
                jugadorIndex = (partida.TurnoActual % 2 == 1) ? 0 : 1;
                dniTurno = partida.Jugadores[jugadorIndex].DniJugador;
            }

            response.Success = true;
            response.Message = $"Le toca al jugador {jugadorIndex + 1}";
            response.Data = new TurnoActualResponse
            {
                NumeroPartida = partida.NumeroPartida,
                DniJugador = dniTurno.ToString()
            };

            return response;
        }
        public ApiResponse<LanzarDadoResponse> LanzarDado(LanzarDado request)
        {
            var response = new ApiResponse<LanzarDadoResponse>();
            try
            {
                var partida = _partidaData.GetAll().FirstOrDefault(p => p.NumeroPartida == request.NumeroPartida);
                if (partida == null)
                {
                    response.Success = false;
                    response.Message = "Partida no encontrada";
                    return response;
                }
                if (partida.Estado != (int)EstadoPartida.EnJuego)
                {
                    response.Success = false;
                    response.Message = "La partida no está en juego, el ganador fue " + partida.DniGanador;
                    return response;
                }
                ValidarPartidaEnJuego(partida);
                var jugador = partida.Jugadores.FirstOrDefault(j => j.DniJugador == request.DniJugador);
                if (jugador == null)
                {
                    response.Success = false;
                    response.Message = "Jugador no encontrado en la partida";
                    return response;
                }

                if (!ValidarEsTurnoDelJugador(partida, request.DniJugador))
                {
                    response.Success = false;
                    response.Message = "No es el turno del jugador";
                    return response;
                }

                int valorDado = Random.Shared.Next(1, 7);
                var nuevoCasillero = ( jugador.PosicionActual + valorDado ) % partida.Tablero.Count;

                jugador.PosicionActual = nuevoCasillero;

                var casillero = partida.Tablero[nuevoCasillero];

                if ( casillero.Tipo == TipoCasillero.Multa.ToString( ).ToLower() )
                {
                    jugador.DineroDisponible -= ( double ) casillero.Monto;

                    jugador.HistorialMovimientos.Add( new Movimiento
                    {
                        Fecha = DateTime.Now,
                        Tipo = "Pago",
                        Monto = casillero.Monto,
                        Casillero = nuevoCasillero
                    } );
                }

                else if ( casillero.Tipo == TipoCasillero.Premio.ToString().ToLower( ) )
                {
                    jugador.DineroDisponible += ( double ) casillero.Monto;

                    jugador.HistorialMovimientos.Add( new Movimiento
                    {
                        Fecha = DateTime.Now,
                        Tipo = "Cobro",
                        Monto = casillero.Monto,
                        Casillero = nuevoCasillero
                    } );
                }
                else
                {
                    if ( casillero.DniPropietario != null )
                    {
                        var propietario = partida.Jugadores.FirstOrDefault( j => j.DniJugador.ToString( ) == casillero.DniPropietario );

                        if ( propietario != null && propietario.DniJugador != jugador.DniJugador )
                        {
                            jugador.DineroDisponible -= ( double ) casillero.PrecioAlquiler;
                            propietario.DineroDisponible += ( double ) casillero.PrecioAlquiler;

                            jugador.HistorialMovimientos.Add( new Movimiento
                            {
                                Fecha = DateTime.Now,
                                Tipo = "Pago",
                                Monto = casillero.PrecioAlquiler,
                                Casillero = nuevoCasillero
                            } );

                            propietario.HistorialMovimientos.Add( new Movimiento
                            {
                                Fecha = DateTime.Now,
                                Tipo = "Cobro",
                                Monto = casillero.PrecioAlquiler,
                                Casillero = nuevoCasillero
                            } );

                            partida.ActualizarJugador( propietario );
                        }
                    }
                    else
                    {
                        casillero.DniPropietario = jugador.DniJugador.ToString( );
                        jugador.DineroDisponible -= ( double ) casillero.PrecioCompra;

                        jugador.HistorialMovimientos.Add( new Movimiento
                        {
                            Fecha = DateTime.Now,
                            Tipo = "Compra",
                            Monto = casillero.PrecioCompra,
                            Casillero = nuevoCasillero
                        } );

                        partida.ActualizarCasillero( casillero );
                    }
                }

                if ( jugador.DineroDisponible < 0 )
                {
                    jugador.Estado = ( int ) EstadoJugador.Derrotado;
                    partida.DniGanador = partida.Jugadores.First( j => j.DniJugador != jugador.DniJugador ).DniJugador;
                    partida.MotivoVictoria = "Ganó por ser el único jugador con saldo positivo";
                    partida.Estado = ( int ) EstadoPartida.Finalizada;
                }
                else if ( partida.Tablero.Where( x => x.DniPropietario == jugador.DniJugador.ToString() ).Count() >= 12 )
                {
                    partida.DniGanador = jugador.DniJugador;
                    partida.MotivoVictoria = "Ganó por obtener 12 provincias";
                    partida.Estado = ( int ) EstadoPartida.Finalizada;
                }

                partida.ActualizarJugador( jugador );
                partida.TurnoActual = (partida.TurnoActual + 1) % 2;
                
                _partidaData.WritePartida(partida);

                response.Success = true;
                response.Message = "Dado lanzado exitosamente";
                response.Data = new LanzarDadoResponse
                {
                    DniJugador = jugador.DniJugador,
                    ValorDado = valorDado,
                    PosicionNueva = jugador.PosicionActual,
                    DineroDisponible = jugador.DineroDisponible
                };
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al lanzar el dado: {ex.Message}";
                return response;
            }
        }

        public ApiResponse<List<JugadorEnPartidaResponse>> ObtenerJugadores(int id)
        {
            var response = new ApiResponse<List<JugadorEnPartidaResponse>>();
            var partida = _partidaData.GetAll().FirstOrDefault(p => p.NumeroPartida == id);

            if (partida == null)
            {
                response.Success = false;
                response.Message = "Partida no encontrada";
                return response;
            }

            var jugadores = partida.Jugadores.Select( x => new JugadorEnPartidaResponse()
            {
                DineroDisponible = x.DineroDisponible,
                PosicionActual = x.PosicionActual,
                DniJugador = x.DniJugador,
            } ).ToList();

            response.Data = jugadores;
            response.Success = true;

            return response;
        }

        public ApiResponse<PartidaResponse> TerminarTurno(TerminarTurnoRequest request)
        {
            var response = new ApiResponse<PartidaResponse>();
            var partida = _partidaData.GetAll().FirstOrDefault(p => p.NumeroPartida == request.NumeroPartida);

            if (partida == null)
            {
                response.Success = false;
                response.Message = "Partida no encontrada";
                return response;
            }

            if (partida.Estado != (int)EstadoPartida.EnJuego)
            {
                response.Success = false;
                response.Message = "La partida no está en juego";
                return response;
            }

            var jugador = partida.Jugadores.FirstOrDefault(j => j.DniJugador.ToString() == request.DniJugador);
            if (jugador == null)
            {
                response.Success = false;
                response.Message = "Jugador no encontrado en la partida";
                return response;
            }

            ValidarEsTurnoDelJugador(partida, jugador.DniJugador);

            // Avanzar al siguiente turno
            partida.TurnoActual++;
            if (partida.TurnoActual > partida.Jugadores.Count)
            {
                partida.TurnoActual = 1;
            }

            EvaluarGanadorYFinalizarSiCorresponde(partida);

            _partidaData.WritePartida(partida);

            response.Success = true;
            response.Message = "Turno finalizado exitosamente";
            response.Data = MapearPartida(partida);

            return response;
        }
        private void EvaluarGanadorYFinalizarSiCorresponde(Partida partida)
        {

            if (CalcularGanadorPor12Provincias(partida))
                return;

            if (CalcularGanadorPorUnicoSaldoPositivo(partida))
                return;
        }
        private bool CalcularGanadorPor12Provincias(Partida partida)
        {
            foreach (var jugador in partida.Jugadores)
            {
                if (jugador.Estado != (int)EstadoJugador.Derrotado)
                {
                    int cantidadPropiedades = partida.Tablero
                        .Count(c => c.DniPropietario != null
                                    && int.TryParse(c.DniPropietario, out var dniProp)
                                    && dniProp == jugador.DniJugador);

                    if (cantidadPropiedades >= 12)
                    {
                        partida.DniGanador = jugador.DniJugador;
                        partida.MotivoVictoria = "Ganó por obtener 12 provincias";
                        partida.Estado = (int)EstadoPartida.Finalizada;
                        partida.FechaFin = DateTime.Now;
                        return true;
                    }
                }
            }
            return false;
        }
        private bool CalcularGanadorPorUnicoSaldoPositivo(Partida partida)
        {
     
            var jugadoresActivos = partida.Jugadores
                .Where(j => j.Estado != (int)EstadoJugador.Derrotado && j.DineroDisponible > 0)
                .ToList();

            if (jugadoresActivos.Count == 1)
            {
                var ganador = jugadoresActivos.First();
                partida.DniGanador = ganador.DniJugador;
                partida.MotivoVictoria = "Ganó por ser el único jugador con saldo positivo";
                partida.Estado = (int)EstadoPartida.Finalizada;
                partida.FechaFin = DateTime.Now;
                return true;
            }
            return false;
        }
        private void ValidarPartidaEnJuego(Partida partida)
        {
            if (partida.Estado != (int)EstadoPartida.EnJuego)
            {
                throw new InvalidOperationException("La partida no está en juego.");
            }
        }
        private bool ValidarEsTurnoDelJugador(Partida partida, int dniJugador)
        {
            return partida.Jugadores[partida.TurnoActual].DniJugador == dniJugador;
        }
       
        private int GenerarNumeroPartida()
        {
            var partidas = _partidaData.GetAll();
            return partidas.Count == 0 ? 1 : partidas.Max(p => p.NumeroPartida) + 1;
        }
        private List<CasilleroTablero> CargarTablero()
        {
            return _tableroData.GetTablero();
        }
        private PartidaResponse MapearPartida(Partida partida)
        {
            return new PartidaResponse
            {
                NumeroPartida = partida.NumeroPartida,
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