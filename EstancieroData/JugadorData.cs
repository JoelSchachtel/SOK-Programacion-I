using Newtonsoft.Json;
using System;
using System.IO;

namespace EstancieroData
{
    public class JugadorData
    {
        private string Direccion { get; set; }

        public JugadorData()
        {
            Direccion = Path.GetFullPath(Path.Combine("../EstancieroData/Data", "jugadores.json"));
        }

        public List<Jugador> GetAll()
        {
            if (File.Exists(Direccion))
            {
                string json = File.ReadAllText(Direccion);
                var jugadores = JsonConvert.DeserializeObject<List<Jugador>>(json);
                return jugadores ?? new List<Jugador>();
            }
            Directory.CreateDirectory(Path.GetFullPath("../EstancieroData/Data"));
            return new List<Jugador>();
        }

        public class Jugador
        {
            public int DNI { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }

        public Jugador EscribirJugador(Jugador jugador)
        {
            var jugadores = GetAll();
            jugadores.Add(jugador);
            string json = JsonConvert.SerializeObject(jugadores, Formatting.Indented);
            File.WriteAllText(Direccion, json);
            return jugador;
        }
    }
}

// Entity - DTO / Request / Response - Data - Service - API
// Referencias actualizadas: WebAPI -> Service, Request, Response -- Service -> Data, Entity, Response -- Data -> Entity

