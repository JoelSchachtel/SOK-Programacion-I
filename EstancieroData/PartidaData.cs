using Newtonsoft.Json;
using EstancieroEntity;

namespace EstancieroData
{
    public class PartidasData
    {
        private string Carpeta { get; }
        private string Archivo { get; }

        public PartidasData()
        {
            Carpeta = Path.GetFullPath(Path.Combine("../EstancieroData/Data"));
            Archivo = Path.Combine(Carpeta, "partidas.json");
        }
        public List<Partida> GetAll()
        {
            if (File.Exists(Archivo))
            {
                var json = File.ReadAllText(Archivo);
                return JsonConvert.DeserializeObject<List<Partida>>(json) ?? new List<Partida>();
            }
            Directory.CreateDirectory(Carpeta);
            return new List<Partida>();
        }
        public Partida WritePartida(Partida p)
        {
            var lista = GetAll();
            int i = lista.FindIndex(x => x.NumeroPartida == p.NumeroPartida);
            if (i >= 0) lista[i] = p; else lista.Add(p);
            File.WriteAllText(Archivo, JsonConvert.SerializeObject(lista, Formatting.Indented));
            return p;
        }
        public void WriteAll(List<Partida> partidas)
        {
            Directory.CreateDirectory(Carpeta);
            File.WriteAllText(Archivo, JsonConvert.SerializeObject(partidas, Formatting.Indented));
        }
    }
}
