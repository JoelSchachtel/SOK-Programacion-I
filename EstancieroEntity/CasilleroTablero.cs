using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntities
{
    public class CasilleroTablero
    {
        public int NroCasillero { get; set; }
        public int TipoCasillero { get; set; } // Despues queda mapeado en Response
        public string Nombre { get; set; }
        public decimal? PrecioCompra { get; set; }
        public decimal? PrecioAlquiler { get; set; }
        public decimal? MontoSancion { get; set; }
        public string? DniPropietario { get; set; }

        //Constructor
        
        public CasilleroTablero(int nroCasillero, int tipoCasillero, string nombre, decimal? precioCompra, decimal? precioAlquiler, decimal? montoSancion, string? dniPropietario)
        {
            NroCasillero = nroCasillero;
            TipoCasillero = tipoCasillero;
            Nombre = nombre;
            PrecioCompra = precioCompra;
            PrecioAlquiler = precioAlquiler;
            MontoSancion = montoSancion;
            DniPropietario = dniPropietario;
        }
    }
}
