using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntity
{
    public class Partida
    {
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public EstadoPartida Estado { get; set; }
        public int? GanadorDni { get; set; }
        public MotivoVictoria? MotivoVictoria { get; set; }

    }
}
