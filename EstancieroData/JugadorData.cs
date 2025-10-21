using Newtonsoft.Json;
using System.IO;

namespace EstancieroData
{
    public class JugadorData
    {
        private string Direccion { get; set; }

        public JugadorData()
        {
            Direccion = Path.Combine(Directory.GetCurrentDirectory(), "Data", "jugadores.json");
        }
        public List<Jugador> GetAll()
        {
            if (File.Exists(Direccion))
            {
                string json = File.ReadAllText(Direccion);
                var jugadores = JsonConvert.DeserializeObject<List<Jugador>>(json);
                return jugadores ?? new List<Jugador>();
            }

            Directory.CreateDirectory(Path.GetDirectoryName("../EstancieroData/Data"));
            return new List<Jugador>();
        }

        public class Jugador
        {
            public int DNI { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }
    }
} 

// Entity - DTO / Request / Response - Data - Service - API
// Referencias actualizadas: WebAPI -> Service, Request, Response -- Service -> Data, Entity, Response -- Data -> Entity

