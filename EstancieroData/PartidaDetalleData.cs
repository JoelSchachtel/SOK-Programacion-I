using Newtonsoft.Json;
using EstancieroEntity;

namespace EstancieroData
{
    public class PartidaDetalleData
    {
       private string Direccion { get; set; }
        public PartidaDetalleData()
        {
            Direccion = Path.Combine(Directory.GetCurrentDirectory(), "Data", "partidas_detalle.json");
        }
        public List<Partida> GetAll()
        {
            if (File.Exists(Direccion))
            {
                string json = File.ReadAllText(Direccion);
                var partidas = JsonConvert.DeserializeObject<List<Partida>>(json);
                return partidas ?? new List<Partida>();
            }
            Directory.CreateDirectory(Path.GetDirectoryName("../EstancieroData/Data"));
            return new List<Partida>();
        }

        public void EscribirDetalle(List<Partida> partidas)
        {
            string json = JsonConvert.SerializeObject(partidas, Formatting.Indented);
            File.WriteAllText(Direccion, json);
        }

        public void EscribirDetalle(int numeroPartida, int dniJugador, Movimiento movimiento)
        {
            var partidas = GetAll();
            if (partidas == null || partidas.Count == 0) return;

            var partida = partidas.FirstOrDefault(p => p.NumeroPartida == numeroPartida);
            if (partida == null) return;

            var jugador = partida.Jugadores?.FirstOrDefault(j => j.DniJugador == dniJugador);
            if (jugador == null) return;

            jugador.HistorialMovimientos ??= new List<Movimiento>();
            if (movimiento.Id == 0)
            {
                movimiento.Id = (jugador.HistorialMovimientos.Count > 0)
                    ? jugador.HistorialMovimientos.Max(m => m.Id) + 1
                    : 1;
            }
            jugador.HistorialMovimientos.Add(movimiento);

            EscribirDetalle(partidas);
        }
    }
}
