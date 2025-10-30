using System;
namespace EstancieroEntity
{
    public class Movimiento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Tipo { get; set; }
        public double? Monto { get; set; }
        public int? Casillero { get; set; }
    }
}