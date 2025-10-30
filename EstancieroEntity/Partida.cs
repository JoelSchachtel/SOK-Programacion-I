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
        public int Estado { get; set; }
        public int TurnoActual { get; set; }
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

        public void ActualizarJugador( JugadorEnPartida jugadorActualizado )
        {
            for ( int i = 0; i < Jugadores.Count; i++ )
            {
                if ( Jugadores[ i ].DniJugador == jugadorActualizado.DniJugador )
                {
                    Jugadores[ i ] = jugadorActualizado;
                    return;
                }
            }
        }

        public void ActualizarCasillero( CasilleroTablero casilleroActualizado )
        {
            for ( int i = 0; i < Tablero.Count; i++ )
            {
                if ( Tablero[ i ].NroCasillero == casilleroActualizado.NroCasillero )
                {
                    Tablero[ i ] = casilleroActualizado;
                    return;
                }
            }
        }
    }
}