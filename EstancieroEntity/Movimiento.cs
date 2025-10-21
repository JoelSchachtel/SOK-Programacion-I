using System;
namespace EstancieroEntity
{
    public class Movimiento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Tipo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public double Monto { get; set; }
        public int CasilleroOrigen { get; set; }
        public int CasilleroDestino { get; set; }
        public int? DniJugadorAfectado { get; set; }
    }
}