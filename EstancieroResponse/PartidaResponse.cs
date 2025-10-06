using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroResponse
{
    public class PartidaResponse
    {
        public int NroPartida { get; set; }
        public EstadoPartida Estado { get; set; }
        public int TurnoActual { get; set; }
        public string DniJugadorTurno { get; set; }
        public string? DniGanador { get; set; }
        public string? MotivoVictoria { get; set; }
        public List<JugadorEnPartidaResponse> Jugadores { get; set; }
    }
}
