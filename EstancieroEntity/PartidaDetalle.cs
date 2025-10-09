using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntities
{
    public class PartidaDetalle
    {
        public int IdPartida { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public int EstadoPartida { get; set; }
        public int TurnoActual { get; set; }
        public List<Partida> ConfiguracionesTurnos { get; set; }
        public string? DniGanador { get; set; }
        public string? MotivoVictoria { get; set; }

        public PartidaDetalle(int idPartida, DateTime fechaInicio, int estadoPartida)
        {
            IdPartida = idPartida;
            FechaInicio = fechaInicio;
            EstadoPartida = estadoPartida;
            TurnoActual = 0;
            ConfiguracionesTurnos = new List<Partida>();
            DniGanador = null;
            MotivoVictoria = null;
        }
    }
}
