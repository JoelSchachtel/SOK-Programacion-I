using EstancieroEntities;
using Newtonsoft.Json;

namespace EstancieroData
{
    public class PartidaData
    {
        private string Carpeta { get; set; }
        private string Archivo { get; set; }
        public PartidaData()
        {
            Carpeta = Path.GetFullPath(Path.Combine("../EstancieroData/Data"));
            Archivo = Path.Combine(Carpeta, "partidas.json");
        }
        public List<Partida> GetAll()
        {
            if (File.Exists(Archivo))
            {
                string json = File.ReadAllText(Archivo);
                var partidas = JsonConvert.DeserializeObject<List<Partida>>(json);
                return partidas ?? new List<Partida>();
            }
            Directory.CreateDirectory(Carpeta);
            return new List<Partida>();
        }
        public Partida WritePartida(Partida partida)
        {
            List<Partida> partidas = GetAll();
            int index = partidas.FindIndex(p => p.NumeroTurno == partida.NumeroTurno && p.DniJugador == partida.DniJugador);
            if (index >= 0)
            {
                partidas[index] = partida;
            }
            else
            {
                partidas.Add(partida);
            }
            string json = JsonConvert.SerializeObject(partidas, Formatting.Indented);
            File.WriteAllText(Archivo, json);
            return partida;
        }
    }
}
