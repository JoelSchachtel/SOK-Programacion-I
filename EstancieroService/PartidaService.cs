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

        // ==============================
        // 🔹 GESTIÓN DE LA PARTIDA
        // ==============================
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
            if (partida.TurnoActual == 1)
            {
                response.Success = true;
                response.Message = "Es el primer turno de la partida";
                response.Data = new TurnoActualResponse { NumeroPartida = partida.NumeroPartida , DniJugador = };
            }
        }
        public ApiResponse<LanzarDadoResponse> LanzarDado(LanzarDado request) { return null; }
        public ApiResponse<PartidaResponse> TerminarTurno(TerminarTurnoRequest request) { return null; }
        public ApiResponse<AccionResponse> ComprarPropiedad(ComprarPropiedadRequest request) { return null; }
        public ApiResponse<AccionResponse> PagarAlquiler(PagarAlquilerRequest request) { return null; }
        public ApiResponse<AccionResponse> AplicarCasillero(AplicarCasilleroRequest request) { return null; }
        public ApiResponse<List<JugadorEnPartidaResponse>> GetJugadores(int nroPartida) { return null; }
        public ApiResponse<List<CasilleroTableroResponse>> GetTablero(int nroPartida) { return null; }
        public ApiResponse<List<MovimientoResponse>> GetHistorialMovimientos(int nroPartida) { return null; }
        public ApiResponse<List<MovimientoResponse>> GetHistorialJugador(int nroPartida, int dniJugador) { return null; }
        private void Acreditar(JugadorEnPartida jugador, double monto, string concepto) { }
        private void Debitar(JugadorEnPartida jugador, double monto, string concepto) { }
        private void Transferir(JugadorEnPartida origen, JugadorEnPartida destino, double monto, string concepto) { }
        private void MarcarDerrotadoSiSaldoNoPositivo(JugadorEnPartida jugador) { }
        private void EvaluarGanadorYFinalizarSiCorresponde(Partida partida) { }
        private bool CalcularGanadorPor12Provincias(Partida partida) { return false; }
        private bool CalcularGanadorPorUnicoSaldoPositivo(Partida partida) { return false; }
        private bool CalcularGanadorPorMayorSaldo(Partida partida) { return false; }
        private void RegistrarMovimiento(Partida partida, Movimiento movimiento, IEnumerable<Transaccion> transacciones = null) { }
        private List<CasilleroTablero> CargarTableroDesdeConfig() { return null; }
        private CasilleroTablero ObtenerCasilleroActual(Partida partida, int dniJugador) { return null; }
        private void AplicarReglaDeCasillero(Partida partida, JugadorEnPartida jugador, CasilleroTablero casillero) { }
        private void ValidarPartidaEnJuego(Partida partida) { }
        private void ValidarEsTurnoDelJugador(Partida partida, int dniJugador) { }
        private void ValidarAccionHabilitada(Partida partida, JugadorEnPartida jugador) { }
        private void ValidarCompraUnicaPorTurno(Partida partida, int dniJugador) { }
        private void RotarTurno(Partida partida) { }
        private void ActualizarStatsUsuarios(Partida partida) { }












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