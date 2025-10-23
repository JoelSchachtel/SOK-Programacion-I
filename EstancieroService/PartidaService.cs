using EstancieroData;
using EstancieroEntity;
using EstancieroRequest;
using EstancieroResponse;
using Newtonsoft.Json;
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
                    response.Message = "La partida no está en juego";
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
                ValidarEsTurnoDelJugador(partida, request.DniJugador);
                int valorDado = Random.Shared.Next(1, 7);
                int posOrigen = jugador.PosicionActual;
                int posDestino = (jugador.PosicionActual + valorDado);
                jugador.PosicionActual = posDestino % partida.Tablero.Count;
                jugador.HistorialMovimientos ??= new List<Movimiento>();
                var movimiento = new Movimiento
                {
                    Id = (jugador.HistorialMovimientos.Count > 0) ? jugador.HistorialMovimientos.Max(m => m.Id) + 1 : 1,
                    Fecha = DateTime.Now,
                    Descripcion = $"Lanzó el dado y avanzó de {posOrigen} a {jugador.PosicionActual}",
                };
                jugador.HistorialMovimientos.Add(movimiento);

                var casillero = ObtenerCasilleroActual(partida, request.DniJugador) ?? partida.Tablero[jugador.PosicionActual];

                if (casillero != null)
                {
                    AplicarReglaDeCasillero(partida, jugador, casillero);
                }

                if (casillero.DniPropietario != null && casillero.DniPropietario != jugador.DniJugador.ToString())
                {
                    var propietario = partida.Jugadores.FirstOrDefault(j => j.DniJugador.ToString() == casillero.DniPropietario);
                    if (propietario != null && casillero.PrecioAlquiler.HasValue)
                    {
                        double montoAlquiler = casillero.PrecioAlquiler.Value;
                        Debitar(jugador, montoAlquiler, $"Pago de alquiler a {propietario.DniJugador} por {casillero.Nombre}");
                        Acreditar(propietario, montoAlquiler, $"Recibió alquiler de {jugador.DniJugador} por {casillero.Nombre}");
                        MarcarDerrotadoSiSaldoNoPositivo(jugador, partida);
                    }
                }

                MarcarDerrotadoSiSaldoNoPositivo(jugador, partida);

                _partidaDetalleData.EscribirDetalle(partida.NumeroPartida, jugador.DniJugador, movimiento);
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
        public ApiResponse<AccionResponse> ComprarPropiedad(ComprarPropiedadRequest request)
        {
            var response = new ApiResponse<AccionResponse>();
            var partida = _partidaData.GetAll().FirstOrDefault(p => p.NumeroPartida == request.PropiedadId);

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
            var casillero = partida.Tablero.FirstOrDefault(c => c.NroCasillero == jugador.PosicionActual);
            if (casillero == null || casillero.PrecioCompra == null)
            {
                response.Success = false;
                response.Message = "No hay propiedad para comprar en este casillero";
                return response;
            }
            if (casillero.DniPropietario != null)
            {
                response.Success = false;
                response.Message = "La propiedad ya tiene un propietario";
                return response;
            }
            if (jugador.DineroDisponible < casillero.PrecioCompra)
            {
                response.Success = false;
                response.Message = "El jugador no tiene suficiente dinero para comprar esta propiedad";
                return response;
            }
            jugador.DineroDisponible -= casillero.PrecioCompra.Value;
            casillero.DniPropietario = jugador.DniJugador.ToString();
            jugador.HistorialMovimientos.Add(new Movimiento
            {
                Fecha = DateTime.Now,
                Tipo = 1, // Tipo personalizado para compra de propiedad
                Descripcion = $"Compró la propiedad {casillero.Nombre}",
                Monto = -casillero.PrecioCompra,
                CasilleroOrigen = jugador.PosicionActual,
                CasilleroDestino = jugador.PosicionActual,
                DniJugadorAfectado = jugador.DniJugador
            });
            _partidaData.WritePartida(partida);
            response.Success = true;
            response.Message = "Propiedad comprada exitosamente";
            response.Data = new AccionResponse
            {
                Id = casillero.NroCasillero,
                Nombre = casillero.Nombre,
                Descripcion = "Compra realizada con éxito"
            };

            return response;
        }
        private void Acreditar(JugadorEnPartida jugador, double monto, string concepto) 
        {
            jugador.DineroDisponible += monto;
            jugador.HistorialMovimientos.Add(new Movimiento
            {
                Fecha = DateTime.Now,
                Tipo = 1, // Tipo personalizado para crédito
                Descripcion = concepto,
                Monto = monto,
                CasilleroOrigen = jugador.PosicionActual,
                CasilleroDestino = jugador.PosicionActual,
                DniJugadorAfectado = jugador.DniJugador
            });
        }
        private void Debitar(JugadorEnPartida jugador, double monto, string concepto)
        {
            jugador.DineroDisponible -= monto;
            jugador.HistorialMovimientos.Add(new Movimiento
            {
                Fecha = DateTime.Now,
                Tipo = 2, // Tipo personalizado para débito
                Descripcion = concepto,
                Monto = -monto,
                CasilleroOrigen = jugador.PosicionActual,
                CasilleroDestino = jugador.PosicionActual,
                DniJugadorAfectado = jugador.DniJugador
            });
        }
        private void MarcarDerrotadoSiSaldoNoPositivo(JugadorEnPartida jugador, Partida partida)
        {
            
            if (jugador.DineroDisponible <= 0 && jugador.Estado != (int)EstadoJugador.Derrotado)
            {
              
                var propiedades = partida.Tablero
                    .Where(c => c.DniPropietario != null && int.TryParse(c.DniPropietario, out var dni) && dni == jugador.DniJugador)
                    .ToList();

                if (propiedades.Any())
                {
                    // aca es donde les decia si quieren hacer lo de que venda las propiedades automaticamente o las elija o no se como quieren hacer
                    foreach (var propiedad in propiedades.OrderBy(p => p.PrecioCompra ?? 0))
                    {
                        if (jugador.DineroDisponible > 0) break;
                        double montoVenta = propiedad.PrecioCompra ?? 0;
                        jugador.DineroDisponible += montoVenta;
                        propiedad.DniPropietario = null; 
                        jugador.HistorialMovimientos.Add(new Movimiento
                        {
                            Fecha = DateTime.Now,
                            Tipo = 100, // Tipo personalizado para venta de propiedad
                            Descripcion = $"Venta automática de propiedad {propiedad.Nombre}",
                            Monto = montoVenta,
                            CasilleroOrigen = propiedad.NroCasillero,
                            CasilleroDestino = propiedad.NroCasillero,
                            DniJugadorAfectado = jugador.DniJugador
                        });
                    }
                }

               
                if (jugador.DineroDisponible <= 0)
                {
                    jugador.Estado = (int)EstadoJugador.Derrotado;
                    jugador.HistorialMovimientos.Add(new Movimiento
                    {
                        Fecha = DateTime.Now,
                        Tipo = 99, 
                        Descripcion = "Jugador derrotado por saldo no positivo",
                        Monto = jugador.DineroDisponible,
                        CasilleroOrigen = jugador.PosicionActual,
                        CasilleroDestino = jugador.PosicionActual,
                        DniJugadorAfectado = jugador.DniJugador
                    });
                }
            }
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
        private bool CalcularGanadorPorMayorSaldo(Partida partida)
        {
           
            var jugadoresConSaldo = partida.Jugadores
                .Where(j => j.Estado != (int)EstadoJugador.Derrotado)
                .Select(j => new{Jugador = j, SaldoTotal = j.DineroDisponible + partida.Tablero.Where(c => c.DniPropietario != null
                                        && int.TryParse(c.DniPropietario, out var dniProp)
                                        && dniProp == j.DniJugador
                                        && c.PrecioCompra.HasValue).Sum(c => c.PrecioCompra.Value)}).ToList();

            
            if (!jugadoresConSaldo.Any())
                return false;

            
            var maxSaldo = jugadoresConSaldo.Max(j => j.SaldoTotal);
            var ganadores = jugadoresConSaldo.Where(j => j.SaldoTotal == maxSaldo).ToList();

            
            if (ganadores.Count == 1)
            {
                var ganador = ganadores.First().Jugador;
                partida.DniGanador = ganador.DniJugador;
                partida.MotivoVictoria = "Ganó por tener el mayor saldo sumando dinero y propiedades";
                partida.Estado = (int)EstadoPartida.Finalizada;
                partida.FechaFin = DateTime.Now;
                return true;
            }
            return false;
        }
        private CasilleroTablero ObtenerCasilleroActual(Partida partida, int dniJugador)
        {
            var jugador = partida.Jugadores.FirstOrDefault(j => j.DniJugador == dniJugador);
            if (jugador == null)
            {
                return null;
            }
            int posicionActual = jugador.PosicionActual;
            if (posicionActual < 0 || posicionActual >= partida.Tablero.Count)
            {
                return null;
            }
            return partida.Tablero[posicionActual];
        }
        private void AplicarReglaDeCasillero(Partida partida, JugadorEnPartida jugador, CasilleroTablero casillero)
        {

        }
        private void ValidarPartidaEnJuego(Partida partida)
        {
            if (partida.Estado != (int)EstadoPartida.EnJuego)
            {
                throw new InvalidOperationException("La partida no está en juego.");
            }
        }
        private void ValidarEsTurnoDelJugador(Partida partida, int dniJugador)
        {
            int? dniTurnoConfig = partida.ConfiguracionTurnos?.FirstOrDefault(t => t.NumeroTurno == partida.TurnoActual)?.DniJugador;
            int jugadorIndex;
            int dniTurno;
            if (dniTurnoConfig.HasValue)
            {
                dniTurno = dniTurnoConfig.Value;
                jugadorIndex = partida.Jugadores.FindIndex(j => j.DniJugador == dniTurno);
                if (jugadorIndex < 0)
                {
                    jugadorIndex = 0;
                    dniTurno = partida.Jugadores[jugadorIndex].DniJugador;
                }
            }
            else
            {
                jugadorIndex = (partida.TurnoActual % 2 == 1) ? 0 : 1;
                dniTurno = partida.Jugadores[jugadorIndex].DniJugador;
            }
            if (dniJugador != dniTurno)
            {
                throw new InvalidOperationException("No es el turno del jugador.");
            }
        }
        private void ActualizarStatsUsuarios(Partida partida)
        {

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