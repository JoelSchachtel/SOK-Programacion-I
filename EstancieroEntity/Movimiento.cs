using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntities
{
    public class Movimiento
    {
        //Propiedades necesarias para gestionar los movimientos del jugador en el tablero
        public DateTime FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public int DesdeCasillero { get; set; }
        public int HaciaCasillero { get; set; }
        public string? DNIJugadorOrigen { get; set; }
        public string? DNIJugadorDestino { get; set; }
        public decimal MontoTransaccion { get; set; }
        public string? Detalle { get; set; }

        // Constructor
        public Movimiento(DateTime fechaMovimiento, string tipoMovimiento, int desdeCasillero, int haciaCasillero, string? dniJugadorOrigen, string? dniJugadorDestino, decimal montoTransaccion, string? detalle)
        {
            FechaMovimiento = fechaMovimiento;
            TipoMovimiento = tipoMovimiento;
            DesdeCasillero = desdeCasillero;
            HaciaCasillero = haciaCasillero;
            DNIJugadorOrigen = dniJugadorOrigen;
            DNIJugadorDestino = dniJugadorDestino;
            MontoTransaccion = montoTransaccion;
            Detalle = detalle;
        }
    }
}
