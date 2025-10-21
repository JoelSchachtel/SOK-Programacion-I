namespace EstancieroEntity
{
    public class CasilleroTablero
    {
        public int NroCasillero { get; set; }
        public int TipoCasillero { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public double? PrecioCompra { get; set; }
        public double? PrecioAlquiler { get; set; }
        public double? Monto { get; set; }
        public double? MontoSancion { get; set; }
        public string? DniPropietario { get; set; }
    }
}
