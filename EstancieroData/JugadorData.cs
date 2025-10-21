using Newtonsoft.Json;
using EstancieroEntity;

namespace EstancieroData
{
    public class JugadorData
    {
        private string Carpeta { get; }
        private string Archivo { get; }
        public JugadorData()
        {
            Carpeta = Path.GetFullPath(Path.Combine("../EstancieroData/Data"));
            Archivo = Path.Combine(Carpeta, "usuarios.json");
        }
        public List<Usuario> GetAll()
        {
            if (File.Exists(Archivo))
            {
                var json = File.ReadAllText(Archivo);
                return JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new List<Usuario>();
            }
            Directory.CreateDirectory(Carpeta);
            return new List<Usuario>();
        }
        public void WriteAll(List<Usuario> usuarios)
        {
            Directory.CreateDirectory(Carpeta);
            File.WriteAllText(Archivo, JsonConvert.SerializeObject(usuarios, Formatting.Indented));
        }
    }
    public class Usuario
    {
        public int DNI { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
// Entity - DTO / Request / Response - Data - Service - API
// Referencias actualizadas: WebAPI -> Service, Request, Response -- Service -> Data, Entity, Response -- Data -> Entity

