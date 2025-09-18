using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntity
{
    public class Casillero
    {
        public int NroCasillero { get; set; }
        public TipoCasillero Tipo { get; set; }
        public string Nombre { get; set; }
        public int? PrecioCompra { get; set; }
        public int? PrecioAlquiler { get; set; }
        public int? Monto { get; set; }
        public string? DniPropietario { get; set; }

        public Casillero()
        {
            Nombre = string.Empty;
        }
    }
}
