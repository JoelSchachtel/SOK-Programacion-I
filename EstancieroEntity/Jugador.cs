namespace EstancieroEntity
{
    public class Jugador
    {
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public List<Movimiento> HistorialMovimientos { get; set; }
        public int PartidasJugadas { get; set; }
        public int PartidasGanadas { get; set; }
        public int PartidasPerdidas { get; set; }
        public int PartidasPendientes { get; set; }

        public Jugador(string dni, string nombre, string email)
        {
            DNI = dni;
            Nombre = nombre;
            Email = email;
            HistorialMovimientos = new List<Movimiento>();
            PartidasJugadas = 0;
            PartidasGanadas = 0;
            PartidasPerdidas = 0;
            PartidasPendientes = 0;
        }
    }
}
