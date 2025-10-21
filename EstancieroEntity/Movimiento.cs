using System;
namespace EstancieroEntity
{
    public class Movimiento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Tipo { get; set; }  // Tipo: 0-MovimientoDado, 1-CompraProvincia, 2-PagoAlquiler, 3-Multa, 4-Premio
        public string Descripcion { get; set; } = string.Empty;
        public double Monto { get; set; }
        public int CasilleroOrigen { get; set; }
        public int CasilleroDestino { get; set; }
        public int? DniJugadorAfectado { get; set; }
    }
}