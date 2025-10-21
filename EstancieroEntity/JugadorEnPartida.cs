using EstancieroEntity;
using System;
using System.Collections.Generic;

namespace EstancieroEntity
{
    public class JugadorEnPartida
    {
        public int NumeroPartida { get; set; }
        public int DniJugador { get; set; }
        public int PosicionActual { get; set; }       
        public double DineroDisponible { get; set; }   
        public int Estado { get; set; }                
        public List<Movimiento> HistorialMovimientos { get; set; } = new();
    }
}