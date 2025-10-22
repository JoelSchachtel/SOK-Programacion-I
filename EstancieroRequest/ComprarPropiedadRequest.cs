using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroRequest
{
    public class ComprarPropiedadRequest
    {
        public int DniJugador { get; set; }
        public int PropiedadId { get; set; }
        public decimal Precio { get; set; }
        public bool PuedeComprar { get; set; }
    }
}
