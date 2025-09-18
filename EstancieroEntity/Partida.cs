using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntity
{
    public class Partida
    {
        public int NumeroDePartida { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime? Fin { get; set; }
        public EstadoPartida EstadoPartida { get; set; }
        public int TurnoActual { get; set; }
        public List<Turno> Turnos { get; set; }
        public List<Casillero> Tablero { get; set; }
        public string? GanadorDni { get; set; }
        public string? MotivoVictoria { get; set; }

    }
}
