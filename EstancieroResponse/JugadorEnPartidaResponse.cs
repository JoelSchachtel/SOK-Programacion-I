using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroResponse
{
    public class JugadorEnPartidaResponse
    {
        public string Dni { get; set; }
        public int Posicion { get; set; }
        public decimal Saldo { get; set; }
        public EstadoJugador Estado { get; set; }
        public int ProvinciasCompradas { get; set; }
    }
}
