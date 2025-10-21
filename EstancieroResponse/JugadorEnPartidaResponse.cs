using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroResponse
{
    public class JugadorEnPartidaResponse
    {
        public int DniJugador { get; set; }
        public int PosicionActual { get; set; }
        public double DineroDisponible { get; set; }
        public EstadoJugador Estado { get; set; }
    }
}
