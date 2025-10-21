using Newtonsoft.Json;
using EstancieroEntity;

namespace EstancieroData
{
    public class TableroData
    {
        private string Carpeta { get; }
        private string Archivo { get; }
        public TableroData()
        {
            Carpeta = Path.GetFullPath(Path.Combine("../EstancieroData/Data"));
            Archivo = Path.Combine(Carpeta, "tablero.json");
        }
        public List<CasilleroTablero> GetTablero()
        {
            if (File.Exists(Archivo))
            {
                var json = File.ReadAllText(Archivo);
                var cfg = JsonConvert.DeserializeObject<TableroConfig>(json);
                return (cfg?.Tablero) ?? new List<CasilleroTablero>();
            }
            Directory.CreateDirectory(Carpeta);
            return new List<CasilleroTablero>();
        }
        public void SaveTablero(List<CasilleroTablero> tablero)
        {
            var cfg = new TableroConfig { Tablero = tablero };
            Directory.CreateDirectory(Carpeta);
            File.WriteAllText(Archivo, JsonConvert.SerializeObject(cfg, Formatting.Indented));
        }
    }
    public class TableroConfig
    {
        public List<CasilleroTablero> Tablero { get; set; } = new();
    }
}
