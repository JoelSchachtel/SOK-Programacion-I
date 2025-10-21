namespace EstancieroEntity
{
    public class CasilleroTablero
    {
        public int NroCasillero { get; set; }
        public int TipoCasillero { get; set; }// 0-inicio, 1-provincia, 2-multa, 3-premio
        public string Nombre { get; set; } = string.Empty;
        public double? PrecioCompra { get; set; }
        public double? PrecioAlquiler { get; set; }
        public double? Monto { get; set; }
        public double? MontoSancion { get; set; }
        public string? DniPropietario { get; set; }
    }
}
