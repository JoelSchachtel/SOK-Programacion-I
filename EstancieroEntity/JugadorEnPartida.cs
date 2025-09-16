using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntity
{
    public class JugadorEnPartida
    {
        public string NumeroPartida { get; set; }
        public string DniJugador { get; set; }
        public int Posicion { get; set; }
        public int Saldo { get; set; }
        public bool EstaEnJuego { get; set; }
        public int CantidadProvincias { get; set; }
    }
}
