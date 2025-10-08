using Newtonsoft.Json;
using EstancieroEntity;
namespace EstancieroData
{
    public class JugadorData
    {
        private string Carpeta { get; set; }
        private string Archivo { get; set; }
        public JugadorData()
        {
            Carpeta = Path.GetFullPath(Path.Combine("../EstancieroData/Data"));
            Archivo = Path.Combine(Carpeta, "jugadores.json");
        }
        public List<Jugador> GetAll()
        {
            if (File.Exists(Archivo))
            {
                string json = File.ReadAllText(Archivo);
                var Jugadores = JsonConvert.DeserializeObject<List<Jugador>>(json);
                return Jugadores ?? new List<Jugador>();
            }
            Directory.CreateDirectory(Carpeta);
            return new List<Jugador>();
        }
        public Jugador WriteJugador(Jugador jugador)
        {
            List<Jugador> jugadores = GetAll();
            int index = jugadores.FindIndex(j => j.DNI == jugador.DNI);
            if (index >= 0)
            {
                jugadores[index] = jugador;
            }
            else
            {
                jugadores.Add(jugador);
            }
            return jugador;
        }
    }
}
// Entity - DTO / Request / Response - Data - Service - API
// Referencias actualizadas: WebAPI -> Service, Request, Response -- Service -> Data, Entity, Response -- Data -> Entity

