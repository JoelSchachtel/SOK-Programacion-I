using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntities
{
    public class JugadorEnPartida
    {
        public int IdPartida { get; set; }
        public string DniJugador { get; set; }
        public int Posicion { get; set; }
        public decimal SaldoDisponible { get; set; }
        public int EstadoJugador { get; set; } // Despues queda mapeado en Response
        public List<Movimiento> Movimientos { get; set; }
        public List<int> ProvinciasCompradas { get; set; }
    }   
}
