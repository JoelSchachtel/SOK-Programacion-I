using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EstancieroEntities;
namespace EstancieroData
{
    public class PartidaDetalleData
    {
        private string Carpeta { get; set; }
        private string Archivo { get; set; }
        public PartidaDetalleData()
        {
            Carpeta = Path.GetFullPath(Path.Combine("../EstancieroData/Data"));
            Archivo = Path.Combine(Carpeta, "partidas_detalle.json");
        }
        public List<PartidaDetalle> GetAll()
        {
            if (File.Exists(Archivo))
            {
                string json = File.ReadAllText(Archivo);
                var partidasDetalle = JsonConvert.DeserializeObject<List<PartidaDetalle>>(json);
                return partidasDetalle ?? new List<PartidaDetalle>();
            }
            Directory.CreateDirectory(Carpeta);
            return new List<PartidaDetalle>();
        }
        public PartidaDetalle WritePartidaDetalle(PartidaDetalle partidaDetalle)
        {
            List<PartidaDetalle> partidasDetalle = GetAll();
            int index = partidasDetalle.FindIndex(pd => pd.IdPartida == partidaDetalle.IdPartida);
            if (index >= 0)
            {
                partidasDetalle[index] = partidaDetalle;
            }
            else
            {
                partidasDetalle.Add(partidaDetalle);
            }
            string json = JsonConvert.SerializeObject(partidasDetalle, Formatting.Indented);
            File.WriteAllText(Archivo, json);
            return partidaDetalle;
        }
    }
}
