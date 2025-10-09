using EstancieroData;
using EstancieroEntities;
using EstancieroEntity;

namespace EstancieroService
{
    public class PartidaService
    {
        private readonly PartidaData _partidaData;
        private readonly PartidaDetalleData _partidaDetalleData;
        private readonly JugadorData _jugadorData;
        //private readonly TableroData _tableroData;

        public PartidaService()
        {
            _partidaData = new PartidaData();
            _partidaDetalleData = new PartidaDetalleData();
            _jugadorData = new JugadorData();
            //_tableroData = new TableroData();
        }

        public Partida IniciarPartidaNueva(List<Jugador> jugadores)
        {
            // Si el DNI de algun jugador no existe, lanza una excepcion
            foreach (var jugador in jugadores)
            {
                var jugadorExistente = _jugadorData.GetAll().FirstOrDefault(j => j.DNI == jugador.DNI);
                if (jugadorExistente == null)
                {
                    throw new Exception($"El jugador con DNI {jugador.DNI} no existe.");
                }

                var partidas = _partidaData.GetAll();
                int nro = partidas.Count == 0 ? 1 : partidas.Max(p => p.NumeroTurno) + 1;


                var p = new Partida
                {
                    NumeroTurno = nro,
                    DniJugador = jugador.DNI,
                    Estado = "En Curso",
                    FechaInicio = DateTime.Now,
                    FechaFin = null,
                    Saldo = 1500, // Saldo inicial
                    Posicion = 0 // Posicion inicial en el tablero
                };



            }
        }
    }
}
