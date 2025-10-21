using EstancieroEntity;
using System;
using System.Collections.Generic;

namespace EstancieroEntity
{
    public class JugadorEnPartida
    {
        public int NumeroPartida { get; set; }
        public int DniJugador { get; set; }
        public int PosicionActual { get; set; }        // 0..30
        public double DineroDisponible { get; set; }   // 5.000.000 inicial
        public int Estado { get; set; }                // 0-EnJuego, 1-Derrotado
        public List<Movimiento> HistorialMovimientos { get; set; } = new();
    }
}