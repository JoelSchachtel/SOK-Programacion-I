namespace EstancieroResponse
{
    public class PartidaResponse
    {
        public int NumeroPartida { get; set; }
        public EstadoPartida Estado { get; set; }
        public int TurnoActual { get; set; }
        public int? DniJugadorTurno { get; set; }
        public int? DniGanador { get; set; }
        public string? MotivoVictoria { get; set; }
        public List<JugadorEnPartidaResponse> Jugadores { get; set; } = new();
    }
}