using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntity
{
    public class JugadorEnPartida
    {
        public string Dni { get; set; } = string.Empty;
        public int Posicion { get; set; }
        public int Saldo { get; set; }
        public EstadoJugador Estado { get; set; }
        public List<MovimientoDeJugador> HistorialMovimientos { get; set; } = new();

        public JugadorEnPartida()
        {
            Estado = EstadoJugador.Activo;
        }
    }
}
