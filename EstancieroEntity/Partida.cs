using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntities
{
    public class Partida
    {
        // Propiedades necesarias para la configuración del turno
        public int NumeroTurno { get; set; }
        public string DniJugador { get; set; }

        // Constructor
        public Partida(int numeroTurno, string dniJugador)
        {
            NumeroTurno = numeroTurno;
            DniJugador = dniJugador;
        }
    }
}
