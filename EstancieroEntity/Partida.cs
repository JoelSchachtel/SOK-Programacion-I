using EstancieroEntity;
using System;
using System.Collections.Generic;

namespace EstancieroEntity
{
    public class Partida
    {
        public int NumeroPartida { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public TimeSpan? Duracion { get; set; }
        public int Estado { get; set; }// 0-EnJuego, 1-Pausada, 2-Suspendida, 3-Finalizada
        public int TurnoActual { get; set; } // 1 o 2 (dos jugadores)
        public List<ConfiguracionTurno> ConfiguracionTurnos { get; set; }
        public List<CasilleroTablero> Tablero { get; set; }
        public List<JugadorEnPartida> Jugadores { get; set; }
        public int? DniGanador { get; set; }
        public string? MotivoVictoria { get; set; }

        public Partida()
        {
            ConfiguracionTurnos = new List<ConfiguracionTurno>();
            Tablero = new List<CasilleroTablero>();
            Jugadores = new List<JugadorEnPartida>();
        }
    }
}