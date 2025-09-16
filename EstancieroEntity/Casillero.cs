using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntity
{
    public class Casillero
    {
        public int Numero { get; set; }
        //public TipoCasillero TipoCasillero { get; set; } Se saca según JSON
        public string? NombreProvincia { get; set; } //IDEM 
        public int? PrecioProvinciaCompra { get; set; } //IDEM
        public int? PrecioProvinciaAlquiler { get; set; } //IDEM
        public int? MontoMulta { get; set; } //IDEM
        public int? MontoPremio { get; set; } //IDEM
        public int DniPropietario { get; set; } //IDEM

    }
}
