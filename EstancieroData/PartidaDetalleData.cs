using Newtonsoft.Json;
using EstancieroEntity;

namespace EstancieroData
{
    public class PartidaDetalleData
    {
        private string CarpetaBase { get; }
        private string CarpetaDetalle => Path.Combine(CarpetaBase, "partidas_detalle");
        public PartidaDetalleData()
        {
            CarpetaBase = Path.GetFullPath(Path.Combine("../EstancieroData/Data"));
            Directory.CreateDirectory(CarpetaBase);
            Directory.CreateDirectory(CarpetaDetalle);
        }
        private string FileFor(int nro) => Path.Combine(CarpetaDetalle, $"{nro}.json");
        public List<JugadorEnPartida> GetDetalle(int nroPartida)
        {
            string f = FileFor(nroPartida);
            if (File.Exists(f))
            {
                var json = File.ReadAllText(f);
                return JsonConvert.DeserializeObject<List<JugadorEnPartida>>(json) ?? new List<JugadorEnPartida>();
            }
            return new List<JugadorEnPartida>();
        }
        public void WriteDetalle(int nroPartida, List<JugadorEnPartida> detalle)
        {
            Directory.CreateDirectory(CarpetaDetalle);
            File.WriteAllText(FileFor(nroPartida), JsonConvert.SerializeObject(detalle, Formatting.Indented));
        }
    }
}
