using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntity
{
    public enum EstadoPartida
    {
        EnJuego,
        Finalizada,
        Suspendida
    }

    public enum TipoCasillero
    {
        Provincia,
        Multa,
        Premio,
        Inicio
    }

    public enum MotivoVictoria
    {
        ProvinciasCompradas,
        UnicoSaldoPositivo,
        JugadorConMayorSaldo
    }
}
